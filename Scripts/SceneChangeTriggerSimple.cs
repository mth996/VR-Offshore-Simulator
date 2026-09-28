using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTriggerSimple : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;

    [Header("Player Settings")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        SceneManager.LoadScene(sceneToLoad);
    }
}