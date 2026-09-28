using UnityEngine;

public class WeldZone : MonoBehaviour
{
    public HazardManager hazardManager;
    public HazardStep weldStep = HazardStep.WeldLeak;

    public string torchTipTag = "WeldTorchTip";

    public ParticleSystem torchWeldVfx;
    public ParticleSystem torchWeldVfx2;
    public GameObject weldedPlate;

    public float holdTime = 5f;

    float timer;
    bool completed;

    int touchingCount = 0; // handles multiple colliders correctly

    void Awake()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        if (weldedPlate != null)
            weldedPlate.SetActive(false);

        if (torchWeldVfx != null)
            torchWeldVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (torchWeldVfx2 != null)
            torchWeldVfx2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void OnTriggerEnter(Collider other)
    {
        if (completed) return;
        if (!IsActiveStep()) return;
        if (!IsTorchTip(other)) return;

        touchingCount++;
        timer = 0f;

        if (torchWeldVfx != null && !torchWeldVfx.isPlaying)
            torchWeldVfx.Play();
        if (torchWeldVfx2 != null && !torchWeldVfx.isPlaying)
            torchWeldVfx2.Play();
    }

    void OnTriggerExit(Collider other)
    {
        if (completed) return;
        if (!IsTorchTip(other)) return;

        touchingCount = Mathf.Max(0, touchingCount - 1);

        if (touchingCount == 0)
        {
            timer = 0f;

            if (torchWeldVfx != null)
                torchWeldVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting); 
            if (torchWeldVfx2 != null)
                torchWeldVfx2.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void Update()
    {
        if (completed) return;

        // If step becomes inactive while welding, reset cleanly
        if (!IsActiveStep())
        {
            timer = 0f;
            touchingCount = 0;

            if (torchWeldVfx != null)
                torchWeldVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if (torchWeldVfx2 != null)
                torchWeldVfx2.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            return;
        }

        if (touchingCount <= 0) return;

        timer += Time.deltaTime;

        if (timer >= holdTime)
        {
            completed = true;

            if (torchWeldVfx != null)
                torchWeldVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting); 
            if (torchWeldVfx2 != null)
                torchWeldVfx2.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            if (weldedPlate != null)
                weldedPlate.SetActive(true);

            hazardManager.OnStepCompleted(weldStep);
        }
    }

    bool IsActiveStep()
    {
        return hazardManager != null && hazardManager.IsStepActive(weldStep);
    }

    bool IsTorchTip(Collider other)
    {
        if (other.CompareTag(torchTipTag)) return true;

        Transform t = other.transform;
        while (t != null)
        {
            if (t.CompareTag(torchTipTag)) return true;
            t = t.parent;
        }
        return false;
    }
}