using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort port = new SerialPort("COM7", 9600);
            port.Open();
            int cpt = 0;
            using (StreamWriter sr = new StreamWriter("truc.csv", false))
            {
                sr.WriteLine("pmw;speed(rps)");
                while (true)
                {
                    string line = port.ReadLine().Replace("\r", "");
                    if (line == "done")
                        break;
                    if (line != "detect")
                    {
                        sr.WriteLine(line.Replace('.', ','));
                        Console.WriteLine(line);
                    }
                }
            }
        }
    }
}
