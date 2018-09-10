using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility.Audio;

namespace NotBuyoTeto.Ingame {
    [Serializable]
    class IngameSfxAudioEntryHolder : IngameSfxTypeHolder<AudioEntry> { }

    public class IngameSfxManager : AudioManager<IngameSfxType> {
        [SerializeField]
        private IngameSfxAudioEntryHolder entries;

        private void Awake() {
            Set(IngameSfxType.RoundStart, entries[IngameSfxType.RoundStart]);
            Set(IngameSfxType.RoundEnd, entries[IngameSfxType.RoundEnd]);
        }
    }
}