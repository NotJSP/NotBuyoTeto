using System;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class MinoManager : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private MinoSpawner spawner;
        [SerializeField]
        private TetoSfxManager sfxManager;
        [SerializeField]
        private Rigidbody2D minoRigidbody;
        [SerializeField]
        private MinoControlSettings controlSettings;

        private List<GameObject> minos = new List<GameObject>();
        private MinoType currentType;
        private bool controlable = true;

        private float fallSpeed;

        public event EventHandler HitMino;

        private NextMino nextMino => perspective.NextMino;
        private HoldMino holdMino => perspective.HoldMino;

        public GameObject CurrentMino => minos.Count != 0 ? minos[minos.Count - 1] : null;

        private void Update() {
            if (!controlable) { return; }

            if (Input.GetButtonDown(@"Hold")) {
                hold();
            }
        }

        public void Initialize(float fallSpeed) {
            controlable = true;

            nextMino.Clear();
            holdMino.Clear();

            minos.ForEach(instantiator.Destroy);
            minos.Clear();

            SetFallSpeed(fallSpeed);
        }

        public void Next() {
            var type = nextMino.Pop();
            set(type);
            holdMino.Free();
        }

        private void hold() {
            var holdIndex = holdMino.Type;
            if (!holdMino.Push(currentType)) { return; }

            Destroy();

            var type = holdIndex.HasValue ? holdIndex.Value : nextMino.Pop();
            set(type);
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

        private void set(MinoType type) {
            currentType = type;
            controlable = true;

            var position = perspective.Field.Ceiling.transform.position;
            var obj = spawner.Spawn(type, position);
            obj.AddComponent<Rigidbody2D>().CopyOf(minoRigidbody);
            var controller = obj.AddComponent<MinoController>().Initialize(sfxManager, controlSettings, fallSpeed);
            controller.Hit += onHitMino;

            minos.Add(obj);
        }

        private void onHitMino(object sender, EventArgs args) {
            HitMino?.Invoke(sender, args);
        }

        public void SetFallSpeed(float speed) {
            fallSpeed = speed;
        }
    }
}
