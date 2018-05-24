using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarCipher
{
    public class Program
    {
        static void Main(string[] args)
        {
            Source source = new Source();
            Destination destination = new Destination();

            source.GenerateKey(destination.Y);
            destination.GenerateKey(source.Y);

            if (source.Key == destination.Key)
            {
                source.ReadMessage();
                string encripted = source.Caesar();
                Console.WriteLine(
                destination.DecryptCaesar(encripted));
            }




        }
    }
}
