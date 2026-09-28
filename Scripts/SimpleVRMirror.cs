using UnityEngine;

public class SimpleVRMirror : MonoBehaviour
{
    public Transform xrCamera;
    public Transform mirrorPlane;

    [Header("Inspector Offset")]
    public Vector3 positionOffset;   // <-- adjust in Inspector

    void LateUpdate()
    {
        if (!xrCamera || !mirrorPlane) return;

        // Reflect position
        Vector3 localPos = mirrorPlane.InverseTransformPoint(xrCamera.position);
        localPos.z *= -1f;

        // Apply inspector offset
        localPos += positionOffset;

        transform.position = mirrorPlane.TransformPoint(localPos);

        // Rotation (front-facing mirror)
        Vector3 rot = xrCamera.eulerAngles;
        rot.y -= 180f;
        transform.rotation = Quaternion.Euler(rot);
    }
}
