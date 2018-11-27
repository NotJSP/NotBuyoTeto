using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    [Serializable]
    public class MinoControlSettings {
        [Header("Control")]
        public float HorizontalVelocity = 6.7f;
        public float AngularVelocity = 120.0f;
        public AnimationCurve AccelarationCurve = new AnimationCurve(
            new Keyframe(0, 0, 1.617043f, 1.617043f, 0, 0.1401876f),
            new Keyframe(1, 1, 0.7356746f, 0.7356746f, 0.3873379f, 0)
        );
        public AnimationCurve DecelerationCurve = new AnimationCurve(
            new Keyframe(0, 0, 1.617043f, 1.617043f, 0, 0.1401876f),
            new Keyframe(1, 1, 0.7356746f, 0.7356746f, 0.3873379f, 0)
        );

        [Header("Limitation")]
        public float LimitHorizontalVelocity = 4.2f;
        public float LimitAngularVelocity = 180.0f;
        public float SoftdropPeekAccelaration = 6.58f;
        public float HarddropPeekAccelaration = 9.87f;
        public float AccelarationTime = 1.0f;
        public float DecelerationTime = 0.5f;
    }
}
