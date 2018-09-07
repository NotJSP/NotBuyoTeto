namespace NotTetrin.Utility {
    public class ASyncValue<T, E> {
        public static implicit operator T(ASyncValue<T, E> v) => v.value;

        private T value;
        private E exception;
        private bool proceeded = false;

        public T Value {
            get {
                return value;
            }
            set {
                this.value = value;
                proceeded = true;
            }
        }

        public E Exception {
            get {
                return exception;
            }
            set {
                this.exception = value;
                proceeded = true;
            }
        }

        public bool Take => proceeded;
        public bool Failure => exception != null;

        public bool TakeOrFailure() => Take;
        public bool TakeAndSucceeded => Take && !Failure;
        public bool TakeAndFailure => Take && Failure;

        public void Reset() {
            value = default(T);
            exception = default(E);
            proceeded = false;
        }

        public override string ToString() => value.ToString();
    }
}
