using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TransportationServer {
    internal class Program {
        private static void Main(string[] args) {
            const string key = "pangram";

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 1234);
            TcpListener listener = new TcpListener(endPoint);

            Console.WriteLine($"[listening], IP= {IPAddress.Any}");
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client Connected");

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string encriptedText = Encoding.Unicode.GetString(buffer, 0, data);


            Console.WriteLine("Recieved Message " + Decipher(encriptedText, key));
            client.Close();
            Console.ReadKey();
        }

        private static int[] GetShiftIndexes(string key) {
            int[] indexes = new int[key.Length];
            List<KeyValuePair<int, char>> sortedKey = new List<KeyValuePair<int, char>>();

            for (int i = 0; i < key.Length; ++i) {
                sortedKey.Add(new KeyValuePair<int, char>(i, key[i]));
            }

            sortedKey.Sort(
                (pair1, pair2) => pair1.Value.CompareTo(pair2.Value)
            );

            for (int i = 0; i < key.Length; ++i) {
                indexes[sortedKey[i].Key] = i;
            }

            return indexes;
        }

        public static string Encipher(string input, string key) {
            input = input.Length % key.Length == 0
                        ? input
                        : input + "abcdefghijklmnopqrstuvwxyz".Substring(
                              0, input.Length % key.Length); //PadRight(input.Length - input.Length % key.Length + key.Length, padChar);

            StringBuilder output = new StringBuilder();
            int totalRows = (int) Math.Ceiling((double) input.Length / key.Length);

            char[,] rowChars = new char[totalRows, key.Length];
            char[,] colChars = new char[key.Length, totalRows];
            char[,] sortedColChars = new char[key.Length, totalRows];
            int currentRow, currentColumn;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (int i = 0; i < input.Length; ++i) {
                currentRow = i / key.Length;
                currentColumn = i % key.Length;
                rowChars[currentRow, currentColumn] = input[i];
            }

            for (int i = 0; i < totalRows; ++i)
            for (int j = 0; j < key.Length; ++j) {
                colChars[j, i] = rowChars[i, j];
            }

            for (int i = 0; i < key.Length; ++i)
            for (int j = 0; j < totalRows; ++j) {
                sortedColChars[shiftIndexes[i], j] = colChars[i, j];
            }

            for (int i = 0; i < input.Length; ++i) {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                output.Append(sortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }

        public static string Decipher(string input, string key) {
            StringBuilder output = new StringBuilder();
            int totalColumns = (int) Math.Ceiling((double) input.Length / key.Length);
            char[,] rowChars = new char[key.Length, totalColumns];
            char[,] colChars = new char[totalColumns, key.Length];
            char[,] unsortedColChars = new char[totalColumns, key.Length];
            int currentRow, currentColumn;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (int i = 0; i < input.Length; ++i) {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = input[i];
            }

            for (int i = 0; i < key.Length; ++i)
            for (int j = 0; j < totalColumns; ++j) {
                colChars[j, i] = rowChars[i, j];
            }

            for (int i = 0; i < totalColumns; ++i)
            for (int j = 0; j < key.Length; ++j) {
                unsortedColChars[i, j] = colChars[i, shiftIndexes[j]];
            }

            for (int i = 0; i < input.Length; ++i) {
                currentRow = i / key.Length;
                currentColumn = i % key.Length;
                output.Append(unsortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }
    }
}