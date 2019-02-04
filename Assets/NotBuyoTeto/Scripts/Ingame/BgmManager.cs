using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame {
    [RequireComponent(typeof(AudioSource))]
    public class BgmManager : MonoBehaviour {
        public AudioClip[] Clips;

        private AudioSource audioSource;
        private List<AudioClip> shuffleClips;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
            shuffleClips = new List<AudioClip>(Clips);
        }

        public void Add(AudioClip clip) {
            if (shuffleClips.Contains(clip)) { return; }
            shuffleClips.Add(clip);
        }

        public void Remove(AudioClip clip) {
            if (!shuffleClips.Contains(clip)) { return; }
            shuffleClips.Remove(clip);
        }

        public void RandomPlay() {
            // クリップがない場合、無音
            if (shuffleClips.Count == 0) {
                audioSource.Stop();
                return;
            }

            var clip = shuffleClips.Shuffle().ElementAt(0);
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void Stop() {
            audioSource.Stop();
        }
    }
}
