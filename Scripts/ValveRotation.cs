using UnityEngine;

public class ValveRotation : MonoBehaviour
{
    [Header("Valve Settings")]
    [Tooltip("Total rotation required to fully close the valve (degrees)")]
    public float requiredCloseAngle = 540f; // 1.5 turns

    [Tooltip("Tolerance for angle comparison")]
    public float tolerance = 5f;

    [Header("State (Read Only)")]
    public bool isFullyClosed;
    public bool isFullyOpen;

    float startAngle;

    void Start()
    {
        // Store initial rotation angle
        startAngle = GetCurrentAngle();
    }

    void Update()
    {
        float currentAngle = GetCurrentAngle();
        float deltaAngle = Mathf.Abs(currentAngle - startAngle);

        isFullyClosed = deltaAngle >= (requiredCloseAngle - tolerance);
        isFullyOpen   = deltaAngle <= tolerance;
    }

    float GetCurrentAngle()
    {
        // IMPORTANT:
        // Change this axis ONLY if your valve rotates on a different axis
        // Z is most common for valve wheels
        return transform.localEulerAngles.z;
    }
}