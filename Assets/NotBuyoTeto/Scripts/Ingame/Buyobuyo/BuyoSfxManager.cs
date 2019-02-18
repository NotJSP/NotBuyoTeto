using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NotBuyoTeto.Utility.Audio;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [Serializable]
    class BuyoSfxAudioEntryHolder : BuyoSfxTypeHolder<AudioEntry> { }

    public class BuyoSfxManager : AudioManager<BuyoSfxType> {
        [SerializeField]
        private BuyoSfxAudioEntryHolder entries;

        private void Awake() {
            Set(BuyoSfxType.Move, entries[BuyoSfxType.Move]);
            Set(BuyoSfxType.Rotate, entries[BuyoSfxType.Rotate]);
            Set(BuyoSfxType.Hit, entries[BuyoSfxType.Hit]);
            Set(BuyoSfxType.Delete, entries[BuyoSfxType.Delete]);
            Set(BuyoSfxType.Hold, entries[BuyoSfxType.Hold]);
        }
    }
}
