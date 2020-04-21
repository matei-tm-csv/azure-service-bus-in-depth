using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Sender
{
    class SenderConsole
    {
        //ToDo: Enter a valid Serivce Bus connection string
        static string ConnectionString = "";
        static string QueuePath = "demoqueue";


        static void Main(string[] args)
        {

            // Create a queue client
            


            // Send some messages
            for (int i = 0; i < 10; i++)
            {
                var content = $"Message: { i }";



                

                Console.WriteLine("Sent: " + i);
            }

            // Close the client
            
            

            Console.WriteLine("Sent messages...");
            Console.ReadLine();

        }
    }
}
