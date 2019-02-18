using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NotBuyoTeto.Utility.Audio;

namespace NotBuyoTeto.Ingame.Tetrin {
    [Serializable]
    class TetoSfxAudioEntryHolder : TetoSfxTypeHolder<AudioEntry> { }

    public class TetoSfxManager : AudioManager<TetoSfxType> {
        [SerializeField]
        private TetoSfxAudioEntryHolder entries;

        private void Awake() {
            Set(TetoSfxType.Move, entries[TetoSfxType.Move]);
            Set(TetoSfxType.Rotate, entries[TetoSfxType.Rotate]);
            Set(TetoSfxType.Hold, entries[TetoSfxType.Hold]);
            Set(TetoSfxType.Hit, entries[TetoSfxType.Hit]);
            Set(TetoSfxType.Delete, entries[TetoSfxType.Delete]);
        }
    }
}
