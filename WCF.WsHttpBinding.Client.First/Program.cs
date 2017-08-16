using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace WCF.WsHttpBinding.Client.First
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            ((sender, certificate, chain, sslPolicyErrors) => ServerCertificateValidate(sender, certificate, chain, sslPolicyErrors));
            //ServicePointManager.ServerCertificateValidationCallback =
            //((sender, certificate, chain, sslPolicyErrors) => true);
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            var obj = client.GetData(10);
            var jb = client.GetString("Antony");
            client.Close();
            Console.Read();
        }

        private static bool ServerCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509CertificateCollection collection = store.Certificates;
            bool result = collection.Contains(certificate);
            store.Close();
            return result;
        }

    }
}
