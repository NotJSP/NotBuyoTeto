using UnityEngine;
using NotTetrin.Utility.Audio;

namespace NotTetrin.Ingame {
    public class CommonSfxManager : AudioManager<CommonSfxType> {
        [SerializeField] AudioClip decideClip;
        [SerializeField] AudioClip cancelClip;

        [SerializeField] AudioClip selectClip;

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

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            SetClip(CommonSfxType.Decide, decideClip, new AudioParameter { Volume = 1.0f });
            SetClip(CommonSfxType.Cancel, cancelClip, new AudioParameter { Volume = 1.0f });
            SetClip(CommonSfxType.Select, selectClip, new AudioParameter { Volume = 1.0f });
        }
    }
}