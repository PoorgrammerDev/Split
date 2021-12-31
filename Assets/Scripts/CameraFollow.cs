using UnityEngine;

namespace Split {

    /*
     * This class handles the smooth movement of the Camera
     */

    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour {

        public bool Active {get; set;}
        public Transform Target {get; set;}
        public Vector3 Offset {get; set;}
        
        [Range(0.0f, 1.0f)]
        [SerializeField] private float smoothSpeed;

        private Vector3 velocity = Vector3.zero;

        //Makes camera follow target smoothly
        void LateUpdate() {
            if (!Active) return;

            Vector3 desiredPosition = Target.position + Offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(this.transform.position, desiredPosition, ref this.velocity, this.smoothSpeed);

            this.transform.position = smoothedPosition;
        }

        public void TeleportToPosition() {
            this.transform.position = Target.position + Offset;
        }



    }
}


