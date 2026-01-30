
using System;
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

        public Camera cam;

        public float walkSpeed = 10;
        public float rotationSpeed = 10;

        // s_ denotes static variables
        private static PlayerController s_Instance;
        private CharacterController chController;
        private Animator animator;
        private Vector3 movement;

        private void Awake()
        {
            chController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            s_Instance = this;
        }


        void FixedUpdate()
        {
            movement.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // x movement controls rotation and z movement controls forward/backward movement
            if (Mathf.Approximately(movement.z, 0)) // not moving
            {
                animator.SetBool("isMoving", false);
                if (Mathf.Approximately(movement.x, 0)) // no rotating
                {
                    animator.SetBool("onlyRotating", false);
                }
                else // no movement but is rotating
                {
                    animator.SetBool("onlyRotating", true);
                }
            }
            else // moving
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("onlyRotating", false); // if moving don't animate for rotating
            }

            animator.SetFloat("speed", Mathf.Max(Mathf.Abs(movement.z), Mathf.Abs(movement.x / 2.0f)));

            RotatePlayer();
            MovePlayer();
        }

        private void RotatePlayer()
        {
            if (movement.x != 0) // player is rotating left/right
            {
                if (movement.x > 0) // rotating to the right
                {
                    chController.transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(transform.right),
                        Time.fixedDeltaTime);
                }
                else // rotating to the left
                {
                    chController.transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(-transform.right),
                        Time.fixedDeltaTime);
                }
            }
        }

        private void MovePlayer()
        {
            if (movement.z != 0) // player is moving forward/backward
            {
                if (movement.z > 0) // forward
                {
                    chController.Move(chController.transform.forward * Time.fixedDeltaTime * walkSpeed);
                }
                else // backward
                {
                    chController.Move(-chController.transform.forward * Time.fixedDeltaTime * walkSpeed);
                }
            }
        }

        /*
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

        }*/

    }
}


