
namespace NotBuyoTeto.Utility {
    public interface IHolder<T, E> {
        E this[T type] { get; }
        int Length { get; }
    }
}
