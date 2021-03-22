namespace Nyeoglike.Unique.WorldGen.Tools.Records {
    public class VetoBox {
        public bool Vetoed { get; internal set; }

        public VetoBox() {
            Vetoed = false;
        }
    }
}
