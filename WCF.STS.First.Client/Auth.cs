using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WCF.STS.First.Client
{
    public class Auth
    {
        private static SecurityToken _authToken;
        public static ClaimsPrincipal UserIdenity { get; private set; }
        public static void AuthInSts(string userName, string userPassword)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);
            var rst = new RequestSecurityToken(RequestTypes.Issue);
            rst.AppliesTo = new EndpointReference("https://RelyingParty/*");
            rst.KeyType = KeyTypes.Bearer;

            using (var trustChannelFactory = new WSTrustChannelFactory("STSBinding"))
            {
                trustChannelFactory.Credentials.UserName.UserName = userName;
                trustChannelFactory.Credentials.UserName.Password = userPassword;

                var channel = (WSTrustChannel)trustChannelFactory.CreateChannel();
                try
                {
                    _authToken = channel.Issue(rst);
                }
                catch (MessageSecurityException ex)
                {
                    channel.Abort();
                    throw new SecurityTokenException(ex.InnerException.Message, ex);
                }
                UserIdenity = CreateUserIdentityFromSecurityToken(_authToken);
            }
        }

        private static ClaimsPrincipal CreateUserIdentityFromSecurityToken(SecurityToken token)
        {
            var genericToken = token as GenericXmlSecurityToken;
            var handlers = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                    .SecurityTokenHandlerCollectionManager.SecurityTokenHandlerCollections.First();
            var cToken = handlers.ReadToken(new XmlTextReader(new StringReader(genericToken.TokenXml.OuterXml)));
            var identity = handlers.ValidateToken(cToken).First();
            var userIdenity = new ClaimsPrincipal(identity);
            return userIdenity;

        }
    }
}
