using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAUEscape;

[System.Serializable]
public class PlayerScanner
{
    public float detectionRadius = 10.0f;
    public float detectionAngle = 90.0f;

    public PlayerController Detect(Transform detector)
    {
        if (PlayerController.Instance == null)
            return null;

        Vector3 toPlayer = PlayerController.Instance.transform.position - detector.position;
        toPlayer.y = 0;

        // magnitude of toPlayer is the distance between player and enemy
        // P - E results in a vector indicating "how to move to get from E to P"
        if (toPlayer.magnitude <= detectionRadius)
        {
            // check if player is within the circle sector of the detection range
            // (monkey only sees forward and to its sides, not behind)
            if (Vector3.Dot(toPlayer.normalized, detector.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                return PlayerController.Instance;
            }
        }

        return null;
    }
}
