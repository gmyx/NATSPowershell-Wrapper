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

namespace NATSWrapper.JetStream
{
    [Alias(
        VerbsData.Initialize, "NATSJSStream",
        VerbsData.Initialize, "NATSJSPublisher",
        VerbsData.Initialize, "NATSJSStream")]
    [Cmdlet(VerbsData.Initialize, "NATSJetStreamStream")]
    [OutputType(typeof(INatsJSStream))]
    public class InitializeNETSJetStreamStream : Cmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        private INatsJSContext Context { get; set; }

        public INatsClient Client { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true)]
        public string Name { get; set; }

        [Parameter(
            Position = 2,
            Mandatory = true)]
        public ICollection<string> Subjects { get; set; }

        [Parameter(
            Position = 3)]
        [ValidateSet("File", "Memory")]
        public string Storage { get; set; } = "File";

        protected override void ProcessRecord()
        {
            //send to NATS.core
            //create a stream config
            var streamConfig = new StreamConfig(Name, Subjects);
            switch (Storage)
            {
                case "File":
                    streamConfig.Storage = StreamConfigStorage.File;
                    break;

                case "Memory":
                    streamConfig.Storage = StreamConfigStorage.Memory;
                    break;
            }

            //add other optiosn later
            WriteObject(Context.CreateStreamAsync(streamConfig));
        }
    }
}