using NATS.Client.JetStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NATSWrapper.JetStream
{
    public struct NATSMessage
    {
        public string Subject;
        public string Message;

        internal NATSMessage(string _subject, string _message)
        {
            Subject = _subject;
            Message = _message;
        }
    }

    [Alias(VerbsData.Initialize + "-NATSJSNextMessage")]
    [Cmdlet(VerbsCommunications.Receive, "NATSJetStreamNextMessage")]
    [OutputType(typeof(NATSMessage))]
    public class ReceiveNATSJetStreamNextMessage : Cmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public INatsJSConsumer Consumer { get; set; }

        protected override void ProcessRecord()
        {
            Task<NATSMessage> task = Task.Run(async () =>
            {
                NatsJSMsg<string>? next = await Consumer.NextAsync<string>();

                if (next is { } msg)
                {
                    await msg.AckAsync();
                    return new NATSMessage(msg.Subject, msg.Data);
                }

                return new NATSMessage("", ""); // no message
            });

            WriteObject(task.Result); //could be blank
        }
    }
}