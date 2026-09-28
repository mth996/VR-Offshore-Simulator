using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;

    

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}