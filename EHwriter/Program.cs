using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using System.Net;

namespace EHwriter
{
    class Program
    {

        static string eventHubName = "ioteventh";
        static string connectionString = "Endpoint=sb://iotnate.servicebus.windows.net/;SharedAccessKeyName=sender;SharedAccessKey=lQ4yQe61GSoWi2kH38t/1d372cKhIj/EgKk67n4dFPo=";
        
static async Task SendingRandomMessages()
{
    PerformanceCounter cpuCounter;
    
    var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
    var httpclient = new HttpListener();

    while (true)
    {
        try
        {
            //Creates message from GUID time byte
            var message = Guid.NewGuid().ToString();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Timestamp: {0} > Current Temperature: {1}", DateTime.Now.ToString(), message);
            await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            string localmsg = "" + DateTime.Now.ToString() + "," + message;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\naros\Desktop\emp.txt",true))
            {
                file.WriteLine(localmsg);
            }
            
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} > Exception: {1}", DateTime.Now.ToString(), exception.Message);
            Console.ResetColor();
        }

        await Task.Delay(200);
    }
}
        
        static void Main(string[] args)
        {
            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
            SendingRandomMessages().Wait();
        }

         public object getCPUCounter()
    {

        PerformanceCounter cpuCounter = new PerformanceCounter();
        cpuCounter.CategoryName = "Processor";
        cpuCounter.CounterName = "% Processor Time";
        cpuCounter.InstanceName = "_Total";

                     // will always start at 0
        dynamic firstValue = cpuCounter.NextValue();
        System.Threading.Thread.Sleep(1000);
                    // now matches task manager reading
        dynamic secondValue = cpuCounter.NextValue();

        return secondValue;

    }
    }
}
