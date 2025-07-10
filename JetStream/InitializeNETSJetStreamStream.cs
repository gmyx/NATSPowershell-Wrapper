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
        VerbsData.Initialize + "-NATSJSStream",
        VerbsData.Initialize + "-NATSJSPublisher",
        VerbsData.Initialize + "-NATSJSStream")]
    [Cmdlet(VerbsData.Initialize, "NATSJetStreamStream")]
    [OutputType(typeof(INatsJSStream))]
    public class InitializeNETSJetStreamStream : Cmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsJSContext Context { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true)]
        public string Name { get; set; }

        [Parameter(
            Position = 2,
            Mandatory = true)]
        public string[] Subjects { get; set; }

        [Parameter(
            Position = 3)]
        [ValidateSet("File", "Memory")]
        public string Storage { get; set; } = "File";

        [Parameter(
            Position = 4)]
        public SwitchParameter noack { get; set; } = false;

        [Parameter]
        [ValidateSet("Limits", "WorkQueue", "Interest")]
        public string Retention { get; set; } = "Limits";

        protected override void ProcessRecord()
        {
            //send to NATS.core
            //create a stream config
            var streamConfig = new StreamConfig(Name, Subjects);

            //handle storage
            switch (Storage)
            {
                case "File":
                    streamConfig.Storage = StreamConfigStorage.File;
                    break;

                case "Memory":
                    streamConfig.Storage = StreamConfigStorage.Memory;
                    break;
            }

            //rention policy
            switch (Retention)
            {
                case "Limits":
                    streamConfig.Retention = StreamConfigRetention.Limits;
                    break;

                case "WorkQueue":
                    streamConfig.Retention = StreamConfigRetention.Workqueue;
                    break;

                case "Interest":
                    streamConfig.Retention = StreamConfigRetention.Interest;
                    break;
            }

            //other quick items
            streamConfig.NoAck = noack;

            //add other optiosn later
            Task<INatsJSStream> task = Task.Run(async () =>
            {
                return await Context.CreateStreamAsync(streamConfig);
            });

            WriteObject(task);
        }
    }
}