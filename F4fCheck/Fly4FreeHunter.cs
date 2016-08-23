using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace F4fCheck
{
    public partial class Fly4FreeHunter : ServiceBase
    {
        public Fly4FreeHunter()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try 
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 900000; // 15 min
                timer.Elapsed += new System.Timers.ElapsedEventHandler(HuntService.Hunt);
                timer.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}", e.Message, e.StackTrace, e.InnerException));
            }
        }

        public void Start()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
        }
    }
}
