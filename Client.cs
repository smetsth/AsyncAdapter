using System;

namespace WcfAsyncPattern
{
    public class Client
    {
        public static void Main()
        {
            CallService();
        }

        private static void CallService()
        {
            var sa = new MyServiceAdapter();
            sa.MyServiceOperation("input", EndCallService);
        }

        private static void EndCallService(Exception ex, string result)
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