
using System;
using System.Collections;
using UnityEngine;

namespace DAUEscape
{
    public class PlayerController : MonoBehaviour, IAttackAnimListener
    {
        // let enemy classes get access to player class through static variables
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }

        public MeleeWeapon meleeWeapon;

        // s_ denotes static variables
        private static PlayerController s_Instance;

        private CharacterController chController;
        private Animator animator;
        private Vector3 movement;
        private float walkSpeed = 10;
        private float rotationSpeed = 1.3f;
        private float gravity = -10.0f;


        private readonly int hashAttack = Animator.StringToHash("Attack");

        private void Awake()
        {
            chController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            s_Instance = this;

            meleeWeapon.SetOwner(gameObject);
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
        }// Update


        public void MeleeAttackStart()
        {
            meleeWeapon.BeginAttack();
        }// MeleeAttackStart


        public void MeleeAttackEnd()
        {
            meleeWeapon.EndAttack();
        }// MeleeAttackEnd


        private void RotatePlayer()
        {
            if (movement.x != 0) // player is rotating left/right
            {
                if (movement.x > 0) // rotating to the right
                {
                    chController.transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(transform.right),
                        Time.fixedDeltaTime * rotationSpeed);
                }
                else // rotating to the left
                {
                    chController.transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(-transform.right),
                        Time.fixedDeltaTime * rotationSpeed);
                }
            }
        }// RotatePlayer


        private void MovePlayer()
        {
            Vector3 moveInDirection = gravity * Vector3.up * Time.fixedDeltaTime;
            Vector3 chControllerFwd = chController.transform.forward;
            chControllerFwd.y = 0;
            chControllerFwd = chControllerFwd.normalized;

            if (movement.z != 0) // player is moving forward/backward
            {
                if (movement.z > 0) // forward
                {
                    moveInDirection += chControllerFwd * Time.fixedDeltaTime * walkSpeed;
                }
                else // backward
                {
                    moveInDirection += -chControllerFwd * Time.fixedDeltaTime * walkSpeed;
                }
            }

            chController.Move(moveInDirection);
        }// MovePlayer

    }
}


