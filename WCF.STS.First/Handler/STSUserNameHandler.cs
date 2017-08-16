using System;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace WCF.STS.First.Handler
{
    public class STSUserNameHandler : UserNameSecurityTokenHandler
    {
        public override bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        public override Type TokenType
        {
            get
            {
                return typeof(UserNameSecurityToken);
            }
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return new string[]
            {
                SecurityTokenTypes.UserName
            };
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            if (token == null)
            {
                throw new FaultException("Incorrect username or password");
            }
            UserNameSecurityToken userToken = token as UserNameSecurityToken;
            ValidateUserCredentials(userToken);
            return new ReadOnlyCollection<ClaimsIdentity>(ExtractClaims(userToken));
        }

        private void ValidateUserCredentials(UserNameSecurityToken token)
        {
            if (!token.UserName.Equals(token.Password))
            {
                throw new FaultException("Incorrect username or password");
            }
        }

        private IList<ClaimsIdentity> ExtractClaims(UserNameSecurityToken token)
        {
            ClaimsIdentity outgoingIndentity = new ClaimsIdentity("UserAuthenticate");
            outgoingIndentity.Label = "UserAuthenticate";
            outgoingIndentity.AddClaim(new Claim(ClaimTypes.Name, token.UserName));
            outgoingIndentity.AddClaim(new Claim(ClaimTypes.Email, "antony.samy@ravsoftsolutions.com"));
            var identities = new List<ClaimsIdentity> { outgoingIndentity };
            return identities;
        }
    }
}