using UnityEngine;

namespace Split {

    /*
     * This class handles the smooth movement of the Camera
     */

    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour {
        [SerializeField] private Transform target;
        
        [Range(0.0f, 1.0f)]
        [SerializeField] private float smoothSpeed;

        [SerializeField] private Vector3 offset;

        private Vector3 velocity = Vector3.zero;

        //Makes camera follow target smoothly
        void LateUpdate() {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(this.transform.position, desiredPosition, ref this.velocity, this.smoothSpeed);

            this.transform.position = smoothedPosition;
        }



    }
}


