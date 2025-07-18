using NATS.Client.Core;
using NATS.Client.JetStream;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;

namespace NATSWrapper.Core
{
    [Cmdlet(VerbsCommunications.Send, "NATSMessage")]
    [OutputType(typeof(Nullable))]
    public class SendNATSMessage : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Subject { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Message { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 2,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsClient Client { get; set; }

        private async Task SendMessage(string aSubject, string aMessage)
        {
            await Client.PublishAsync(subject: aSubject, data: aMessage);
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            //send to NATS.core
            _ = Task.WaitAll(new Task[] { SendMessage(Subject, Message) }, 500);
        }
    }
}