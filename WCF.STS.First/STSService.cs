using System;
using System.Linq;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WCF.STS.First.Utility;
using System.Web.Configuration;

namespace WCF.STS.First
{
    public class STSService : SecurityTokenService
    {
        private readonly SigningCredentials _signingCredentials;
        private readonly EncryptingCredentials _encryptingCredentials;

        public STSService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
            _signingCredentials = new X509SigningCredentials(
                CertificateUtility.GetCertificateByThumbprint(
                StoreLocation.LocalMachine,
                StoreName.My,
                WebConfigurationManager.AppSettings["SigningCertificateThumbprint"].ToString()));
            if (!string.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings["EncryptingCertificateName"].ToString()))
            {
                _encryptingCredentials = new X509EncryptingCredentials(
                    CertificateUtility.GetCertificateByThumbprint(
                    StoreLocation.LocalMachine, StoreName.My,
                    WebConfigurationManager.AppSettings["EncryptingCertificateName"].ToString()));
            }
        }

        protected override Lifetime GetTokenLifetime(Lifetime requestLifetime)
        {
            if (requestLifetime != null)
            {
                return requestLifetime;
            }
            var lifeTime = new Lifetime(DateTime.Now, DateTime.Now.AddMinutes(30));
            return lifeTime;
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            ClaimsIdentity sourceIdentity = principal.Identities.First();
            ClaimsIdentity destinationIndentity = new ClaimsIdentity("Sample");
            CopyClaim(sourceIdentity, destinationIndentity);
            return destinationIndentity;
        }

        private void CopyClaim(ClaimsIdentity sourceIdentity, ClaimsIdentity destinationIndentity)
        {
            foreach (var claim in sourceIdentity.Claims)
            {
                var newClaim = new Claim(claim.Type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer);
                foreach (var key in claim.Properties.Keys)
                {
                    newClaim.Properties.Add(key, claim.Properties[key]);
                }
                destinationIndentity.AddClaim(newClaim);
            }

            if (sourceIdentity.Actor != null)
            {
                destinationIndentity.Actor = new ClaimsIdentity();
                CopyClaim(sourceIdentity.Actor, destinationIndentity.Actor);
            }
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            Scope scope = new Scope(request.AppliesTo.Uri.ToString(), _signingCredentials);

            if (_encryptingCredentials != null)
            {
                scope.EncryptingCredentials = _encryptingCredentials;
            }
            else
            {
                scope.TokenEncryptionRequired = false;
            }

            if (Uri.IsWellFormedUriString(request.ReplyTo, UriKind.Absolute))
            {
                if (request.AppliesTo.Uri.Host != new Uri(request.ReplyTo).Host)
                {
                    scope.ReplyToAddress = request.AppliesTo.Uri.AbsoluteUri;
                }
                else
                {
                    scope.ReplyToAddress = request.ReplyTo;
                }
            }
            else
            {
                Uri resulturi = null;
                if (Uri.TryCreate(request.AppliesTo.Uri.AbsoluteUri, UriKind.Absolute, out resulturi))
                {
                    scope.ReplyToAddress = resulturi.AbsoluteUri;
                }
                else
                {
                    scope.ReplyToAddress = request.AppliesTo.Uri.AbsoluteUri;
                }
            }

            return scope;
        }

        protected override SecurityTokenDescriptor CreateSecurityTokenDescriptor(RequestSecurityToken request, Scope scope)
        {
            return base.CreateSecurityTokenDescriptor(request, scope);
        }
    }
}