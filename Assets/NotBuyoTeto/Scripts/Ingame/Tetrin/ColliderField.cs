using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility.Tiling;

namespace NotBuyoTeto.Ingame.Tetrin {
    [RequireComponent(typeof(TileCreator))]
    public class ColliderField : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private TetoSfxManager sfxManager;

        private IEnumerable<ColliderGroup> groups;
        public event EventHandler<DeleteMinoInfo> LineDeleted;

        private void Start() {
            groups = Create();
        }

        public ColliderGroup[] Create() {
            var objects = GetComponent<TileCreator>().Create();
            var groups = objects.Select(o => o.GetComponent<ColliderGroup>());
            foreach (var group in groups) {
                group.Initialize(instantiator, perspective.Field.LeftSideWall);
            }
            return groups.ToArray();
        }

        public void DeleteLine() {
            var eraseGroups = groups.Where(g => g.EnteredAll);
            var lineCount = eraseGroups.Count();
            var objectCount = eraseGroups.Sum(g => g.EnterCount);
            if (lineCount == 0) { return; }

            foreach (var group in eraseGroups) {
                group.DeleteLine();
            }

            var info = new DeleteMinoInfo(lineCount, objectCount);
            LineDeleted?.Invoke(this, info);

            sfxManager.Play(TetoSfxType.MinoDelete);
        }
    }
}