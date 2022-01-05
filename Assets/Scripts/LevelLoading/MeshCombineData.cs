using System.Collections.Generic;
using UnityEngine;

namespace Split.LevelLoading {
    /// <summary>
    /// Holds combined tile mesh data for one large mesh. Enforces the max vertex limit.
    /// </summary>
    public class MeshCombineData {
        private const int MAX_MESH_VERTICIES = 65535;

        private List<CombineInstance> combineInstances;
        private int vertexCount;

        public MeshCombineData() {
            combineInstances = new List<CombineInstance>();
            vertexCount = 0;
        }

        public bool Add(CombineInstance combine) {
            if (HasSpace(combine)) {
                this.combineInstances.Add(combine);
                this.vertexCount += combine.mesh.vertexCount;
                return true;
            }

            return false;
        }

        public bool HasSpace(CombineInstance combine) {
            return (this.vertexCount + combine.mesh.vertexCount < MAX_MESH_VERTICIES);
        }

        public CombineInstance[] ToArray() {
            return combineInstances.ToArray();
        }

        public bool IsEmpty() {
            return combineInstances.Count == 0;
        }
    }
}