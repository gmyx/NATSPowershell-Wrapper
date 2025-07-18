using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NATSWrapper.JetStream
{
    [Alias(VerbsData.Initialize + "-NATSJSFetchMessage")]
    [Cmdlet(VerbsCommunications.Receive, "NATSJetStreamFetchMessage")]
    [OutputType(typeof(NATSMessage[]))]
    public class ReceiveNATSJetStreamFetchMessages : Cmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsJSConsumer Consumer { get; set; }

        [Parameter(
            Position = 1)]
        public SwitchParameter noack { get; set; } = false;

        [Parameter]
        public int Timeout { get; set; } = 500;

        [Parameter]
        public int MaxMessages { get; set; } = 100;

        [Parameter]
        public SwitchParameter NoWait { get; set; } = false;

        private async Task<NATSMessage[]> GetMessagesNoWait(CancellationToken token)
        {
            NATSMessage[] result = [];
            await foreach (var next in Consumer.FetchNoWaitAsync<string>(new NatsJSFetchOpts { MaxMsgs = MaxMessages }))
            {
                if (noack == false) _ = next.AckAsync();
                result = [.. result, new NATSMessage(next.Subject, next.Data)];
            }

            return result;
        }

        private async Task<NATSMessage[]> GetMessagesWait(CancellationToken token)
        {
            NATSMessage[] result = [];
            await foreach (var next in Consumer.FetchAsync<string>(new NatsJSFetchOpts { MaxMsgs = MaxMessages, Expires = new TimeSpan(0, 0, 0, 0, Timeout) }))
            {
                if (noack == false) _ = next.AckAsync();
                result = [.. result, new NATSMessage(next.Subject, next.Data)];
            }

            return result;
        }

        protected override void ProcessRecord()
        {
            //setup cancellation token, and send if timeout is hit
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task<NATSMessage[]> getNext;

            switch (NoWait.ToBool())
            {
                case true:

                    getNext = GetMessagesNoWait(token);

                    break;

                case false:
                    getNext = GetMessagesWait(token);

                    break;
            }

            //wait for and process results
            bool success = Task.WaitAll([getNext], Timeout, token);

            if (success == true)
            {
                WriteObject(getNext.Result); //could be blank
            }
            else
            {
                source.Cancel();
            }
        }
    }
}