using System;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Utility;

namespace NotTetrin.Ingame {
    public class MinoManager : MonoBehaviour {
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private Director director;
        [SerializeField] private MinoSpawner spawner;
        [SerializeField] private IngameSfxManager sfxManager;
        [SerializeField] private Rigidbody2D minoRigidbody;

        private List<GameObject> minos = new List<GameObject>();
        private int currentIndex;
        private bool controlable = true;
        private float fallSpeed = 1.5f;
        private float defaultfallSpeed = 1.5f;

        public event EventHandler HitMino;

        private NextMino nextMino => director.NextMino;
        private HoldMino holdMino => director.HoldMino;

        public GameObject CurrentMino => minos.Count != 0 ? minos[minos.Count - 1] : null;

        private void Update() {
            if (!controlable) { return; }

            if (Input.GetButtonDown(@"Hold")) {
                hold();
            }
        }

        public void Reset() {
            fallSpeed = defaultfallSpeed;
            controlable = true;

            nextMino.Clear();
            holdMino.Clear();

            minos.ForEach(instantiator.Destroy);
            minos.Clear();
        }

        public void Next() {
            var index = nextMino.Pop();
            set(index);
            holdMino.Free();
        }

        private void hold() {
            var holdIndex = holdMino.Value;
            if (!holdMino.Push(currentIndex)) { return; }

            Destroy();

            var index = holdIndex.HasValue ? holdIndex.Value : nextMino.Pop();
            set(index);
        }

        public void Release() {
            controlable = false;
            var controller = CurrentMino.GetComponent<MinoController>();
            Destroy(controller);
        }

        public void Destroy() {
            controlable = false;
            instantiator.Destroy(CurrentMino);
            minos.RemoveAt(minos.Count - 1);
        }

        private void set(int index) {
            currentIndex = index;
            controlable = true;

            var obj = spawner.Spawn(index);
            obj.AddComponent<Rigidbody2D>().CopyOf(minoRigidbody);
            var controller = obj.AddComponent<MinoController>().Initialize(sfxManager, fallSpeed);
            controller.Hit += onHitMino;
            minos.Add(obj);

            Debug.Log(fallSpeed);
        }

        private void onHitMino(object sender, EventArgs args) {
            HitMino?.Invoke(sender, args);
        }

        public void fallSpeedUp(int level) {
            fallSpeed = fallSpeed + (0.01f * level);
        }
    }
}
