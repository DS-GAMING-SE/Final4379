using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSceneOnTouch : MonoBehaviour
{
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    private string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputHandler.TouchStarted += GoToScene;
    }

    private void OnDestroy()
    {
        inputHandler.TouchStarted -= GoToScene;
    }
    private void GoToScene(Vector2 position)
    {
        SceneManager.LoadScene(sceneName);
    }
}
