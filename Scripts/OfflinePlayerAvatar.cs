using Unity.XR.CoreUtils;
using UnityEngine;

public class OfflinePlayerAvatar : MonoBehaviour
{
    [Header("Avatar References")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private SkinnedMeshRenderer headRenderer; // Optional (for hiding head)

    private Transform xrHead;

    private void Start()
    {
        XROrigin xrOrigin = FindFirstObjectByType<XROrigin>();

        if (xrOrigin != null && xrOrigin.Camera != null)
        {
            xrHead = xrOrigin.Camera.transform;
        }
        else
        {
            Debug.LogError("OfflinePlayerAvatar: XROrigin or XR Camera not found.");
        }
    }

    private void LateUpdate()
    {
        if (xrHead == null || headTransform == null)
            return;

        // Follow XR headset
        headTransform.SetPositionAndRotation(
            xrHead.position,
            xrHead.rotation
        );
    }

    /// <summary>
    /// Optional: Hide or show the head mesh (useful for first-person view).
    /// </summary>
    public void SetHeadVisible(bool visible)
    {
        if (headRenderer != null)
            headRenderer.enabled = visible;
    }
}
