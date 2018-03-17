using System;

namespace Retele {
    public class Program {
        public static void Main() {
            ParityChecker source = new ParityChecker();
            source.Encode();
            source.Corrupt();

            ParityChecker target = new ParityChecker(source.Array, source.ParityLine, source.ParityColumn);
            target.ScanForErrors();

            Console.ReadKey();
        }
    }
}