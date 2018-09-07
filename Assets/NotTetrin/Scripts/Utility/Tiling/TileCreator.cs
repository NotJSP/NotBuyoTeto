using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Utility.Tiling {
    public class TileCreator : MonoBehaviour {
        [Header(@"Objects")]
        public Instantiator Instantiator;
        public GameObject Prefab;
        public GameObject Field;

        [Header(@"Properties")]
        [Range(1, 100)]
        public int DivideNumX = 10;
        [Range(1, 100)]
        public int DivideNumY = 10;

        public GameObject[] Create() {
            var rect = Field.rect();
            var size = Prefab.size();
            var helper = TileHelper.Calculate(rect, size, DivideNumX, DivideNumY);

            var objects = new GameObject[DivideNumX * DivideNumY];

            for (int j = 0; j < DivideNumY; j++) {
                var y = (helper.Unit.y / 2) + rect.yMin + helper.Unit.y * j;
                for (int i = 0; i < DivideNumX; i++) {
                    var x = (helper.Unit.x / 2) + rect.xMin + helper.Unit.x * i;
                    var position = new Vector3(x, y);
                    var obj = instantiate(Prefab, position, Quaternion.identity);
                    obj.transform.localScale = new Vector3(obj.transform.localScale.x * helper.Rate.x, obj.transform.localScale.y * helper.Rate.y);
                    obj.transform.SetParent(transform);
                    objects[i + j * DivideNumX] = obj;
                }
            }

            return objects;
        }

        private GameObject instantiate(GameObject original, Vector3 position, Quaternion rotation) {
            if (Instantiator != null) {
                return Instantiator.Instantiate(original, position, rotation);
            }
            return Instantiate(original, position, rotation);
        }
    }
}