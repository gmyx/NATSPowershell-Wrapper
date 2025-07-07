using NATS.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NATSWrapper.Core
{
    [Cmdlet(VerbsCommon.Close, "NATSClient")]
    [OutputType(typeof(Nullable))]
    public class CloseNATSClient : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsClient Client { get; set; }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            //send to NATS.core
            //var natsClient = new NATS.Net.NatsClient();
            _ = Client.DisposeAsync();
        }
    }
}