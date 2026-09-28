using UnityEngine;

public class TaskZone : MonoBehaviour
{
    public HazardManager hazardManager;
    public HazardStep step;

    [Header("Hold To Complete")]
    public bool completeOnStay = true;
    public float stayTime = 2f;

    float timer;
    bool playerInside;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!hazardManager.IsStepActive(step)) return;

        playerInside = true;
        timer = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        timer = 0f;
    }

    void Update()
    {
        if (!playerInside) return;
        if (!completeOnStay) return;
        if (!hazardManager.IsStepActive(step)) return;

        timer += Time.deltaTime;

        if (timer >= stayTime)
        {
            playerInside = false;
            hazardManager.OnStepCompleted(step);
        }
    }
}