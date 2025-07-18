using NATS.Client.Core;
using NATS.Client.JetStream;
using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace NATSWrapper.JetStream
{
    [Alias(VerbsData.Initialize + "-NATSJSNextMessage")]
    [Cmdlet(VerbsCommunications.Receive, "NATSJetStreamNextMessage")]
    [OutputType(typeof(NATSMessage))]
    public class ReceiveNATSJetStreamNextMessage : Cmdlet
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

        private async Task<NATSMessage> GetNextMessage(CancellationToken token)
        {
            NatsJSMsg<string>? next = await Consumer.NextAsync<string>(cancellationToken: token);

            if (next is { } msg)
            {
                if (noack == false) _ = msg.AckAsync();
                return new NATSMessage(msg.Subject, msg.Data);
            }

            return new NATSMessage("", ""); // no message
        }

        protected override void ProcessRecord()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var getNext = GetNextMessage(token);
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