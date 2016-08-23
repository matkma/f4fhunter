using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using F4fCheck;

namespace Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesToRun = new Fly4FreeHunter();

            Console.WriteLine("Starting service...");
            servicesToRun.Start();
            // selfDiagnosticService.Start();
            Console.WriteLine("Service started.");
            Console.WriteLine("Press Enter to stop the service.");
            Console.ReadLine();
            servicesToRun.Stop();
            Console.WriteLine("Service stopped.");
        }
    }
}
