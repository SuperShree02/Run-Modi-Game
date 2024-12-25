using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1; // Resume game
        SceneManager.LoadScene(1); // Reload scene
    }

    public void quitGame()
    {
        Debug.Log("Quit!!");
        Application.Quit();
    }
}
