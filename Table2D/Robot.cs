using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ProjetSI
{
    public class Robot : IDisposable, INotifyPropertyChanged
    {
        public Robot()
        {
            Done = true;
        }

        private SerialPort port;
        public string PortNumber
        {
            get => port?.PortName;
            set
            {
                try
                {
                    SerialPort port1 = new SerialPort(value)
                    {
                        WriteTimeout = 5000,
                        BaudRate = 9600,
                    };
                    port1.DataReceived += Port_DataReceived;
                    port1.Open();
                    if (port != null)
                        port.DataReceived -= Port_DataReceived;
                    Text = "";
                    port?.Dispose();
                    Started = false;
                    Done = true;
                    Ready?.Invoke(this, EventArgs.Empty);
                    if (port != null)
                        Text += "Closed port" + Environment.NewLine;
                    port = port1;
                    Text += "Open port" + Environment.NewLine;
                    Notify();
                }
                catch (Exception ex)
                {
                    Text += ex.Message + Environment.NewLine;
                }
            }
        }

        private Action action = () => { };
        public bool Done { get; set; }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = port.ReadLine().Replace("\r", "");
            if (line == "received")
            {
                action();
            }
            else if (line == "done")
            {
                MainWindow.DipThread.Invoke(() =>
                {
                    Thread.Sleep(50);
                    Done = true;
                    Ready?.Invoke(this, new EventArgs());
                });
            }
            else
                Text += line + Environment.NewLine;
        }

        private string text;
        public string Text
        {
            get => text; set
            {
                text = value;
                Notify();
            }
        }

        public bool WriteLine(string line)
        {
            try
            {
                if (!port.IsOpen)
                    return false;
                bool done = false;
                action = () => done = true;
                int cpt = 0;
                while (!done)
                {
                    port.WriteLine(line);
                    Thread.Sleep(40);
                    cpt++;
                    if (cpt > 125)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetSpeed(double speed)
        {
            return WriteLine($"speed={speed.ToString().Replace(",", ".")};");
        }

        public bool SetDownAngle(double angle)
        {
            return WriteLine($"angle={angle.ToString().Replace(",", ".")};");
        }

        public bool SetShootAngle(double angle)
        {
            double len = Ballistique.GetTigeLength(angle);
            return WriteLine($"length={len.ToString().Replace(",", ".")};");
        }

        public bool DropBall()
        {
            return WriteLine($"fire=1;");
        }

        public bool Start()
        {
            try
            {
                if (port.IsOpen)
                {
                    port.WriteLine($"start=1;");
                    Started = true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                if (port.IsOpen)
                {
                    port.WriteLine($"start=0;");
                    Started = false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            ((IDisposable)port).Dispose();
        }

        private bool started;
        public bool Started
        {
            get => started; set
            {
                started = value;
                Notify();
            }
        }
        public event Action<object, EventArgs> Ready;
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify([CallerMemberName] string str = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
    }
}
