using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotTetrin.Utility.Tiling;

namespace NotTetrin.Ingame.Marathon {
    [RequireComponent(typeof(TileCreator))]
    public class CollidersField : MonoBehaviour {
        public IEnumerable<CollidersGroup> Create(Instantiator instantiator, GameObject wall) {
            var objects = GetComponent<TileCreator>().Create();
            var groups = objects.Select(o => o.GetComponent<CollidersGroup>());
            foreach (var group in groups) {
                group.Initialize(instantiator, wall);
            }
            return groups;
        }
    }
}
