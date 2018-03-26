using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace TokenRing.Network
{
    public class Computer
    {
        #region Static Members

        private static int _messageCounter;

        #endregion

        #region Constructors

        public Computer(Random random)
        {
            IP = GetRandomIpAddress(random);
            Buffer = string.Empty;
        }

        #endregion

        #region Properties

        public string Buffer { get; set; }

        public string IP { get; set; }

        public string NextIP { get; set; }

        #endregion

        #region Methods - Public

        public bool CopyIfDestination(Token token)
        {
            lock (token) {
                if (token.DestinationIP == IP) {
                    token.IsDestinationReached = true;

                    Buffer += Buffer == string.Empty ? token.Message : ";" + token.Message;
                    token.IsAvailable = true;

                    return true;
                }

                return false;
            }
        }

        public void LoadToken(Token token, IList<string> computersList)
        {
            lock (token) {
                //Monitor.Wait(token);

                token.Message = $"This is message {_messageCounter++}";
                token.IsDestinationReached = false;
                token.IsAvailable = false;
                token.SourceIP = IP;
                token.DestinationIP = computersList.Where(i => i != IP).ToList().OrderBy(d => new Random(Guid.NewGuid().GetHashCode()).Next())
                    .First();

                Thread.Sleep(1000);
                //Monitor.PulseAll(token);s
            }
        }

        #endregion

        #region Methods - Private

        private static string GetRandomIpAddress(Random random)
        {
            return $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
        }

        #endregion
    }
}