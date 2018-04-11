using System;
using System.IO;
using System.Linq;

namespace SlidingWindow {
    public class Source {
        public string MessageToSend { get; set; } = File.ReadAllLines(@"E:\SlidingWindow.txt").First();

        public void SendPackage(Package package) {
            lock (package) {
                if (IsDestinationAbleToReceive(package)) {
                    if (IsFirstPackage(package)) {
                        SetFirstMessage(package);
                    }
                    else {
                        if (IsPackageReceived(package))
                        {
                            package.Message = package.X + package.F > MessageToSend.Length
                                                  ? MessageToSend.Substring(package.X)
                                                  : MessageToSend.Substring(package.X, package.F);
                        }
                    }

                    if (package.X + package.F > MessageToSend.Length) {
                        package.FIN = 1;
                    }

                    Console.WriteLine("Source sent the following package: " + package);
                }
            }
        }

        private static bool IsDestinationAbleToReceive(Package package) {
            return package.F != 0;
        }

        private static bool IsFirstPackage(Package package) {
            return package.F == -1;
        }

        private static void SetFirstMessage(Package package) {
            package.X = 0;
            package.ACK = 1;
            package.SYN = 0;
            package.Message = string.Empty;
        }

        /// <summary>
        ///     Determines whether [is package resent] [the specified package].
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>
        ///     <c>true</c> if [is package resent] [the specified package]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPackageReceived(Package package) {
            return package.ACK == 1 && package.SYN == 1;
        }
    }
}