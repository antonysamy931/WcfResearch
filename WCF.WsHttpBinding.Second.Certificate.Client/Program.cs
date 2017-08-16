using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCF.WsHttpBinding.Second.Certificate.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Service1Client client = new Service1Client();
            var obj = client.GetData(10);
            Console.Read();
        }
    }
}
