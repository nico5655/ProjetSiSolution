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
    /// <summary>
    /// Represents the distant Arduino.
    /// </summary>
    public class Robot : IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new Robot object with default values.
        /// </summary>
        public Robot()
        {
            Done = true;
        }

        private SerialPort port;
        /// <summary>
        /// Number of the serial port to use to communicate with the Arduino.
        /// </summary>
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

        /// <summary>
        /// Action to take when the transmission is done.
        /// </summary>
        private Action action = () => { };

        /// <summary>
        /// Is the transmition done?
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// Called when data is received from the Arduino.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Write a line on the serial port and make sure it is received.
        /// </summary>
        /// <param name="line">Line to send.</param>
        /// <returns></returns>
        public bool WriteLine(string line)
        {
            try
            {
                if (!port.IsOpen)//can't write if it isn't open.
                    return false;
                bool done = false;
                action = () => done = true;
                int cpt = 0;
                while (!done)//while it is not received, we keep writing.
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

        /// <summary>
        /// Sets the motors speed on the robot.
        /// </summary>
        /// <param name="speed">Desired speed for the ball.</param>
        /// <returns></returns>
        public bool SetSpeed(double speed)
        {
            return WriteLine($"speed={speed.ToString().Replace(",", ".")};");
        }

        /// <summary>
        /// Sets the model servo angle on the robot.
        /// </summary>
        /// <param name="angle">Desired downAngle.</param>
        /// <returns></returns>
        public bool SetDownAngle(double angle)
        {
            return WriteLine($"angle={angle.ToString().Replace(",", ".")};");
        }

        /// <summary>
        /// Sets the ball shoot angle on the model.
        /// </summary>
        /// <param name="angle">Ballistic shoot angle (will be converted to TigeLength).</param>
        /// <returns></returns>
        public bool SetShootAngle(double angle)
        {
            double len = Ballistique.GetTigeLength(angle);
            return WriteLine($"length={len.ToString().Replace(",", ".")};");
        }

        /// <summary>
        /// Drops the ball (starting the shoot).
        /// </summary>
        /// <returns></returns>
        public bool DropBall()
        {
            return WriteLine($"fire=1;");
        }

        /// <summary>
        /// Start speed motors turn.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Stop speed motors.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// True if the speed motors are turning.
        /// </summary>
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
