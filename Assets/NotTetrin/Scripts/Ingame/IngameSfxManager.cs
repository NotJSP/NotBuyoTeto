using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Utility.Audio;

namespace NotTetrin.Ingame {
    public class IngameSfxManager : AudioManager<IngameSfxType> {
        [Header(@"Game")]
        [SerializeField] private AudioClip gameStartClip;
        [SerializeField] private AudioClip gameOverClip;

        [Header(@"Mino")]
        [SerializeField] private AudioClip minoMoveClip;
        [SerializeField] private AudioClip minoTurnClip;
        [SerializeField] private AudioClip minoHitClip;
        [SerializeField] private AudioClip minoHoldClip;
        [SerializeField] private AudioClip minoDeleteClip;

        private void Awake() {
            SetClip(IngameSfxType.GameStart, gameStartClip, new AudioParameter{ Volume = 1.0f });
            SetClip(IngameSfxType.GameOver, gameOverClip, new AudioParameter{ Volume = 0.9f });
            SetClip(IngameSfxType.MinoMove, minoMoveClip, new AudioParameter{ Volume = 1.0f });
            SetClip(IngameSfxType.MinoTurn, minoTurnClip, new AudioParameter{ Volume = 0.8f });
            SetClip(IngameSfxType.MinoHit, minoHitClip, new AudioParameter{ Volume = 0.9f });
            SetClip(IngameSfxType.MinoHold, minoHoldClip, new AudioParameter{ Volume = 1.0f, Pitch = 1.2f });
            SetClip(IngameSfxType.MinoDelete, minoDeleteClip, new AudioParameter { Volume = 2.0f });
        }
    }
}