using UnityEngine;

namespace NotTetrin.Utility.Audio {
    public class AudioParameter {
        public static readonly AudioParameter Default = new AudioParameter();

        public float Volume = 1.0f;
        public float Pitch = 1.0f;

        public override int GetHashCode() => Volume.GetHashCode() ^ Pitch.GetHashCode();

        public override bool Equals(object obj) {
            var other = obj as AudioParameter;
            if (other == null) { return false; }
            return Volume == other.Volume && Pitch == other.Pitch;
        }

        public AudioSource Set(AudioSource source) {
            source.volume = Volume;
            source.pitch = Pitch;
            return source;
        }
    }
}
