using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Utility.Audio {
    [Serializable]
    public class AudioEntry {
        [SerializeField]
        private int channel = 0;
        public int Channel => channel;

        [SerializeField]
        private AudioClip clip;
        public AudioClip Clip => clip;

        [SerializeField]
        private AudioParameter parameter;
        public AudioParameter Parameter => parameter;
    }
}
