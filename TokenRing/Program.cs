using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TokenRing.Network;

namespace TokenRing {
    public class Program {
        private static int _numberOfLoads = 4;


        //Testing other pc sync



        public static void Main() {
            Token token = new Token();
            List<Computer> computerList = GenerateNetworkRing(10);
            List<Thread> computerThreads = GenerateComputerThreads(computerList, token);
            //Random randomDistributor = new Random(Guid.NewGuid().GetHashCode());

            Parallel.ForEach(computerThreads, ct => ct.Start());

            while (_numberOfLoads != 0) {
                //while (token.IsAvailable) {}
                //ThreadPool.QueueUserWorkItem(computerList.First().LoadToken(token, computerList.Select(c=>c.IP).ToList()), token);

                PrintComputerBuffers(computerList);

                //lock (token) {
                //    Monitor.Pulse(token);
                //    Monitor.Wait(token);
                //}

                if (token.IsAvailable) {
                    ComputerThreadsHandler(computerList, token);
                }

                Computer sourceComputer = computerList.Find(c => c.IP == token.SourceIP);
                //.OrderBy(d => randomDistributor.Next()).First();
                //sourceComputer.LoadToken(token, computerList.Select(c => c.IP).ToList());

                Console.WriteLine($"Sursa: {token.SourceIP} Destinatie: {token.DestinationIP}");
                Computer computer = computerList.Find(x => x.IP == sourceComputer.NextIP);

                lock (token) {
                    while (!token.IsDestinationReached) {
                        Console.WriteLine(computer.CopyIfDestination(token)
                                              ? $"{computer.IP}: Am ajuns la destinatie"
                                              : $"{computer.IP}: Muta jetonul");

                        computer = computerList.Find(x => x.IP == computer.NextIP);
                    }

                    MoveTokenBackToSource(token, computerList);
                }

                _numberOfLoads--;
            }

            PrintComputerBuffers(computerList);
        }

        private static void ComputerThreadsHandler(List<Computer> computerList, Token token) {
            Token temporaryToken = token;
            Parallel.ForEach(computerList,
                             computer => {
                                 new Thread(() => {
                                                computer.LoadToken(temporaryToken,
                                                                   computerList.Select(c => c.IP).ToList());
                                            }).Start();
                             });
        }

        private static List<Thread> GenerateComputerThreads(List<Computer> computerList, Token token) {
            List<Thread> computerThreads = new List<Thread>();

            foreach (Computer computer in computerList) {
                Token temporaryToken = token;
                Thread computerThread = new Thread(() => {
                                                       computer.LoadToken(
                                                           temporaryToken, computerList.Select(c => c.IP).ToList()
                                                       );
                                                   });
                computerThreads.Add(computerThread);
            }

            return computerThreads;
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

            for (int i = 0; i < numberOfComputers - 2; i++) {
                string lastIp = computer.IP;
                computer = new Computer(random) {NextIP = lastIp};
                lastComputer = computer;
                list.Add(computer);
            }

            originalComputer.NextIP = lastComputer.IP;

            return list;
        }

        public static void PrintComputerBuffers(IList<Computer> computers) {
            foreach (Computer computer in computers) {
                Console.WriteLine($"{computer.IP} -> {computer.Buffer}");
            }
        }
    }
}