using System.Collections;
using UnityEngine;

public class ExplosionFailSafe : MonoBehaviour
{
    [Header("References")]
    public HazardManager hazardManager;

    [Header("Timing")]
    public float timeBeforeExplosion = 15f;

    [Header("Explosion")]
    public ParticleSystem explosionVfx;
    public AudioSource explosionAudio;
    public GameObject explosion;    public float explosionDuration = 2f;

    [Header("Fire (after explosion)")]
    public ParticleSystem fireVfx;
    public AudioSource fireAudio;

    bool _triggered;

    void OnEnable()
    {
        if (explosion != null)
            explosion.SetActive(true);
        _triggered = false;

        if (explosionVfx != null)
            explosionVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (fireVfx != null)
            fireVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        StartCoroutine(ExplosionTimer());
    }

    IEnumerator ExplosionTimer()
    {
        float t = 0f;

        while (t < timeBeforeExplosion)
        {
            // ✅ Cancel explosion if player already reached tank (step1 completed)
            if (hazardManager != null && hazardManager.HasReachedTank)
                yield break;

            t += Time.deltaTime;
            yield return null;
        }

        if (_triggered) yield break;
        _triggered = true;

        // Explosion once
        if (explosionVfx != null) explosionVfx.Play();
        if (explosionAudio != null) explosionAudio.Play();

        yield return new WaitForSeconds(explosionDuration);

        if (explosionVfx != null)
            explosionVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        // Fire stays on
        if (fireVfx != null) fireVfx.Play();
        if (fireAudio != null) fireAudio.Play();

        // Switch to evacuation
        if (hazardManager != null)
        {
            hazardManager.EnterEvacuationMode();
            hazardManager.ActivateBoatObjective();
        }
    }
}