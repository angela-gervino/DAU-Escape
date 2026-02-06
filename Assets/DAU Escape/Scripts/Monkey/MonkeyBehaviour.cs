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

        private PlayerController m_Target;
        private EnemyController enemyController;
        private Animator animator;
        private float timeSinceLostTarget = 0;
        private Vector3 originPosition;

        private readonly int hashInPursuit = Animator.StringToHash("InPursuit"); // bool: is monkey currently chasing the player?
        private readonly int hashNearBase = Animator.StringToHash("NearBase"); // bool: is monkey close to its original position? 
        private readonly int hashAttack = Animator.StringToHash("Attack");

        private void Awake()
        {
            playerScanner = new PlayerScanner();
            enemyController = GetComponent<EnemyController>();
            animator = GetComponent<Animator>();
            originPosition = transform.position;
        }

        private void Update()
        {
            var target = playerScanner.Detect(transform);

            if (m_Target == null)
            {
                if (target != null) // just detected target
                {
                    m_Target = target;
                }
            }
            else
            {
                Vector3 toTarget = m_Target.transform.position - transform.position;
                if (toTarget.magnitude <= attackDistance)
                {
                    enemyController.StopFollowTarget();
                    animator.SetTrigger(hashAttack);
                }
                else
                {
                    animator.SetBool(hashInPursuit, true);
                    enemyController.FollowTarget(m_Target.transform.position);
                }

                if (target == null) // not in detection range
                {
                    timeSinceLostTarget += Time.deltaTime;

                    if (timeSinceLostTarget >= timeToStopPursuit)
                    {
                        m_Target = null;
                        animator.SetBool(hashInPursuit, false);
                        StartCoroutine(WaitOnPursuit());
                    }
                }
                else // target is in range so have not lost target
                {
                    timeSinceLostTarget = 0;
                }

            }

            Vector3 toBase = originPosition - transform.position;
            toBase.y = 0;

            animator.SetBool(hashNearBase, toBase.magnitude < 0.01f);
        }

        private IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(waitUntilMove);
            enemyController.FollowTarget(originPosition); // don't want to keep following player since pursuit has stopped so go back to origin pos
        }

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
        }
#endif
    }
}

