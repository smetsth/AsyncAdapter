using System;

namespace WcfAsyncPattern
{
    public class MyServiceAdapter
        : AsyncServiceAdapter<IMyServiceContractAsync>
    {
        public void MyServiceOperation(string val, Action<Exception, string> callback)
        {
            Call((c, a, s) => c.BeginMyServiceOperation(val, a, s),
                 (c, a) => c.EndMyServiceOperation(a),
                 callback);
        }
    }
}