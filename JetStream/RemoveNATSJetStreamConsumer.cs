using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace NATSWrapper.JetStream
{
    [Alias(VerbsCommon.Remove + "-NATSJSConsumer")]
    [Cmdlet(VerbsCommon.Remove, "NATSJetStreamConsumer")]
    public class RemoveNATSJetStreamConsumer : Cmdlet
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

        [Parameter(
            Position = 2,
            Mandatory = true)]
        public INatsJSConsumer Consumer { get; set; }

        protected override void ProcessRecord()
        {
            _ = Context.DeleteConsumerAsync(Stream.Info.Config.Name, Consumer.Info.Config.Name);
        }
    }
}