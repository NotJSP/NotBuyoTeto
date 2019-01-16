using System;
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

        public static IEnumerator animate(Animator animator, string state) {
            IsAnimating = true;

            animator.Play(state);
            yield return new WaitForEndOfFrame();
            yield return new WaitWhile(() => isPlaying(animator));

            IsAnimating = false;
        }

        public static IEnumerator In(AnimationTransitEntry e, Action afterAction = null) {
            e.Object.SetActive(true);
            yield return animate(e.Animator, e.InState);
            afterAction?.Invoke();
        }

        public static IEnumerator Out(AnimationTransitEntry e, Action afterAction = null) {
            yield return animate(e.Animator, e.OutState);
            e.Object.SetActive(false);
            afterAction?.Invoke();
        }

        public static IEnumerator Transition(AnimationTransitEntry from, AnimationTransitEntry to, Action afterAction = null) {
            if (from != null) {
                yield return Out(from);
            }
            if (to != null) {
                yield return In(to);
            }
            afterAction?.Invoke();
        }

        private static bool isPlaying(Animator animator) {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        }
    }
}
