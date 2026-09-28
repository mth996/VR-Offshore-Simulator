using System.Collections;
using UnityEngine;

public class AutoValveRotator : MonoBehaviour
{
    public float rotationAngle = 540f; // 1.5 turns
    public float rotationSpeed = 120f;
    public bool IsRotationDone { get; private set; }

    Coroutine rotateRoutine;

    public void RotateClockwise()
    {
        StartRotation(-rotationAngle);
    }

    public void RotateAntiClockwise()
    {
        StartRotation(rotationAngle);
    }

    void StartRotation(float angle)
    {
        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(Rotate(angle));
    }

    IEnumerator Rotate(float angle)
    {
        IsRotationDone = false;

        float rotated = 0f;

        while (Mathf.Abs(rotated) < Mathf.Abs(angle))
        {
            float step = rotationSpeed * Time.deltaTime * Mathf.Sign(angle);
            transform.Rotate(Vector3.up, step, Space.Self);
            rotated += step;
            yield return null;
        }

        IsRotationDone = true;
    }
}