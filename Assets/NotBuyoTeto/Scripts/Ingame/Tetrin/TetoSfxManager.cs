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
            Set(TetoSfxType.MinoMove, entries[TetoSfxType.MinoMove]);
            Set(TetoSfxType.MinoRotate, entries[TetoSfxType.MinoRotate]);
            Set(TetoSfxType.MinoHold, entries[TetoSfxType.MinoHold]);
            Set(TetoSfxType.MinoHit, entries[TetoSfxType.MinoHit]);
            Set(TetoSfxType.MinoDelete, entries[TetoSfxType.MinoDelete]);
        }
    }
}
