using System;
using UnityEngine;

namespace NotBuyoTeto.Utility.Audio {
    [Serializable]
    public class AudioParameter {
        public static readonly AudioParameter Default = new AudioParameter();

        [Range(0f, 1f)]
        public float Volume = 1.0f;
        [Range(0f, 1f)]
        public float Pitch = 1.0f;

        public override int GetHashCode() => Volume.GetHashCode() ^ Pitch.GetHashCode();

        public AudioSource SetTo(AudioSource source) {
            source.volume = Volume;
            source.pitch = Pitch;
            return source;
        }

        public override bool Equals(object obj) {
            var other = obj as AudioParameter;
            if (other == null) { return false; }
            return Volume == other.Volume && Pitch == other.Pitch;
        }
    }
}
