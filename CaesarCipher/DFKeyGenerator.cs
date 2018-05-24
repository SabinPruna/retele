using System;

namespace CaesarCipher {
    public class DFKeyGenerator {
        private const int Q = 7;
        private const int A = 3;

        public static int GetX() {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(0, Q);
        }

        public static int GetY(int x) {
            return (int) Math.Pow(A, x) % Q;
        }

        public static int GetKey(int x, int y) {
            return (int) Math.Pow(y, x) % Q;
        }
    }
}