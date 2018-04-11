namespace SlidingWindow {
    public class Package {
        // ReSharper disable once InconsistentNaming
        public byte ACK { get; set; }
        // ReSharper disable once InconsistentNaming
        public byte SYN { get; set; }
        // ReSharper disable once InconsistentNaming
        public byte FIN { get; set; }
        public int X { get; set; }
        public int F { get; set; }
        public string Message { get; set; }

        public Package() {
            ACK = 0;
            SYN = 0;
            FIN = 0;
            X = 0;
            F = -1;
            Message = string.Empty;
        }

        public override string ToString() {
            return $"[ACK:{ACK}, SYN:{SYN}, FIN:{FIN}, x:{X}, f:{F}, Message:{Message}]";
        }
    }
}