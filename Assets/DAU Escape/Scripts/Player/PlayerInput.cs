using UnityEngine;

namespace DAUEscape
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 movement;

        public Vector3 moveInput
        {
            get
            {
                return movement;
            }
        }

        public bool IsMoving
        {
            get
            {
                return !Mathf.Approximately(moveInput.magnitude, 0);
            }
        }

        void Update()
        {
            movement.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );
        }
    }
}


