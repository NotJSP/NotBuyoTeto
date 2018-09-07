
namespace NotTetrin.Ingame.Marathon {
    public class DeleteMinoInfo {
        public int LineCount { get; private set; }
        public int ObjectCount { get; private set; }

        public DeleteMinoInfo(int lineCount, int objectCount) {
            this.LineCount = lineCount;
            this.ObjectCount = objectCount;
        }
    }
}
