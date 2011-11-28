using System;
using System.ServiceModel;

namespace WcfAsyncPattern
{
    /// <summary>
    /// Contains helper methods to invoke service operations asynchronous
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncServiceAdapter<T>
    {
        public void Call(
            Func<T, AsyncCallback, object, IAsyncResult> beginFunction,
            Action<T, IAsyncResult> endAction,
            Action<Exception> callbackCommand)
        {
            var channel = CreateChannel();

            var state = new ServiceCallState
                            {
                                Channel = channel,
                                EndAction = endAction,
                                CallbackCommand = callbackCommand
                            };

            beginFunction(channel, CallDone, state);
        }

        public void Call<TR>(
            Func<T, AsyncCallback, object, IAsyncResult> beginFunction,
            Func<T, IAsyncResult, TR> endFunction,
            Action<Exception, TR> callbackCommand)
        {
            var channel = CreateChannel();

            var state = new ServiceCallState<TR>
                            {
                                Channel = channel,
                                EndFunction = endFunction,
                                CallbackCommand = callbackCommand
                            };

            beginFunction(channel, CallDone<TR>, state);
        }

        private void CallDone(IAsyncResult asyncResult)
        {
            var serviceCallState = asyncResult.AsyncState as ServiceCallState;

            try
            {
                serviceCallState.EndAction(serviceCallState.Channel, asyncResult);

                serviceCallState.CallbackCommand(null);

                var clientChannel = serviceCallState.Channel as IClientChannel;
                if (null != clientChannel)
                {
                    clientChannel.Close();
                }
            }
            catch (Exception ex)
            {
                serviceCallState.CallbackCommand(ex);

                var clientChannel = serviceCallState.Channel as IClientChannel;
                if (null != clientChannel)
                {
                    clientChannel.Abort();
                }
            }
        }

        private void CallDone<TR>(IAsyncResult asyncResult)
        {
            var serviceCallState = asyncResult.AsyncState as ServiceCallState<TR>;

            try
            {
                TR result = serviceCallState.EndFunction(serviceCallState.Channel, asyncResult);

                serviceCallState.CallbackCommand(null, result);

                var clientChannel = serviceCallState.Channel as IClientChannel;
                if (null != clientChannel)
                {
                    clientChannel.Close();
                }
            }
            catch (Exception ex)
            {
                serviceCallState.CallbackCommand(ex, default(TR));

                var clientChannel = serviceCallState.Channel as IClientChannel;
                if (null != clientChannel)
                {
                    clientChannel.Abort();
                }
            }
        }

        private T CreateChannel()
        {
            var channelFactory = new ChannelFactory<T>(typeof(T).FullName);

            return channelFactory.CreateChannel();
        }

        private class ServiceCallState
        {
            public T Channel { get; set; }
            public Action<Exception> CallbackCommand { get; set; }
            public Action<T, IAsyncResult> EndAction { get; set; }
        }

        private class ServiceCallState<TR>
        {
            public T Channel { get; set; }
            public Action<Exception, TR> CallbackCommand { get; set; }
            public Func<T, IAsyncResult, TR> EndFunction { get; set; }
        }
    }
}