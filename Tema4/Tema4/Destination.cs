using System;
using System.Threading;

namespace SlidingWindow {
    public class Destination {
        public string Buffer { get; set; } = string.Empty;

        public void Run(Package package) {
            lock (package) {
                package.F = GenerateF();

                while (package.F == 0) {
                    Thread.Sleep(3000);
                    package.F = GenerateF();
                }

                if (package.ACK == 1 && package.SYN == 0) {
                    package.SYN = 1;
                    package.X = 1;
                }
                else {
                    if (package.ACK == 1 && package.SYN == 1 && package.FIN == 1) {
                        Console.WriteLine($"Message: {Buffer}");
                    }
                    else {
                        if (package.ACK == 1 && package.SYN == 1) {
                            Buffer = Buffer + package.Message;
                            Console.WriteLine($"Destination: {package.Message}");
                            package.X = package.X + 1;
                        }
                    }
                }

                Console.WriteLine("Destination: " + package);
            }
        }

        private static int GenerateF() {
            Random rand = new Random();
            return rand.Next(20);
        }
    }
}