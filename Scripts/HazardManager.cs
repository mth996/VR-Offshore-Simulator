using System.Collections;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    [Header("Trigger GameObjects")]
    public GameObject step1Trigger;   // GoToTankArea
    public GameObject step2Trigger;   // CloseValve trigger
    public GameObject weldTrigger;    // WeldLeak trigger
    public GameObject step4Trigger;   // ReopenValve trigger
    public GameObject boatTrigger;    // EvacuateBoat trigger

    [Header("Highlights")]
    public GameObject step1Highlight;
    public GameObject step2Highlight;
    public GameObject weldHighlight;
    public GameObject step4Highlight;
    public GameObject boatHighlight;

    [Header("VFX / Audio")]
    public ParticleSystem leakVfx;
    public AudioSource sirenAudio;
    public ParticleSystem torchWeldVfx;
    public ParticleSystem torchWeldVfx2;

    [Header("UI")]
    public GameObject startIntroUI;
    public GameObject congratsPanel;

    [Header("Timing")]
    public float hazardDelay = 5f;
    public float vfxOnlyDuration = 2f;
    public float blinkInterval = 0.5f;
    public float completionDelayAfterReopen = 2f;

    [Header("Fail Safe Object (ExplosionFailSafe holder)")]
    public GameObject ExpFs;

    public HazardStep CurrentStep { get; private set; } = HazardStep.None;

    // used to cancel explosion if player reached tank in time
    public bool HasReachedTank { get; private set; } = false;

    bool _hazardStarted = false;

    Coroutine _blinkRoutine;
    GameObject _currentHighlight;

    void Awake()
    {
        DeactivateAllTriggers();
        DeactivateAllHighlights();

        if (startIntroUI != null) startIntroUI.SetActive(true);

        if (leakVfx != null)
            leakVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (sirenAudio != null)
            sirenAudio.Stop();

        if (congratsPanel != null)
            congratsPanel.SetActive(false);

        if (ExpFs != null)
            ExpFs.SetActive(false);
        
        if (torchWeldVfx != null)
            torchWeldVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (torchWeldVfx2 != null)
            torchWeldVfx2.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        CurrentStep = HazardStep.None;
        HasReachedTank = false;
        _hazardStarted = false;
    }

    // Call from UI button OnClick
    public void StartHazard()
    {
        if (_hazardStarted) return;
        _hazardStarted = true;

        if (startIntroUI != null) startIntroUI.SetActive(false);

        StartCoroutine(HazardSequence());
    }

    IEnumerator HazardSequence()
    {
        yield return new WaitForSeconds(hazardDelay);

        if (leakVfx != null)
            leakVfx.Play();

        yield return new WaitForSeconds(vfxOnlyDuration);

        if (sirenAudio != null)
            sirenAudio.Play();

        if (ExpFs != null)
            ExpFs.SetActive(true);

        GoToStep(HazardStep.GoToTankArea);
    }

    public bool IsStepActive(HazardStep step) => step == CurrentStep;

    public void OnStepCompleted(HazardStep step)
    {
        if (step != CurrentStep) return;

        StopBlink();
        GetHighlight(step)?.SetActive(false);

        switch (step)
        {
            case HazardStep.GoToTankArea:
                HasReachedTank = true;
                GoToStep(HazardStep.CloseValve);
                break;

            case HazardStep.CloseValve:
                // ✅ NOW: stop BOTH siren + leak VFX when valve is closed
                StopLeakAndSiren();

                // ✅ also disable failsafe so it cannot explode after isolation
                if (ExpFs != null)
                    ExpFs.SetActive(false);

                GoToStep(HazardStep.WeldLeak);
                break;

            case HazardStep.WeldLeak:
                // welding completed -> go reopen valve
                GoToStep(HazardStep.ReopenValve);
                break;

            case HazardStep.ReopenValve:
                StartCoroutine(CompleteAfterDelay());
                break;

            case HazardStep.EvacuateBoat:
                StopLeakAndSiren();
                GoToStep(HazardStep.Completed);
                if (congratsPanel != null) congratsPanel.SetActive(true);
                break;
        }
    }

    IEnumerator CompleteAfterDelay()
    {
        yield return new WaitForSeconds(completionDelayAfterReopen);

        GoToStep(HazardStep.Completed);

        if (congratsPanel != null)
            congratsPanel.SetActive(true);
    }

    void StopLeakAndSiren()
    {
        if (leakVfx != null)
            leakVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (sirenAudio != null)
            sirenAudio.Stop();
    }

    public void EnterEvacuationMode()
    {
        DeactivateAllTriggers();
        DeactivateAllHighlights();
        StopBlink();

        StopLeakAndSiren();

        CurrentStep = HazardStep.EvacuateBoat;
    }

    public void ActivateBoatObjective()
    {
        DeactivateAllTriggers();
        DeactivateAllHighlights();
        StopBlink();

        if (boatTrigger != null) boatTrigger.SetActive(true);

        if (boatHighlight != null)
        {
            boatHighlight.SetActive(true);
            _currentHighlight = boatHighlight;
            _blinkRoutine = StartCoroutine(BlinkHighlight());
        }

        Debug.Log("Evacuate: Go to boat!");
    }

    void GoToStep(HazardStep step)
    {
        CurrentStep = step;

        DeactivateAllTriggers();
        DeactivateAllHighlights();
        StopBlink();

        if (step == HazardStep.Completed)
            return;

        GameObject trigger = GetTrigger(step);
        GameObject highlight = GetHighlight(step);

        if (trigger != null) trigger.SetActive(true);

        if (highlight != null)
        {
            highlight.SetActive(true);
            _currentHighlight = highlight;
            _blinkRoutine = StartCoroutine(BlinkHighlight());
        }

        Debug.Log("Activated step: " + step);
    }

    GameObject GetTrigger(HazardStep step)
    {
        switch (step)
        {
            case HazardStep.GoToTankArea: return step1Trigger;
            case HazardStep.CloseValve: return step2Trigger;
            case HazardStep.WeldLeak: return weldTrigger;
            case HazardStep.ReopenValve: return step4Trigger;
            case HazardStep.EvacuateBoat: return boatTrigger;
        }
        return null;
    }

    GameObject GetHighlight(HazardStep step)
    {
        switch (step)
        {
            case HazardStep.GoToTankArea: return step1Highlight;
            case HazardStep.CloseValve: return step2Highlight;
            case HazardStep.WeldLeak: return weldHighlight;
            case HazardStep.ReopenValve: return step4Highlight;
            case HazardStep.EvacuateBoat: return boatHighlight;
        }
        return null;
    }

    void DeactivateAllTriggers()
    {
        if (step1Trigger) step1Trigger.SetActive(false);
        if (step2Trigger) step2Trigger.SetActive(false);
        if (weldTrigger) weldTrigger.SetActive(false);
        if (step4Trigger) step4Trigger.SetActive(false);
        if (boatTrigger) boatTrigger.SetActive(false);
    }

    void DeactivateAllHighlights()
    {
        if (step1Highlight) step1Highlight.SetActive(false);
        if (step2Highlight) step2Highlight.SetActive(false);
        if (weldHighlight) weldHighlight.SetActive(false);
        if (step4Highlight) step4Highlight.SetActive(false);
        if (boatHighlight) boatHighlight.SetActive(false);
    }

    void StopBlink()
    {
        if (_blinkRoutine != null) StopCoroutine(_blinkRoutine);
        _blinkRoutine = null;

        if (_currentHighlight != null)
            _currentHighlight.SetActive(true);
    }

    IEnumerator BlinkHighlight()
    {
        while (_currentHighlight != null)
        {
            _currentHighlight.SetActive(!_currentHighlight.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}