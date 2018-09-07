using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.Marathon {
    public class GroupManager : MonoBehaviour {
        [SerializeField] MarathonDirector director;
        [SerializeField] Instantiator instantiator;
        [SerializeField] IngameSfxManager sfxManager;

        private IEnumerable<CollidersGroup> groups;
        public event EventHandler<DeleteMinoInfo> MinoDeleted;

        private void Start() {
            groups = director.CollidersField.Create(instantiator, director.SideWall);
        }

        public void DeleteMino() {
            var eraseGroups = groups.Where(g => g.IsEntered);
            var lineCount = eraseGroups.Count();
            var objectCount = eraseGroups.Sum(g => g.EnteredObjectCount);
            if (lineCount == 0) { return; }

            foreach (var group in eraseGroups) {
                group.DeleteMino();
            }

            var info = new DeleteMinoInfo(lineCount, objectCount);
            MinoDeleted?.Invoke(this, info);
            sfxManager.Play(IngameSfxType.MinoDelete);
        }
    }
}