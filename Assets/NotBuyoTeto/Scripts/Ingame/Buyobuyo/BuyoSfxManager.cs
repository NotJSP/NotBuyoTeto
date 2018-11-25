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
            Set(BuyoSfxType.BuyoMove, entries[BuyoSfxType.BuyoMove]);
            Set(BuyoSfxType.BuyoRotate, entries[BuyoSfxType.BuyoRotate]);
            Set(BuyoSfxType.BuyoHold, entries[BuyoSfxType.BuyoHold]);
            Set(BuyoSfxType.BuyoHit, entries[BuyoSfxType.BuyoHit]);
            Set(BuyoSfxType.BuyoDelete, entries[BuyoSfxType.BuyoDelete]);
        }
    }
}
