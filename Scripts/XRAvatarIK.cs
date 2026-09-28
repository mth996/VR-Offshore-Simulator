using Unity.XR.CoreUtils;
using UnityEngine;

public class XRAvatarIK : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform torsoParentTransform;
    [SerializeField] private Transform headVisualsRoot;
    [SerializeField] private Transform neck;

    [Header("Settings")]
    [SerializeField] private float headHeightOffset = 0.3f;
    [SerializeField] private float rotateThreshold = 25f;
    [SerializeField] private float rotateSpeed = 3f;

    private Transform xrHead;
    private XROrigin xrOrigin;

    private void Start()
    {
        xrOrigin = FindFirstObjectByType<XROrigin>();

        if (xrOrigin == null || xrOrigin.Camera == null)
        {
            Debug.LogError("XRAvatarIK: XROrigin or Camera not found.");
            enabled = false;
            return;
        }

        xrHead = xrOrigin.Camera.transform;
    }

    private void LateUpdate()
    {
        if (xrHead == null) return;

        // 1️⃣ Head visuals follow XR camera (with height offset)
        headVisualsRoot.position = xrHead.position + Vector3.up * headHeightOffset;

        // 2️⃣ Neck rotation (yaw only)
        Vector3 headForward = Vector3.ProjectOnPlane(xrHead.forward, Vector3.up);
        Quaternion targetRotation = Quaternion.LookRotation(headForward);

        neck.rotation = Quaternion.Slerp(
            neck.rotation,
            targetRotation,
            Time.deltaTime * rotateSpeed
        );

        // 3️⃣ Rotate body when exceeding threshold
        float angle = Vector3.Angle(torsoParentTransform.forward, headForward);

        if (angle > rotateThreshold)
        {
            torsoParentTransform.rotation = Quaternion.Slerp(
                torsoParentTransform.rotation,
                targetRotation,
                Time.deltaTime * rotateSpeed
            );
        }
    }
}
