# QueueView

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
