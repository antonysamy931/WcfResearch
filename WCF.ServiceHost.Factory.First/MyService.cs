using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace WCF.ServiceHost.Factory.First
{
    public class MyService : IMyService
    {

        public string GetMessage()
        {
            return "Success";
        }
    }

    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string GetMessage();
    }
}