using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarCipher
{
    public class Destination
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Key { get; set; }

        public Destination()
        {
            X = DFKeyGenerator.GetX();
            Y = DFKeyGenerator.GetY(X);
        }

        public void GenerateKey(int y)
        {
            Key = DFKeyGenerator.GetKey(X, y);
            Console.WriteLine($"Destination: X:{X} Y:{Y} KEY:{Key}");
        }


        public string DecryptCaesar(string message)
        {
            char[] buffer = message.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                letter = (char)(letter + (-Key));
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
