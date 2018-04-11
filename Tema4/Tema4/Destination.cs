using System;
using System.Threading;

namespace SlidingWindow {
    public class Destination {
        public string Buffer { get; set; } = string.Empty;

        public string MessageToReceive{ get; set; } = string.Empty;

        public void ReceivePackage(Package package) {
            lock (package) {
                Console.WriteLine($"Destination received the following package: {package}");

                MessageToReceive += package.Message;

                UpdatePackageInfo(package);

            }
        }

        private static void UpdatePackageInfo(Package package) {
            if (package.F >= 0) {
                package.X += package.F;
            }
            package.F = GenerateF();
            package.ACK = 1;
            package.SYN = 1;
            package.Message = string.Empty;
        }

        private static int GenerateF() {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(5);
        }
    }
}