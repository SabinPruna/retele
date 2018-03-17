using TokenRing.Network;

namespace TokenRing {
    internal class Program {
        private static void Main() {
            GenerateNetworkRing(5);
        }

        private static void GenerateNetworkRing(int numberOfComputers) {
            Computer originalComputer = new Computer();
            Computer lastComputer = new Computer();

            for (int i = 0; i < numberOfComputers - 1; i++) {
                Computer computer = new Computer {NextIP = originalComputer.IP};
                lastComputer = computer;
            }

            originalComputer.NextIP = lastComputer.IP;
        }
    }
}