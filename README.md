# QueueView

## Overview
QueueView is a linux-style command line app for interacting with Azure Service Bus written in .NET Core.

## Getting Started
QueueView uses a connection string to interact with Azure Service Bus. It does so by maintaining a list of 'connections' in a config file in the user's home folder.

### Add a connection
To save a new connection, provide a friendly name and the connection string using the `connections` verb:
```console
sean@computer $ qv connections -n servicebus-prod -c "Endpoint=sb://NAMESPACE.servicebus.windows.net/;SharedAccessKeyName=KEYNAME;SharedAccessKey=KEY"
```

### View saved connections
To see the newly added connection, use the `connections` verb without any arguments.
```console
sean@computer $ qv connections
servicebus-prod Endpoint=sb://NAMESPACE.servicebus.windows.net/;SharedAccessKeyName=KEYNAME;SharedAccessKey=KEY
sean@computer $
```

### Read messages from a queue
Now that there is a saved connection, it is possible to read messages. QueueView handles Azure Service Bus queues and subscriptions. Queues use a producer-consumer model, while subscriptions use a publisher-subscriber model.

To read from a queue, use the 'messages' verb. Reading from a queue requires two bits of information: the name of a saved connection and the name of the queue.
```console
sean@computer $ qv messages --connection servicebus-prod --queue status-updates
MessageId, SeqNum, Body, User Properties
au6d34, 123, {"title": "I got a dog!", "content": "His name is Buddy, and he's the cutest. :)"}, {}
sean@computer $
```

By default, the messages print in a pseudo-CSV format. The messages are printed one message per line, and additional metadata is provided such as the sequence number of the message. You can specify which fields you would like to see using the `--fields` argument.
```console
sean@computer $ qv messages --connection servicebus-prod --queue status-updates --fields Body
{"title": "I got a dog!", "content": "His name is Buddy, and he's the cutest. :)"}
sean@computer $
```

### Chaining with other tools
This lends itself to easier parsing and formatting via chaining with other command line utilities such as `grep` or `jq`.
```console
sean@computer $ qv messages --connection servicebus-prod --queue status-updates --fields Body | jq '.'
{
  "title": "I got a dog!",
  "content": "His name is Buddy, and he's the cutest. :)"
}
sean@computer $
```

## Permissions
QueueView can only do as much as the permissions of the connection string allow. If the connection string is an administrator-level connection string, you will be able to see metadata about queues and write to queues or topics. If the connection string only provides read-only permissions, then it will only be possible to peek or dequeue messages.

## Connections
- [x] Get or set the default connection. Use `.` to get. `qv connections -d <connection-name>`
- [x] List the connections `qv connections`
- [x] Add a connection `qv connections -s <connection-string> -n <connection-name>`
- [x] Update a connection `qv connections -s <connection-string> -n <connection-name> -u`
- [x] Delete a connection `qv connections -D <connection-name>`

## Queues
- [x] Get or set the default queue. Use `.` to get. `qv queues -d <queue-name>`
- [x] List the queues in the default connection `qv queues`
- [x] List the queues in the specified connection `qv queues -c <connection-name>`

## Topics
- [x] Get or set the default topic. Use `.` to get. `qv topics -d <topic-name>`
- [x] List the topics in the default connection `qv topics`
- [x] List the topics in the specified connection `qv topics -c <connection-name>`

## Subscriptions
- [x] Get or set the default subscription. Use `.` to get. `qv subscriptions -d <subscription-name>`
- [x] List the subscriptions in the default topic using the default connection `qv subscriptions`
- [x] List the subscriptions in the specified topic using the default connection `qv subscriptions -t <topic-name>`
- [x] List the subscriptions in the specified topic using the specified connection `qv subscriptions -c <connection-name> -t <topic-name>`

## Messages
- [x] List the messages in the specified queue `qv messages -q <queue-name>`
- [x] List the messages in the specified subscription `qv messages -s <subscription-name>`
- [x] List the messages in the specified topic and subscription `qv messages -t <topic-name> -s <subscription-name>`
- [x] List the deadletter messages in the specified queue or subscription `qv messages [...] -d`
- [x] List a specified number of messages in the specified queue or subscription `qv messages [...] -n <count>`
- [x] List the messages in a table format `qv messages [...] -p`
- [x] List only the specified fields for messages `qv messages [...] -f field1,field2`

## Send
- [x] Send messages to a queue `qv send [...] -C <destination-connection> -Q <destination-queue>`
- [x] Send messages to a topic `qv send [...] -C <destination-connection> -T <destination-topic>`
- [x] Send messages from a file (one message per line) `qv send [...] -f <file-name>`
- [x] Send messages from standard input (one message per line) `echo 'message' | qv send [...] -i`
- [x] Send messages read from a Service Bus queue `qv send -c <source-connection> -q <source-queue> [...]`
- [x] Send messages read from a Service Bus subscription `qv send -c <source-connection> -t <source-topic> -s <source-subscription> [...]`
- [x] Consume messages as you read them from Service Bus `qv send [...] -D`
- [x] Send messages read from a dead letter queue in Service Bus `qv send [...] -d`

## Status
- [x] Show metadata about the specified queue in the default connection `qv status -q <queue-name>`
- [x] Show metadata about the specified queue in the specified connection `qv status -c <connection-name> -q <queue-name>`
- [x] Show metadata about the specified subscription in the default topic and default connection `qv status -s <subscription-name>`
- [x] Show metadata about the specified subscription in the default topic and specified connection `qv status -c <connection-name> -s <subscription-name>`
- [x] Show metadata about the specified subscription in the specified topic and specified connection `qv status -c <connection-name> -t <topic-name> -s <subscription-name>`
