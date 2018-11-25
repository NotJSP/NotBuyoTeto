
namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class DeleteBuyoInfo {
        public int LineCount { get; private set; }
        public int ObjectCount { get; private set; }

        public DeleteBuyoInfo(int lineCount, int objectCount) {
            this.LineCount = lineCount;
            this.ObjectCount = objectCount;
        }
    }
}
