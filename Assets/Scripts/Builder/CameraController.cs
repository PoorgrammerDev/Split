using System.Collections;
using UnityEngine;

namespace Split.Builder {
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CameraFollow))]
    public class CameraController : MonoBehaviour {
        private new Camera camera;
        private CameraFollow follow;

        private void Awake() {
            this.camera = GetComponent<Camera>();
            this.follow = GetComponent<CameraFollow>();
        }


        

    }
}