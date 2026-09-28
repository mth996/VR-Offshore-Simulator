using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarFollowXROrigin : MonoBehaviour
{
    public Transform xrOrigin;

    void LateUpdate()
    {
        transform.position = new Vector3(
            xrOrigin.position.x,
            0f, // LOCK TO FLOOR
            xrOrigin.position.z
        );

        transform.rotation = Quaternion.Euler(
            0f,
            xrOrigin.eulerAngles.y,
            0f
        );
    }
}
