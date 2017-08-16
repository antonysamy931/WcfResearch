using System.IdentityModel.Tokens;
using System.IdentityModel.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Selectors;
using System.Web.Configuration;
using WCF.STS.First.Utility;
using WCF.STS.First.Handler;

namespace WCF.STS.First
{
    public class STSConfiguration : SecurityTokenServiceConfiguration
    {
        public STSConfiguration()
            : base(WebConfigurationManager.AppSettings["IssuerName"].ToString(),
                new X509SigningCredentials(
                CertificateUtility.GetCertificateByThumbprint(
                StoreLocation.LocalMachine,
                StoreName.My,
                WebConfigurationManager.AppSettings["SigningCertificateThumbprint"].ToString())))
        {
            Init(WebConfigurationManager.AppSettings["IssuerCertificateThumprint"].ToString());
        }

        public STSConfiguration(string piIssuerName, string piSigningCertificateThumbprint, string piIssuerCertificateThumbprint)
            : base(piIssuerName, new X509SigningCredentials(
                CertificateUtility.GetCertificateByThumbprint(
                StoreLocation.LocalMachine,
                StoreName.My, piSigningCertificateThumbprint)))
        {
            Init(piSigningCertificateThumbprint);
        }

        private void Init(string piIssuerCertificateThumbPrint)
        {
            var usernameHandler = new STSUserNameHandler();
            SecurityTokenService = typeof(STSService);
            usernameHandler.Configuration = new SecurityTokenHandlerConfiguration();
            usernameHandler.Configuration.AudienceRestriction = new AudienceRestriction();
            usernameHandler.Configuration.AudienceRestriction.AudienceMode = AudienceUriMode.Never;
            usernameHandler.Configuration.SaveBootstrapContext = true;
            usernameHandler.Configuration.IssuerNameRegistry = new STSIssuerNameRegister(piIssuerCertificateThumbPrint);
            SecurityTokenHandlers.AddOrReplace(usernameHandler);
        }
    }
}