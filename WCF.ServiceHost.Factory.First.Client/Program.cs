using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF.ServiceHost.Factory.First.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServiceClient client = new MyServiceClient();
            var st=client.GetMessage();
            Console.Read();
        }
    }
}
