using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

namespace DAUEscape
{
    public class MonkeyBehaviour : MonoBehaviour
    {
        public PlayerScanner playerScanner;
        public float timeToStopPursuit = 2.0f; // if target out of detection range for this many seconds, stop pursuit
        public float waitUntilMove = 2.0f; // when pursuit stops, how many seconds should NavMesh agent wait before moving again
        public float attackDistance = 1.0f; // need to be closer than this distance to player in order to attack them

        private PlayerController currentTarget; // previously detected target that monkey is currently chasing/attacking
        private EnemyController enemyController;
        private Animator animator;
        private float timeSinceLostTarget = 0;
        private Vector3 originalPosition; // monkey's position when the game starts
        private Quaternion originalRotation; // monkey's rotation when the game starts

        private readonly int hashInPursuit = Animator.StringToHash("InPursuit"); // bool: is monkey currently chasing the player?
        private readonly int hashNearBase = Animator.StringToHash("NearBase"); // bool: is monkey close to its original position? 
        private readonly int hashAttack = Animator.StringToHash("Attack"); // trigger

        private void Awake()
        {
            playerScanner = new PlayerScanner();
            enemyController = GetComponent<EnemyController>();
            animator = GetComponent<Animator>();
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }// Awake


        private void Update()
        {
            var detectedTarget = playerScanner.Detect(transform);

            if (detectedTarget != null) { currentTarget = detectedTarget; } // set target to follow/attack if one has been detected

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
        }// Update


        private void PerformNearBaseTasks()
        {
            // if the monkey is back at its original position (near its base), rotate towards its original rotation
            // set 'nearBase' bool appropriately
            Vector3 toBase = originalPosition - transform.position;
            toBase.y = 0;

            bool nearBase = toBase.magnitude < 0.01f;
            animator.SetBool(hashNearBase, nearBase);

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

            if (toTarget.magnitude <= attackDistance) // in attacking distance so attack target
            {
                // first: rotate towards player (slowly, so use Quaternion.Slerp)
                var toTargetRotation = Quaternion.LookRotation(toTarget, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRotation, Time.deltaTime * 180);

                enemyController.StopFollowTarget();
                animator.SetTrigger(hashAttack);
            }
            else // not in attacking distance so keep chasing them
            {
                animator.SetBool(hashInPursuit, true);
                enemyController.FollowTarget(currentTarget.transform.position);
            }
        }// AttackorFollowTarget


        private void CheckStopPursuit()
        {
            timeSinceLostTarget += Time.deltaTime;

            if (timeSinceLostTarget >= timeToStopPursuit)
            {
                currentTarget = null;
                animator.SetBool(hashInPursuit, false);
                StartCoroutine(WaitOnPursuit());
            }
        }// StopPursuit();


        private IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(waitUntilMove);
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

