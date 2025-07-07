using NATS.Client.Core;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

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

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            //send to NATS.core
            //var natsClient = new NATS.Net.NatsClient();
            _ = Client.PublishAsync(subject: Subject, data: Message);
        }
    }
}