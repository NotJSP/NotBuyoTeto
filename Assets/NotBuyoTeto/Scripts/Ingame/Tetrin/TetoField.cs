using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class TetoField : MonoBehaviour {
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
        [SerializeField]
        private ColliderField colliderField;
        public ColliderField ColliderField => colliderField;
    }
}
