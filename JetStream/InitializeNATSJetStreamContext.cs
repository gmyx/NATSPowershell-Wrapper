using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NATSWrapper.JetStream
{
    [Alias(VerbsData.Initialize + "-NATSJSContext")]
    [Cmdlet(VerbsData.Initialize, "NATSJetStreamContext")]
    [OutputType(typeof(INatsJSContext))]
    public class InitializeNATSJetStreamContext : PSCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsClient Client { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Client.CreateJetStreamContext());
        }
    }
}