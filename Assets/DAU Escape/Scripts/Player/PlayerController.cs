
using System;
using System.Collections;
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

        public float walkSpeed = 2;
        public float rotationSpeed = 10;
        public MeleeWeapon meleeWeapon;

        // s_ denotes static variables
        private static PlayerController s_Instance;
        private CharacterController chController;
        private Animator animator;
        private Vector3 movement;

        private readonly int hashAttack = Animator.StringToHash("Attack");

        private void Awake()
        {
            chController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            s_Instance = this;
        }// Awake


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
        }// FixedUpdate

        private void Update()
        {
            animator.ResetTrigger(hashAttack);
            if (Input.GetButtonDown("Fire1")) // left button on mouse
            {
                animator.SetTrigger(hashAttack);


            }
        }

        public void MeleeAttackStart()
        {
            meleeWeapon.BeginAttack();
        }

        public void MeleeAttackEnd()
        {
            meleeWeapon.EndAttack();
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
        }// RotatePlayer


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
        }// MovePlayer

    }
}


