using UnityEngine;

namespace DAUEscape
{
    public class MonkeyBehaviour : MonoBehaviour
    {
        public float detectionRadius = 10.0f;
        public float detectionAngle = 90.0f;

        private void Start()
        {
            Debug.Log(PlayerController.Instance.maxForwardSpeed);
        }

        private void Update()
        {
            LookForPlayer();
        }

        private PlayerController LookForPlayer()
        {
            if (PlayerController.Instance == null)
                return null;

            Vector3 enemyPosition = transform.position;
            Vector3 toPlayer = PlayerController.Instance.transform.position - enemyPosition;
            toPlayer.y = 0;

            // magnitude of toPlayer is the distance between player and enemy
            // P - E results in a vector indicating "how to move to get from E to P"
            if (toPlayer.magnitude <= detectionRadius)
            {
                // check if player is within the circle sector of the detection range
                // (monkey only sees forward and to its sides, not behind)
                if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    Debug.Log("Player detected");
                }
            }

            return null;
        }

        // method is only part of unity editor for debugging purposes only
        // will not be part of production code
#if UNITY_EDITOR
        // executed when monkey is selected (in scene view)
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0, 0, 0.7f, 0.4f);
            UnityEditor.Handles.color = c;

            Vector3 rotatedForward = Quaternion.Euler(
                0,
                -detectionAngle * 0.5f,
                0) * transform.forward;

            // Draw arc representing detection range of the monkey
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up,
                rotatedForward,
                detectionAngle,
                detectionRadius);
        }
#endif
    }
}

