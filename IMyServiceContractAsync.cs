using System;
using System.ServiceModel;

namespace WcfAsyncPattern
{
    [ServiceContract(Name = "IMyServiceContract")]
    public interface IMyServiceContractAsync
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMyServiceOperation(string val, AsyncCallback callback, object state);

        string EndMyServiceOperation(IAsyncResult res);
    }
}