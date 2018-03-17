namespace TokenRing.Network {
    public class Token {
        public string Message { get; set; }

        public string SourceIP { get; set; }

        public string DestinationIP { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsDestinationReached { get; set; } = false;
    }
}