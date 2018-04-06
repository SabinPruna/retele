using System;
using System.IO;

namespace SlidingWindow {
    public class Source {
        public string Buffer { get; set; } = string.Empty;

        public void Run(Package package) {
            lock (package) {
                if (package.ACK == 0) {
                    package.ACK = 1;
                    Buffer = GenerateMessage();
                }
                else if (Buffer.Length == 0) {
                    package.FIN = 1;
                }
                else if (package.SYN == 1) {
                    if (Buffer.Length > package.F) {
                        package.Message = Buffer.Substring(0, package.F);
                        Buffer = Buffer.Substring(package.F);
                        package.X = package.X + package.F - 1;
                    }
                    else {
                        package.Message = Buffer;
                        package.X = package.X + Buffer.Length - 1;
                        Buffer = string.Empty;
                    }
                }

                Console.WriteLine("Source: " + package);
            }
        }

        /// <summary>
        ///     Generates the message.
        /// </summary>
        /// <returns></returns>
        private string GenerateMessage() {
            Random random = new Random();
            string[] words = File.ReadAllLines(@"E:\SlidingWindow.txt");

            return words[random.Next(words.Length)];
        }
    }
}