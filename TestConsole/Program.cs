using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetSI;
using System.IO.Pipes;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NamedPipeServerStream pipe= new NamedPipeServerStream("ping/tir764",PipeDirection.InOut))
            {
                pipe.WaitForConnection();
                using (StreamReader sr = new StreamReader(pipe))
                {
                    Console.WriteLine(sr.ReadLine());
                }
            }
            Console.ReadKey();
        }
    }
}
