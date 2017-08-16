using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Tokens;

namespace WCF.STS.First
{
    public class STSIssuerNameRegister : IssuerNameRegistry
    {
        private string IssuerThumbprint;
        
        public STSIssuerNameRegister(string issuerThumbprint)
        {
            IssuerThumbprint = issuerThumbprint;
        }

        public override string GetIssuerName(SecurityToken securityToken)
        {
            X509SecurityToken token = securityToken as X509SecurityToken;
            if (token != null)
            {
                if (string.Equals(token.Certificate.Thumbprint, IssuerThumbprint, StringComparison.InvariantCultureIgnoreCase))
                {
                    return token.Certificate.SubjectName.Name;
                }
            }
            throw new SecurityTokenException("Untrusted issuer");
        }
    }
}