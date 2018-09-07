using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.MultiPlay {
    public class WinsCounter : MonoBehaviour {
        [SerializeField] WinsStar[] stars;

        [HideInInspector]
        public int Count;
        public bool CountLimited => (Count >= stars.Length);

        public void Increment() {
            if (CountLimited) { return; }
            StartCoroutine(enableStar(stars[Count]));
            Count++;
        }

        private IEnumerator enableStar(WinsStar star) {
            yield return new WaitForSeconds(2.0f);
            star.Enable();
        }
    }
}