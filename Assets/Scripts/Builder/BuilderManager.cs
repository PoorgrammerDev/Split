using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Split.Builder.CameraStates;

namespace Split.Builder {
    public class BuilderManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BuilderLevelLoader levelLoader;

        private BuilderLevelData data;

        private void Awake() {
        }
        
        public void LevelLoaded(BuilderLevelData data) {
            this.data = data;

            cameraController.SetState(new IsoTilebound(cameraController, levelLoader));
        }

    }
}


