using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Gameover()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MazeDemo 1");
    }


    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
