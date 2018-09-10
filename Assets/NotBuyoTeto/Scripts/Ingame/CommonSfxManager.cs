using System;
using UnityEngine;
using NotBuyoTeto.Utility.Audio;

namespace NotBuyoTeto.Ingame {
    [Serializable]
    class CommonSfxAudioEntryHolder : CommonSfxTypeHolder<AudioEntry> { }

    public class CommonSfxManager : AudioManager<CommonSfxType> {
        private static CommonSfxManager instance;

        public static CommonSfxManager Instance {
            get {
                if (instance) { return instance; }

                // Hierarchyから探す
                instance = FindObjectOfType<CommonSfxManager>();
                if (instance) { return instance; }

                // Resourcesから探す
                var resource = Resources.Load<CommonSfxManager>(@"CommonSfxManager");
                instance = Instantiate(resource);
                if (instance) { return instance; }

                return null;
            }
        }

        [SerializeField]
        private CommonSfxAudioEntryHolder entries;

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            Set(CommonSfxType.Decide, entries[CommonSfxType.Decide]);
            Set(CommonSfxType.Cancel, entries[CommonSfxType.Cancel]);
            Set(CommonSfxType.Select, entries[CommonSfxType.Select]);
        }
    }
}