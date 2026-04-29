using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Rendering.PostProcessing;

namespace DAUEscape
{
    public class MonkeyBehaviour : MonoBehaviour, IMessageReceiver
    {
        public PlayerScanner playerScanner;
        private float timeToStopPursuit = 2.0f; // if target out of detection range for this many seconds, stop pursuit
        private float waitUntilMove = 2.0f; // when pursuit stops, how many seconds should NavMesh agent wait before moving again
        private float attackDistance = 1.5f; // need to be closer than this distance to player in order to attack them
        private const float COOLDOWN_TIME = 2; // the amount of time needed between attacks
        private float toCooldownFinished = 0;

        private Animator animator;
        private PlayerController currentTarget; // previously detected target that monkey is currently chasing/attacking
        private EnemyController enemyController;
        private float timeSinceLostTarget = 0;
        private Vector3 originalPosition; // monkey's position when the game starts
        private Quaternion originalRotation; // monkey's rotation when the game starts

        private readonly int hashInPursuit = Animator.StringToHash("InPursuit"); // bool: is monkey currently chasing the player?
        private readonly int hashNearBase = Animator.StringToHash("NearBase"); // bool: is monkey close to its original position? 
        private readonly int hashAttack = Animator.StringToHash("Attack"); // trigger
        private readonly int hashHurt = Animator.StringToHash("Hurt"); // trigger
        private readonly int hashDead = Animator.StringToHash("Dead"); // trigger
        private readonly int hashAttackBlocked = Animator.StringToHash("AttackBlocked"); // bool: attack cooldown time is in effect t/f
        private readonly int hashChase = Animator.StringToHash("Chase"); // in pursuit and should be in running animation

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            animator = GetComponent<Animator>();

            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }// Awake


        private void Update()
        {
            if (PlayerController.Instance.IsRespawning)
            {
                GoToOriginalSpot();
                PerformNearBaseTasks();
                return;
            }

            GuardPosition();
        }// Update


        public void StartCooldown()
        {
            toCooldownFinished = COOLDOWN_TIME;
        }


        private void GoToOriginalSpot()
        {
            currentTarget = null;
            enemyController.Animator.SetBool(hashInPursuit, false);
            enemyController.FollowTarget(originalPosition);
        }


        public void OnReceiveMessage(MessageType type)
        {
            switch (type)
            {
                case MessageType.DEAD:
                    OnDead();
                    break;
                case MessageType.DAMAGED:
                    OnReceiveDamage();

                    // in case monkey is hit from an angle where it can't detect the player
                    currentTarget = PlayerController.Instance; // assuming only the player can deal damage to the monkey
                    FollowTarget();
                    break;
                default:
                    break;

            }
        }// OnReceiveMessage


        private void OnReceiveDamage()
        {
            enemyController.Animator.SetTrigger(hashHurt);
        }// OnReceiveDamage


        private void OnDead()
        {
            // Get rid of the health bar
            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null) { Destroy(canvas); Debug.Log("Here!"); }

            enemyController.Animator.SetTrigger(hashDead);
        }// OnDead


        private void GuardPosition()
        {
            var detectedTarget = playerScanner.Detect(transform);

            if (detectedTarget != null) { currentTarget = detectedTarget; } // set target to if one has been detected

            if (currentTarget != null)
            {
                AttackOrFollowTarget(); // decide whether to attack or chase the detected target

                if (detectedTarget != null) // target is in the detection range (so they are not lost)
                {
                    timeSinceLostTarget = 0;
                }
                else // target has been lost for some amount of time, check if monkey should continue chasing or stop
                {
                    CheckStopPursuit();
                }
            }

            PerformNearBaseTasks();
        }// GuardPosition


        private void PerformNearBaseTasks()
        {
            // if the monkey is back at its original position (near its base), rotate towards its original rotation
            // set 'nearBase' bool appropriately
            Vector3 toBase = originalPosition - transform.position;
            toBase.y = 0;

            bool nearBase = toBase.magnitude < 0.01f;
            enemyController.Animator.SetBool(hashNearBase, nearBase);

            if (nearBase && !currentTarget) // at original position and not in pursuit
            {
                // rotate towards original position
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * 5);
            }
        }// PerformNearBaseTasks


        private void AttackOrFollowTarget()
        {
            // determine whether to attack the target or chase them depending on how far the monkey is from the player
            Vector3 toTarget = currentTarget.transform.position - transform.position;

            if (toCooldownFinished > 0)
            {
                toCooldownFinished = Math.Max(0, toCooldownFinished - Time.deltaTime);

                if (toCooldownFinished == 0)
                {
                    animator.SetBool(hashAttackBlocked, false);
                }
            }


            if (toTarget.magnitude <= attackDistance) // in attacking distance so attack target
            {
                if (toCooldownFinished == 0) { AttackTarget(toTarget); }
                animator.SetBool(hashChase, false); // in attacking distance so not running (should be attacking or waiting on cooldown)
            }
            else // not in attacking distance so keep chasing them
            {
                FollowTarget();
                animator.SetBool(hashChase, true); // in pursuit but not in attack distance so chase target (run)
            }
        }// AttackorFollowTarget


        private void AttackTarget(Vector3 toTarget)
        {
            // first: rotate towards player (slowly, so use Quaternion.Slerp)
            var toTargetRotation = Quaternion.LookRotation(toTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRotation, Time.deltaTime * 180);

            enemyController.StopFollowTarget();
            enemyController.Animator.SetTrigger(hashAttack);
        }// AttackTarget


        private void FollowTarget()
        {
            enemyController.Animator.SetBool(hashInPursuit, true);
            enemyController.FollowTarget(currentTarget.transform.position);
        }// FollowTarget


        private void CheckStopPursuit()
        {
            timeSinceLostTarget += Time.deltaTime;

            if (timeSinceLostTarget >= timeToStopPursuit)
            {
                currentTarget = null;
                enemyController.Animator.SetBool(hashInPursuit, false);
                StartCoroutine(WaitBeforeReturn());
            }
        }// CheckStopPursuit();


        private IEnumerator WaitBeforeReturn()
        {
            yield return new WaitForSeconds(waitUntilMove); // wait this many seconds before returning to original position (base)
            enemyController.FollowTarget(originalPosition); // don't want to keep following player since pursuit has stopped so go back to origin pos
        }// WaitOnPursuit


        // method is part of unity editor for debugging purposes only
        // will not be part of production code
#if UNITY_EDITOR
        // executed when monkey is selected (in scene view)
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0, 0, 0.7f, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotatedForward = Quaternion.Euler(
                0,
                -playerScanner.detectionAngle * 0.5f,
                0) * transform.forward;

            // Draw arc representing detection range of the monkey
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                playerScanner.detectionAngle,
                playerScanner.detectionRadius);

            // Draw circle representing smaller melee detection range around the monkey
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                360,
                playerScanner.meleeDetectionRadius
            );
        }
#endif
    }
}

