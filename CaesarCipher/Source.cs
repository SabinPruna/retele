using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarCipher
{
    public class Source
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Key { get; set; }
        public string Message { get; set; }

        public Source() {
            X = DFKeyGenerator.GetX();
            Y = DFKeyGenerator.GetY(X);
        }

        public void GenerateKey(int y)
        {
            Key = DFKeyGenerator.GetKey(X, y);
            Console.WriteLine($"Source: X:{X} Y:{Y} KEY:{Key}");
        }

        public void ReadMessage()
        {
            Console.WriteLine("Enter message to encript: ");
            Message = Console.ReadLine();
        }


        public string Caesar() {
            char[] buffer = Message.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                letter = (char)(letter + Key);
                if (letter > 'z')
                {
                    letter = (char)(letter - 26);
                }
                else if (letter < 'a')
                {
                    letter = (char)(letter + 26);
                }
                buffer[i] = letter;
            }
            return new string(buffer);
        }
    }
}

