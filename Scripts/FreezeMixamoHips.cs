using UnityEngine;

public class FreezeMixamoHips : MonoBehaviour
{
    public Transform hips;   // assign in Inspector

    Vector3 hipsLocalPos;
    Quaternion hipsLocalRot;

    void Start()
    {
        if (hips == null)
        {
            Debug.LogError("Hips not assigned!");
            enabled = false;
            return;
        }

        hipsLocalPos = hips.localPosition;
        hipsLocalRot = hips.localRotation;
    }

    void LateUpdate()
    {
        hips.localPosition = hipsLocalPos;
        hips.localRotation = hipsLocalRot;
    }
}
