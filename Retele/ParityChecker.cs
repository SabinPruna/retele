using System;
using System.Linq;
using System.Text;

namespace Retele {
    public class ParityChecker {
        public ParityChecker() {
            Console.WriteLine("Word:");
            Message = Console.ReadLine();
            if (Message != null) {
                Array = new int[Message.Length, 7];
                ParityLine = new int[7];
                ParityColumn = new int[Message.Length];
            }
        }

        public ParityChecker(int[,] array, int[] parityLine, int[] parityColumn) {
            Array = array;
            ParityLine = parityLine;
            ParityColumn = parityColumn;
        }

        public int[,] Array { get; set; }
        public int[] ParityLine { get; set; }
        public int[] ParityColumn { get; set; }
        public int ExtraByte { get; set; }
        public string Message { get; set; }

        public void Encode() {
            for (int i = 0; i < Message.Length; i++) {
                int ascii = Message[i];
                string binary = Convert.ToString(ascii, 2);
                int j = 7 - binary.Length;
                for (int k = 0; k < j; k++) {
                    Array[i, k] = 0;
                }

                foreach (char c in binary) {
                    Array[i, j] = int.Parse(c.ToString());
                    j++;
                }
            }

            CalculateLine();
            CalculateColumn();
            CalculateExtraByte();
            ShowArray();
        }

        public void ScanForErrors() {
            int[] parityCol = new int[ParityColumn.Length];
            int[] parityLine = new int[7];
            for (int i = 0; i < ParityColumn.Length; i++) {
                parityCol[i] = ParityColumn[i];
            }

            for (int i = 0; i < 7; i++) {
                parityLine[i] = ParityLine[i];
            }

            int x = -1, y = -1;
            bool hasError = false;
            CalculateColumn();
            for (int i = 0; i < parityCol.Length; i++) {
                if (ParityColumn[i] != parityCol[i]) {
                    y = i;
                    hasError = true;
                }
            }

            CalculateLine();
            for (int i = 0; i < 7; i++) {
                if (ParityLine[i] != parityLine[i]) {
                    x = i;
                    hasError = true;
                }
            }

            if (!hasError) {
                Console.WriteLine("Message was not corrupted.");
            }
            else {
                Console.WriteLine($"Message was corrupted at byte [{y},{x}].");
                FixMessage(y, x);
                ParityColumn = parityCol;
                ParityLine = parityLine;
                ShowArray();
            }

            Decode();
        }

        public void Corrupt() {
            Random rand = new Random();
            int randx = rand.Next(1, Message.Length);
            int randy = rand.Next(1, 7);
            FixMessage(randx, randy);
            Console.WriteLine($"Corrupted byte at position: {randx},{randy}.");
        }

        private void Decode() {
            StringBuilder message = new StringBuilder();
            for (int i = 0; i < ParityColumn.Length; i++) {
                int res = 0;
                int k = 0;
                for (int j = 6; j >= 0; j--) {
                    res += Array[i, j] * int.Parse(Math.Pow(2, k).ToString());
                    k++;
                }

                char c = (char) res;
                message.Append(c.ToString());
            }

            Console.WriteLine("Received:");
            Console.WriteLine(message);
        }

        private void FixMessage(int x, int y) {
            Array[x, y] = 1 - Array[x, y];
        }

        private void CalculateExtraByte() {
            int resCol = ParityColumn.Sum();

            int resLine = ParityLine.Sum();

            if (resCol % 2 == resLine % 2) {
                ExtraByte = resCol % 2;
            }
        }

        private void CalculateColumn() {
            for (int i = 0; i < ParityColumn.Length; i++) {
                int res = 0;
                for (int j = 0; j < 7; j++) {
                    res += Array[i, j];
                }

                ParityColumn[i] = res % 2;
            }
        }

        private void CalculateLine() {
            for (int i = 0; i < 7; i++) {
                int res = 0;
                for (int j = 0; j < ParityColumn.Length; j++) {
                    res += Array[j, i];
                }

                ParityLine[i] = res % 2;
            }
        }

        public void ShowArray() {
            for (int i = 0; i < ParityColumn.Length; i++) {
                for (int j = 0; j < 7; j++) {
                    Console.Write(Array[i, j]);
                }

                Console.Write(" ");
                Console.WriteLine(ParityColumn[i]);
            }

            Console.WriteLine();
            for (int j = 0; j < 7; j++) {
                Console.Write(ParityLine[j]);
            }

            Console.Write(" ");
            Console.WriteLine(ExtraByte);
            Console.WriteLine();
        }
    }
}