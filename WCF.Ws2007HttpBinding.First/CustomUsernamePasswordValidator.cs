using System;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace WCF.Ws2007HttpBinding.First
{
    public class CustomUsernamePasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            if (string.Equals(userName, "test", StringComparison.InvariantCultureIgnoreCase) && string.Equals(password, "test", StringComparison.InvariantCultureIgnoreCase))
                return;
            else
                throw new FaultException("Invalid user");
        }
    }
}