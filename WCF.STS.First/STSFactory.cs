using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Web.Configuration;

namespace WCF.STS.First
{
    public class STSFactory : ServiceHostFactory
    {
        private static Binding STSBinging
        {
            get
            {
                return new WS2007HttpBinding("STS-WS2007HttpBinging");
            }
        }

        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            string issuerName = WebConfigurationManager.AppSettings["IssuerName"];
            string signingCertificateThumbPrint = WebConfigurationManager.AppSettings["SigningCertificateThumbprint"];
            string issuerCertificateThumbPrint = WebConfigurationManager.AppSettings["IssuerCertificateThumprint"];
            var config = new STSConfiguration(issuerName, signingCertificateThumbPrint, issuerCertificateThumbPrint);

            Uri baseuri = baseAddresses.FirstOrDefault(x => x.Scheme == Uri.UriSchemeHttps);
            if (baseuri == null)
            {
                throw new FaultException("The STS should be hosed in https");
            }

            WSTrustServiceHost host = new WSTrustServiceHost(config, baseAddresses);
            host.AddServiceEndpoint(typeof(IWSTrust13SyncContract), STSBinging, baseuri.AbsoluteUri);
            return host;
        }
    }
}