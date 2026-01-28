using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // target should not be able to be accessed by other classes but 
    // we want to be able to access it in the inspector window
    [SerializeField]
    private Transform target;

    void LateUpdate()
    {
        if (!target)
            return;

        float currentRotationAngle = transform.eulerAngles.y;
        float wantedRotationAngle = target.eulerAngles.y;

        // provide intermediate values between curr and wanted so camera doesn't 
        // jump too fast (smooths out camera movement)
        currentRotationAngle = Mathf.LerpAngle(
            currentRotationAngle,
            wantedRotationAngle,
            0.5f);

        // camera position is the same as player position in x,z
        // but a bit above the player (higher y)
        transform.position = new Vector3(
            target.position.x,
            5.0f,
            target.position.z);

        // rotate by currentRotationAngle degrees around the y-axis
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        Vector3 rotatedPosition = currentRotation * Vector3.forward;

        transform.position -= rotatedPosition * 10;

        transform.LookAt(target);
    }
}
