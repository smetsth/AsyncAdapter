using System.ServiceModel;

namespace WcfAsyncPattern
{
    [ServiceContract]
    public interface IMyServiceContract
    {
        [OperationContract]
        string MyServiceOperation(string val);
    }
}