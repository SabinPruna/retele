using System;

namespace TokenRing.Network {
    public class Computer {
        public Computer() {
            IP = GetRandomIpAddress();
            Buffer = string.Empty;
        }

        public string IP { get; set; }

        public string NextIP { get; set; }

        public string Buffer { get; set; }

        private static string GetRandomIpAddress() {
            Random random = new Random();
            return $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
        }
    }
}