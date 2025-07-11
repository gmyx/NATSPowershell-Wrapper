using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace NATSWrapper.JetStream
{
    [Alias(VerbsCommon.Remove + "-NATSJSStream")]
    [Cmdlet(VerbsCommon.Remove, "NATSJetStreamStream")]
    public class RemoveNATSJetStreamStream : Cmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsJSContext Context { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsJSStream Stream { get; set; }

        protected override void ProcessRecord()
        {
            Context.DeleteStreamAsync(Stream.Info.Config.Name);
        }
    }
}