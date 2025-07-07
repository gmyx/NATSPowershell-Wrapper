# NATSPowershell-Wrapper
A quick and dirty powershell for NATS.Nets

#Implementation notes
* reading from a stream is only do-able via JetStream - non JetStream will require events to be implemented later
* * only string support for now - objects to be added at a later time