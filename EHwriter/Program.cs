using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Microsoft.WindowsAzure;
using System.Net;
using CommonUtils;
using TestApp.TrollBridgeService;

namespace EHwriter
{
    class Program
    {
        private static TokenHolder _holder;
        static string tbCustId;
        static string tbSiteId;
        static string tbMacAddress;
        static string tbServiceURL;
        static string tbMessage;
        static EventHubSender ehSender;
        static EventHubClient ehClient;
        static string eventHubName = "ioteventh";
        static string connectionString = "Endpoint=sb://iotnate.servicebus.windows.net/;SharedAccessKeyName=sender;SharedAccessKey=lQ4yQe61GSoWi2kH38t/1d372cKhIj/EgKk67n4dFPo=";
        
static async Task SendingRandomMessages()
{
    var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);

    if (_holder.AzureEventHubs == null)
    {
        Console.WriteLine("Sorry, but there is nothing for me to send too");
    }
    else
    {
        foreach (ShareAccessToken sat in _holder.AzureEventHubs)
        {
            try
            {
                string sas = CommonFunc.DecompressString(sat.SasToken);

                var connStr = ServiceBusConnectionStringBuilder.CreateUsingSharedAccessSignature(sat.Uri, sat.Name, "sender", sas);
                //ehClient = EventHubClient.CreateFromConnectionString(connStr,sat.Name);
                ehSender = EventHubSender.CreateFromConnectionString(connStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    while (true)
    {
        try
        {
            //Creates message from GUID time byte
            var message = Guid.NewGuid().ToString();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Timestamp: {0} > Current Time: {1}", DateTime.Now.ToString(), message);
            tbMessage = "" + DateTime.Now.ToString() + "," + message;

            await ehSender.SendAsync(new EventData(Encoding.UTF8.GetBytes(tbMessage)));
            //await ehClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(tbMessage)));

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\naros\Desktop\emp.txt",true))
            {
              file.WriteLine(tbMessage);
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
            Console.WriteLine("Welcome and Thank you for using the Troll Bridge Client Solution Example");
            Console.WriteLine("To access your Azure Service please enter the following...");
            Console.WriteLine("Please Enter the Customer ID:");
            tbCustId = Console.ReadLine();
            Console.WriteLine("Please Enter the Site ID:");
            tbSiteId = Console.ReadLine();
            Console.WriteLine("Please Enter the MAC Address:");
            tbMacAddress = Console.ReadLine();
            Console.WriteLine("Please Enter your Troll Bridge Service URL");
            //tbServiceURL = Console.ReadLine();
            Console.ReadLine();
            tbServiceURL = "http://trollbridge.azurewebsites.net/KeyMaster.svc";
            getTokens();
            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
            SendingRandomMessages().Wait();
        }

         static public void getTokens()
         {
             if (_holder != null && _holder.EarliestTokenExpires.ToUniversalTime() > DateTime.UtcNow.AddMinutes(2)) return;

             try
             {
                 string machineIdentifier = CommonFunc.CompressString(tbCustId + "_" + tbSiteId + "_" + tbMacAddress);
                 KeyMasterClient client = new KeyMasterClient();

                 var endpointUri = string.Format("{0}/soap", tbServiceURL);

                 client.Endpoint.Address = new System.ServiceModel.EndpointAddress(endpointUri);

                 _holder = client.GetTokensForMe(machineIdentifier);
                 client.Close();
                 if (_holder == null)
                 {
                     Console.WriteLine("Sorry, no data returned!");
                     return;
                 }

                 if (_holder.AzureEventHubs.Count() < 1)
                 {
                     Console.WriteLine("Sorry, but you are not authorized!");
                     return;
                 }
             }
             catch (Exception ex)
             {
                 _holder = null;
                 throw ex;
             }
         }
    }
}
