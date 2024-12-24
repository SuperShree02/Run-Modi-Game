using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            Restart();
        }
    }
    public void Restart()
    {
        Time.timeScale = 1; // Resume game
        SceneManager.LoadScene(0); // Reload scene
    }
}
