using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace NATSWrapper.JetStream
{
    [Alias(VerbsData.Initialize + "-NATSJSNextConsumer")]
    [Cmdlet(VerbsData.Initialize, "NATSJetStreamConsumer")]
    [OutputType(typeof(INatsJSConsumer))]
    public class InitializeNATSJetStreamConsumer : Cmdlet
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
        public string Stream { get; set; }

        [Parameter(
            Position = 2)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            ConsumerConfig consumerConfig = new ConsumerConfig(Name);
            //not ideal, but PS is not easliy async - and the command expects a results. Maybe using End would be better
            var task = Task.Run<INatsJSConsumer>(async () => await Context.CreateOrUpdateConsumerAsync(Stream, consumerConfig));

            WriteObject(task.Result);
        }
    }
}