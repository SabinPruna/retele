using System;
using System.Collections.Generic;
using System.Linq;

namespace TokenRing.Network {
    public class Computer {
        private static int _messageCounter;

        public Computer(Random random) {
            IP = GetRandomIpAddress(random);
            Buffer = string.Empty;
        }

        public string IP { get; set; }

        public string NextIP { get; set; }

        public string Buffer { get; set; }

        private static string GetRandomIpAddress(Random random) {
            return $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
        }

        public bool CopyIfDestination(Token token) {
            if (token.DestinationIP == IP) {
                token.IsDestinationReached = true;
                Buffer += ";" + token.Message;
                token.IsAvailable = true;

                return true;
            }

            return false;
        }

        public void LoadToken(Token token, IList<string> computersList) {
            token.Message = $"This is message {_messageCounter++}";
            token.IsDestinationReached = false;
            token.IsAvailable = false;
            token.SourceIP = IP;
            token.DestinationIP = computersList.Where(i => i != IP).ToList().OrderBy(d => new Random().Next())
                                               .FirstOrDefault();
        }
    }
}