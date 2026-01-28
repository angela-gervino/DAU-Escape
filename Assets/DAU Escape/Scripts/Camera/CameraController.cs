using Cinemachine;
using UnityEngine;

namespace DAUEscape
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineFreeLook freeLookCamera;

        public CinemachineFreeLook PlayerCam
        {
            get
            {
                return freeLookCamera;
            }
        }

        void Update()
        {

        }
    }
}

