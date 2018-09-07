using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotTetrin.Utility.Audio {
    public class AudioManager<T> : MonoBehaviour {
        private Dictionary<T, AudioClip> _clips = new Dictionary<T, AudioClip>();
        private Dictionary<T, AudioParameter> _parameters = new Dictionary<T, AudioParameter>();
        private Dictionary<T, int> _channels = new Dictionary<T, int>();
        private Dictionary<Tuple<int, AudioParameter>, AudioSource> _sources = new Dictionary<Tuple<int, AudioParameter>, AudioSource>();

        private AudioSource getSource(T type) {
            var tuple = Tuple.Create(_channels[type], _parameters[type]);
            return _sources[tuple];
        }

        private bool contains(T type) {
            return _clips.ContainsKey(type);
        }

        public void SetClip(T type, AudioClip clip, int channel = 0) => SetClip(type, clip, AudioParameter.Default, channel);

        public void SetClip(T type, AudioClip clip, AudioParameter parameter, int channel = 0) {
            _clips.Add(type, clip);
            _parameters.Add(type, parameter);
            _channels.Add(type, channel);

            var tuple = Tuple.Create(channel, parameter);
            if (!_sources.ContainsKey(tuple)) {
                var obj = new GameObject(@"AudioSource (Channel: " + channel + ", AudioParameter: " + parameter.GetHashCode() + ")");
                var source = obj.AddComponent<AudioSource>();
                source.playOnAwake = false;
                parameter.Set(source);
                obj.transform.parent = transform;
                _sources.Add(tuple, source);
            }
        }

        public void Play(T type) {
            if (!contains(type)) {
                Debug.LogError(@"[AudioManager.Play(" + typeof(T) + ")] 存在しない種類が指定されました");
                return;
            }

            var source = getSource(type);
            source.PlayOneShot(_clips[type]);
        }

        public void Stop(T type) {
            if (!contains(type)) {
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

            var type = _channels.First(p => p.Value == channel).Key;
            Stop(type);
        }
    }
}
