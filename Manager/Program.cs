using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = new Task(() =>
            {
                var host = new ServiceHost(typeof(WCFService));
                host.Open();
            });
            task.Start();
            Console.ReadLine();
        }
    }
}
