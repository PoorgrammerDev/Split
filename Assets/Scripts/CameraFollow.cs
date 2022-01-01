using UnityEngine;

namespace Split {

    /*
     * This class handles the smooth movement of the Camera
     */

    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour {
        [SerializeField] private Transform target;
        [SerializeField] private bool active;
        [SerializeField] private Vector3 offset;

        
        
        
        [Range(0.0f, 1.0f)]
        [SerializeField] private float smoothSpeed;

        private Vector3 velocity = Vector3.zero;

        private void Awake() {
            if (target != null) active = true;
        }

        //Makes camera follow target smoothly
        void LateUpdate() {
            if (!active) return;

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(this.transform.position, desiredPosition, ref this.velocity, this.smoothSpeed);

            this.transform.position = smoothedPosition;
        }

        public void TeleportToPosition() {
            this.transform.position = target.position + offset;
        }

        public Transform Target {
            get {
                return target;
            }
            set {
                target = value;
            }
        }

        public Vector3 Offset {
            get {
                return offset;
            }
            set {
                offset = value;
            }
        }

        public bool Active {
            get {
                return active;
            }
            set {
                active = value;
            }
        }

    }
}


