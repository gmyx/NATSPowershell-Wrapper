using NATS.Client.JetStream;
using System;
using System.Management.Automation;
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

        private async Task<NATSMessage> GetNextMessage()
        {
            NatsJSMsg<string>? next = await Consumer.NextAsync<string>();

            if (next is { } msg)
            {
                if (noack == false) await msg.AckAsync();
                return new NATSMessage(msg.Subject, msg.Data);
            }

            return new NATSMessage("", ""); // no message
        }

        protected override void ProcessRecord()
        {
            try
            {
                var task = AsyncHelper.TimeoutAfter<NATSMessage>(GetNextMessage(), new System.TimeSpan(0, 0, 0, 0, Timeout));
                WriteObject(task.Result); //could be blank
            }
            catch (Exception)
            {
                WriteObject(new NATSMessage("", "")); //we don't care about the timeout error
            }
        }
    }
}