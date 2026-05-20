using UnityEngine;

public class loadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
