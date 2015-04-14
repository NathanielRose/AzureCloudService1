using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json.Linq;

namespace WorkerRole1
{
  
        public class EventProcessor : IEventProcessor
        {
            Stopwatch checkpointStopWatch;
            PartitionContext partitionContext;

            QueueClient queue;

            public EventProcessor()
            {
                queue = QueueClient.CreateFromConnectionString("Endpoint=sb://iotnate.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+XRBh06RRMu0/Ew/DSEtdOvOXm7fk9401S/AtmAMEAs=","IoTq");
            }

            public async Task CloseAsync(PartitionContext context, CloseReason reason)
            {
                Console.WriteLine(string.Format("Processor Shuting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason.ToString()));
                if (reason == CloseReason.Shutdown)
                {
                    await context.CheckpointAsync();
                }
            }

            public Task OpenAsync(PartitionContext context)
            {
                Console.WriteLine(string.Format("EventProcessor initialize.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));
                this.checkpointStopWatch = new Stopwatch();
                this.checkpointStopWatch.Start();
                return Task.FromResult<object>(null);
            }

            public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
            {
                foreach (EventData eventData in messages)
                {
                    Console.WriteLine("Processing Event Hub Data");

                    string key = eventData.PartitionKey;

                    string data = System.Text.Encoding.Unicode.GetString(eventData.GetBytes());

                    //string data = Encoding.UTF8.GetString(eventData.GetBytes());

                    try
                    {
                        var json = JObject.Parse(data);
                        string text = json["Message"].ToString();
                        string agent = json["BrowserInfo"].ToString();

                        if (queue != null)
                        {
                            await queue.SendAsync(new BrokeredMessage((agent + "##" + text)));

                            Trace.TraceInformation("Added to queue:" + agent);
                        }

                    }

                    catch (Exception except)
                    {
                        Console.WriteLine(except.Message);
                    }

                    Console.WriteLine(string.Format("Message received.  Partition: '{0}', Data: '{1}'",
                        context.Lease.PartitionId, data));


                    Console.WriteLine(string.Format("Message received.  Partition: '{0}', Device: '{1}'",
                            this.partitionContext.Lease.PartitionId, key));
                }
                //Call checkpoint every 5 minutes, so that worker can resume processing from the 5 minutes back if it restarts.
                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
                {
                    await context.CheckpointAsync();

                    lock(this)
                    {
                        this.checkpointStopWatch.Reset();
                    }
                }
            }
            
        }
    }
