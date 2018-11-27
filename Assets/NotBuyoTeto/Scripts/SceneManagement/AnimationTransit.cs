using System.Collections;
using UnityEngine;

namespace NotBuyoTeto.SceneManagement {
    public class AnimationTransitEntry {
        public readonly GameObject Object;
        public readonly Animator Animator;
        public readonly string InState;
        public readonly string OutState;

        public AnimationTransitEntry(GameObject obj, string inState, string outState) {
            this.Object = obj;
            this.Animator = obj.GetComponent<Animator>();
            this.InState = inState;
            this.OutState = outState;
        }
    }

    public static class AnimationTransit {
        public static bool IsAnimating { get; private set; }

        public static IEnumerator In(AnimationTransitEntry e, bool changeActivity = true) {
            IsAnimating = true;

            e.Object.SetActive(true);

            e.Animator.Play(e.InState);
            yield return new WaitForEndOfFrame();
            yield return new WaitWhile(() => isPlaying(e.Animator));

            IsAnimating = false;
        }

        public static IEnumerator Out(AnimationTransitEntry e, bool changeActivity = true) {
            IsAnimating = true;

            e.Animator.Play(e.OutState);
            yield return new WaitForEndOfFrame();
            yield return new WaitWhile(() => isPlaying(e.Animator));

            e.Object.SetActive(false);

            IsAnimating = false;
        }

        public static IEnumerator Transition(AnimationTransitEntry from, AnimationTransitEntry to) {
            var changeActivity = !from.Object.Equals(to);

            if (from != null) {
                yield return Out(from, changeActivity);
            }
            if (to != null) {
                yield return In(to, changeActivity);
            }
        }

        private static bool isPlaying(Animator animator) {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        }
    }
}
