using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class PPEItemWearable : MonoBehaviour
{
    public PPEType type;

    [Header("Snap Settings")]
    public bool lockRotation = true;
    public bool lockPosition = true;

    Rigidbody rb;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    Collider col;

    bool isWorn = false;
    Transform originalParent;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        col = GetComponent<Collider>();
        originalParent = transform.parent;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isWorn) return;

        // Only react if we bumped into a WearPoint
        if (other.TryGetComponent<WearPoint>(out var wearPoint))
        {
            if (wearPoint.type != type) return;

            AttachToWearPoint(wearPoint);
        }
    }

    void AttachToWearPoint(WearPoint point)
    {
        isWorn = true;

        // Make it stick to the avatar
        transform.SetParent(point.transform);

        if (lockPosition)
            transform.localPosition = point.localPositionOffset;

        if (lockRotation)
            transform.localRotation = Quaternion.Euler(point.localRotationOffset);

        // Turn off physics so it doesn't fall
        rb.isKinematic = true;
        rb.useGravity = false;

        // Optional: make collider a trigger so it doesn't hit things
        col.isTrigger = true;

        // Optional: stop re-grab once worn
        grab.enabled = false;

        // Tell manager
        PPEManager.Instance?.SetPPEWorn(type, true);
    }

    // If you want to support taking it off later:
    public void DetachFromBody()
    {
        isWorn = false;

        transform.SetParent(originalParent);
        rb.isKinematic = false;
        rb.useGravity = true;
        col.isTrigger = false;
        grab.enabled = true;

        PPEManager.Instance?.SetPPEWorn(type, false);
    }
}
