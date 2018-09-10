using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Utility.Audio {
    public class AudioManager<T> : MonoBehaviour {
        private Dictionary<T, AudioClip> _clips = new Dictionary<T, AudioClip>();
        private Dictionary<T, AudioParameter> _parameters = new Dictionary<T, AudioParameter>();
        private Dictionary<T, int> _channels = new Dictionary<T, int>();
        private Dictionary<Tuple<int, AudioParameter>, AudioSource> _sources = new Dictionary<Tuple<int, AudioParameter>, AudioSource>();

        private AudioSource getSource(T type) {
            var tuple = Tuple.Create(_channels[type], _parameters[type]);
            return _sources[tuple];
        }

        public bool Contains(T type) {
            return _clips.ContainsKey(type);
        }

        public void Set(T type, AudioEntry entry) {
            _clips.Add(type, entry.Clip);
            _parameters.Add(type, entry.Parameter);
            _channels.Add(type, entry.Channel);

            var tuple = Tuple.Create(entry.Channel, entry.Parameter);
            if (!_sources.ContainsKey(tuple)) {
                var obj = new GameObject(@"AudioSource (Channel: " + entry.Channel + ", AudioParameter: " + entry.Parameter.GetHashCode() + ")");
                var source = obj.AddComponent<AudioSource>();
                source.playOnAwake = false;
                entry.Parameter.SetTo(source);
                obj.transform.parent = transform;
                _sources.Add(tuple, source);
            }
        }

        public void Play(T type) {
            if (!Contains(type)) {
                Debug.LogError(@"[AudioManager.Play(" + typeof(T) + ")] 存在しない種類が指定されました");
                return;
            }

            var source = getSource(type);
            source.PlayOneShot(_clips[type]);
        }

        public void Stop(T type) {
            if (!Contains(type)) {
                Debug.LogError(@"[AudioManager.Stop(" + typeof(T) + ")] 存在しない種類が指定されました");
                return;
            }

            var source = getSource(type);
            source.Stop();
        }

        public void Stop(int channel) {
            if (!_channels.ContainsValue(channel)) { 
                Debug.LogError(@"[AudioManager.Stop(int)] 存在しないチャンネルが指定されました");
                return;
            }

            var pairs = _channels.Where(p => p.Value == channel);
            foreach (var pair in pairs) {
                var type = pair.Key;
                Stop(type);
            }
        }
    }
}
