using UnityEditor;
using UnityEngine;
using Split.Map;

namespace Split.EditorScripts {

    [CustomEditor(typeof(MapGenerator))]
    public class MapGenEditor : Editor {
        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            MapGenerator generator = this.target as MapGenerator;
            if (GUILayout.Button("Generate")) {
                generator.GenerateMap();
            }
            else if (GUILayout.Button("Clear")) {
                generator.ClearMap();
            }
        }
    }
}

