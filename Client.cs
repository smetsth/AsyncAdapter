using System;
using System.ServiceModel;

namespace WcfAsyncPattern
{
    public class Client
    {
        private void CallService()
        {
            var sa = new MyServiceAdapter();
            sa.MyServiceOperation("input", EndCallService);
        }

        private void EndCallService(Exception ex, string result)
        {
            if(null == ex)
            {
                //ToDo UI code when exception occurs
                return;
            }

            //ToDo handle result
        }
    }
}