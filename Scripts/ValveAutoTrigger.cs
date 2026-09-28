using System.Collections;
using UnityEngine;

public class ValveAutoTrigger : MonoBehaviour
{
    public enum ValveMode
    {
        CloseValve,
        OpenValve
    }

    [Header("Mode")]
    public ValveMode mode;

    [Header("References")]
    public HazardManager hazardManager;
    public AutoValveRotator valve;

    [Header("Timing")]
    public float completeDelay = 0.1f;

    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (mode == ValveMode.CloseValve)
            valve.RotateClockwise();
        else
            valve.RotateAntiClockwise();

        StartCoroutine(CompleteStepAfterRotation());
    }

    IEnumerator CompleteStepAfterRotation()
    {
        // wait until valve finishes rotating
        yield return new WaitUntil(() => valve.IsRotationDone);

        yield return new WaitForSeconds(completeDelay);

        hazardManager.OnStepCompleted(
            mode == ValveMode.CloseValve
                ? HazardStep.CloseValve
                : HazardStep.ReopenValve
        );
    }
}