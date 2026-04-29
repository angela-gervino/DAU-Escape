
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DAUEscape
{
    public class PlayerController : MonoBehaviour, IAttackAnimListener, IMessageReceiver
    {
        // let enemy classes get access to player class through static variables
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }

        public bool IsRespawning { get { return isRespawning; } }

        public MeleeWeapon meleeWeapon;

        // s_ denotes static variables
        private static PlayerController s_Instance;

        private CharacterController chController;
        private Animator animator;
        private Vector3 movement;
        private Damageable damageable;
        private float walkSpeed = 10;
        private float rotationSpeed = 1.5f;
        private float gravity = -10.0f;

        private AnimatorStateInfo currentStateInfo;
        private AnimatorStateInfo nextStateInfo;
        private bool isAnimatorTransitioning;
        private bool inputBlocked;
        private bool isRespawning;
        private static bool dialogueInProgress;

        private Vector3 originalPosition;
        private Quaternion originalRotation;

        // Animator Trigger Hashes
        private readonly int hashAttack = Animator.StringToHash("Attack");
        private readonly int hashHurt = Animator.StringToHash("Hurt");
        private readonly int hashDead = Animator.StringToHash("Dead");

        // Animator Tag Hashes
        private readonly int hashBlockInput = Animator.StringToHash("BlockInput");

        public static void UpdateDialogueStatus(bool value)
        {
            dialogueInProgress = value;
        }

        private void Awake()
        {
            chController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
            s_Instance = this;

            originalPosition = transform.position;
            originalRotation = transform.rotation;

            meleeWeapon.SetOwner(gameObject);
        }// Awake


        void FixedUpdate()
        {
            CacheAnimationState();
            UpdateInputBlocking();

            if (isRespawning) { return; }

            if (!inputBlocked)
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

        }// FixedUpdate


        private void Update()
        {
            animator.ResetTrigger(hashAttack);
            if (Input.GetButtonDown("Fire1") && !inputBlocked) // left button on mouse and player input is not blocked
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


        public void StartRespawn()
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            damageable.ResetHP();
        }


        public void FinishRespawn()
        {
            isRespawning = false;
        }


        public void OnReceiveMessage(MessageType type)
        {
            switch (type)
            {
                case MessageType.DEAD:
                    isRespawning = true;
                    animator.SetTrigger(hashDead);
                    break;
                case MessageType.DAMAGED:
                    animator.SetTrigger(hashHurt);
                    break;
                default:
                    break;

            }
        }// OnReceiveMessage


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


        private void CacheAnimationState()
        {
            currentStateInfo = animator.GetCurrentAnimatorStateInfo(0); // animator state from default/base layer (0)
            nextStateInfo = animator.GetNextAnimatorStateInfo(0);
            isAnimatorTransitioning = animator.IsInTransition(0);
        }// CacheAnimationState


        // based on the current state (player animation or transition) we can block the player/user from providing input
        private void UpdateInputBlocking()
        {
            inputBlocked = currentStateInfo.tagHash == hashBlockInput && !isAnimatorTransitioning;
            inputBlocked |= nextStateInfo.tagHash == hashBlockInput; // curr state blocked, or one we're transitioning into is blocked
            inputBlocked |= currentStateInfo.IsName("Take Damage"); // if taking damage, block ability to move or attack
            inputBlocked |= currentStateInfo.IsName("Attack");
            inputBlocked |= dialogueInProgress;
        }// UpdateInputBlocking
    }
}


