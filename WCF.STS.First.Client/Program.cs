using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.ServiceModel.Security;
using System.Xml;

namespace WCF.STS.First.Client
{
    class Program
    {        
        static void Main(string[] args)
        {
            Auth.AuthInSts("an", "an");
            Console.Read();
        }
    }
}
