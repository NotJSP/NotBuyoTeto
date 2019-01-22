using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame {
    public class Field : MonoBehaviour {
        [SerializeField]
        private Ceiling ceiling;
        public Ceiling Ceiling => ceiling;
        [SerializeField]
        private GameObject leftSideWall;
        public GameObject LeftSideWall => leftSideWall;
        [SerializeField]
        private GameObject rightSideWall;
        public GameObject RightSideWall => rightSideWall;
        [SerializeField]
        private GameObject floor;
        public GameObject Floor => floor;
    }
}
