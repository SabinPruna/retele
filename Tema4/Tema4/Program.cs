using System;
using System.Threading;

namespace SlidingWindow {
    public class Program {
        private static void Main() {
            Source source = new Source();
            Destination destination = new Destination();
            Package package = new Package();

            while (package.FIN == 0) {
                Thread sourceThread = new Thread(() => source.SendPackage(package));
                sourceThread.Start();
                sourceThread.Join();

                Thread destinationThread = new Thread(() => destination.ReceivePackage(package));
                destinationThread.Start();
                destinationThread.Join();
            }

            Console.WriteLine($"The message sent: {source.MessageToSend}");
            Console.WriteLine($"The message received: {destination.MessageToReceive}");
        }
    }
}