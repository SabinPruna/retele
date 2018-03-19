using System;
using System.Collections.Generic;
using System.Linq;
using TokenRing.Network;

namespace TokenRing {
    public class Program {
        private static int _numberOfLoads = 4;

        public static void Main() {
            List<Computer> computerList = GenerateNetworkRing(10);
            Token token = new Token();

            while (_numberOfLoads != 0) {
                //while (!token.IsAvailable) {
                //    //get Co
                //}

                PrintBuffers(computerList);

                Computer sourceComputer = computerList.OrderBy(d => new Random(DateTime.Now.Millisecond).Next()).First();
                sourceComputer.LoadToken(token, computerList.Select(c => c.IP).ToList());
                Console.WriteLine($"Sursa: {token.SourceIP} Destinatie: {token.DestinationIP}");

                Computer computer = computerList.Find(x => x.IP == sourceComputer.NextIP);

                while (!token.IsDestinationReached) {
                    Console.WriteLine(computer.CopyIfDestination(token)
                                          ? $"{computer.IP}: Am ajuns la destiantie"
                                          : $"{computer.IP}: Muta jetonul");

                    computer = computerList.Find(x => x.IP == computer.NextIP);
                }

                MoveTokenBackToSource(token, computerList);
                PrintBuffers(computerList);
                _numberOfLoads--;
            }
        }

        private static void MoveTokenBackToSource(Token token, List<Computer> computerList) {
            Computer computer = computerList.Find(x => x.IP == token.DestinationIP);

            while (computer.NextIP != token.SourceIP) {
                Console.WriteLine($"{computer.IP}: Muta jetonul");
                computer = computerList.Find(x => x.IP == computer.NextIP);
            }

            Console.WriteLine($"{computer.NextIP}: Am ajuns inapoi");
        }

        private static List<Computer> GenerateNetworkRing(int numberOfComputers) {
            Random random = new Random();

            List<Computer> list = new List<Computer>();

            Computer originalComputer = new Computer(random);
            list.Add(originalComputer);
            Computer computer = new Computer(random) {NextIP = originalComputer.IP};
            list.Add(computer);
            Computer lastComputer = new Computer(random);

            for (int i = 0; i < numberOfComputers -2; i++) {
                string lastIp = computer.IP;
                computer = new Computer(random) {NextIP = lastIp};
                lastComputer = computer;
                list.Add(computer);
            }

            originalComputer.NextIP = lastComputer.IP;

            return list;
        }

        public static void PrintBuffers(IList<Computer> computers) {
            foreach (Computer computer in computers) {
                Console.WriteLine($"{computer.IP} -> {computer.Buffer}");
            }
        }
    }
}