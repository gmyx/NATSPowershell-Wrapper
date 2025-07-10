# NATSPowershell-Wrapper
A quick and dirty powershell for NATS.Nets

# Implementation notes
* reading from a stream is only do-able via JetStream - non JetStream will require events to be implemented later
* * only string support for now - objects to be added at a later time

# Goal is be able to send, receive and manage NATS and JetStream with Powershell commands aiming for 100% coverage

# basic instructions

## send a message to a subject
```
$Client = Initialize-NATSClient -ComputerName {name or IP} -Port {4222 is default} #Return value of NATS.Net INatsClient

Send-NATSMessage -Client $Client -Subject Test.One -Message "Hello NATS"

Close-NATSClient -Client $Client
```

## receive JetStream Message (Using Next Message)
```
$Client = Initialize-NATSClient -ComputerName {name or IP} -Port {4222 is default} #Return value of NATS.Net INatsClient

$Context = Initialize-NATSJetStreamContext -Client $Client

$Consumer = Initialize-NATSJetStreamConsumer -Context $Context -Stream ExampleStream

$Message = Receive-NATSJetStreamNextMessage -Consumer $Consumer #return value is a struct

write-host Subject is $Message.Subject
write-host message is $Message.Message
```