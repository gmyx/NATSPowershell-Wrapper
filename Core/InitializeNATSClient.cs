using NATS.Client.Core;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Xml.Linq;

namespace NATSWrapper.Core
{
    [Cmdlet(VerbsData.Initialize, "NATSClient")]
    [OutputType(typeof(INatsClient))]
    public class InitializeNATSClient : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ComputerName { get; set; }

        [Parameter(
            Position = 1)]
        public int Port { get; set; } = 4222;

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            //send to NATS.core
            var NATSClient = new NATS.Net.NatsClient(url: $"nats://{ComputerName}:{Port}");

            WriteObject(NATSClient);
        }
    }
}