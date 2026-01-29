
using UnityEngine;

namespace DAUEscape
{
    public class PlayerController : MonoBehaviour
    {
        // let enemy classes get access to player class through static variables
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }

        public float maxForwardSpeed = 8.0f;
        public float rotationSpeed;
        public int maxRotationSpeed = 1200;
        public int minRotationSpeed = 800;

        // s_ denotes static variables
        private static PlayerController s_Instance;
        private PlayerInput playerInput;
        private CharacterController chController;
        private Animator animator;
        private CameraController cameraController;

        private Quaternion qTargetRotation;

        private float desiredForwardSpeed;
        private float forwardSpeed;

        private readonly int hashForwardSpeed = Animator.StringToHash("ForwardSpeed");

        const float acceleration = 20.0f;
        const float deceleration = 135.0f;

        private void Awake()
        {
            chController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            cameraController = Camera.main.GetComponent<CameraController>();

            s_Instance = this;
        }

        void FixedUpdate()
        {
            ComputeForwardMovement();
            ComputeRotation();

            if (playerInput.IsMoving)
            {
                float rotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, forwardSpeed / desiredForwardSpeed);
                qTargetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    qTargetRotation,
                    rotationSpeed * Time.fixedDeltaTime);
                transform.rotation = qTargetRotation;
            }
            chController.Move(playerInput.moveInput.normalized * Time.fixedDeltaTime * 10);
        }

        private void ComputeForwardMovement()
        {
            Vector3 moveInput = playerInput.moveInput.normalized;
            desiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            float accel = playerInput.IsMoving ? acceleration : deceleration;

            forwardSpeed = Mathf.MoveTowards(
                forwardSpeed,
                desiredForwardSpeed,
                Time.fixedDeltaTime * accel);

            animator.SetFloat(hashForwardSpeed, forwardSpeed);
        }

        private void ComputeRotation()
        {
            Vector3 moveInput = playerInput.moveInput.normalized;

            // direction of camera
            // multiplying by vector3.forward turns quaternion into vector3
            Vector3 cameraDirection = Quaternion.Euler(
                0,
                cameraController.PlayerCam.m_XAxis.Value,
                0) * Vector3.forward;

            Quaternion targetRotation;

            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1.0f)) // dot product of -1 means going backwards
            {
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            }
            else
            {
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);
            }

            qTargetRotation = targetRotation;

        }

    }
}


