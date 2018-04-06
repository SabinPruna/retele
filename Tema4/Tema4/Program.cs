using System.Threading;

namespace SlidingWindow {
    public class Program {
        private static void Main() {
            Source source = new Source();
            Destination destination = new Destination();
            Package package = new Package();

            while (package.FIN == 0) {
                Thread sourceThread = new Thread(() => source.Run(package));
                sourceThread.Start();
                sourceThread.Join();

                Thread destinationThread = new Thread(() => destination.Run(package));
                destinationThread.Start();
                destinationThread.Join();
            }
        }
    }
}