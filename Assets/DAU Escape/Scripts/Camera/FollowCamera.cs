using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform lookAt;

    void LateUpdate()
    {
        if (!target) { return; }

        float currentRotationAngle = transform.eulerAngles.y;
        float wantedRotationAngle = target.eulerAngles.y;

        // LerpAngle allows for smoother rotation from current to wanted angle instead of a sharp jump by providing intermediate values
        currentRotationAngle = Mathf.LerpAngle(
            currentRotationAngle,
            wantedRotationAngle,
            0.5f
        );

        transform.position = new Vector3(target.position.x, target.position.y + 3.0f, target.position.z); // 3.0f

        // currentRotationAngle degrees around the y-axis
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // rotate vector forward currentRotationAngle degrees around the y-axis
        Vector3 rotatedPosition = currentRotation * Vector3.forward;

        transform.position = transform.position - rotatedPosition * 5; // 5 units behind player

        transform.LookAt(lookAt);
    }
}