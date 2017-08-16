using System;
using System.Security.Cryptography.X509Certificates;

namespace WCF.STS.First.Utility
{
    public class CertificateUtility
    {
        public static X509Certificate2 GetCertificateByThumbprint(StoreLocation location, StoreName name, string thumbprint)
        {
            X509Certificate2 cert = null;
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);
            certificates = store.Certificates;
            foreach (var c in certificates)
            {
                if (string.Equals(c.Thumbprint, thumbprint, StringComparison.InvariantCultureIgnoreCase))
                {
                    cert = c;
                }
            }
            return cert;
        }
    }
}