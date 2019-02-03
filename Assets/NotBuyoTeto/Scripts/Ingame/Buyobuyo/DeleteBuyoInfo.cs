
namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class DeleteBuyoInfo {
        public int ObjectCount { get; private set; }
        public int ComboCount { get; private set; }

        public DeleteBuyoInfo(int objectCount, int comboCount) {
            this.ObjectCount = objectCount;
            this.ComboCount = comboCount;
        }
    }
}
