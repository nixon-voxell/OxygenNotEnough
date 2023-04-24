using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameOverMenu gameOverMenu;
    public void Gameover()
    {
        this.gameOverMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        GameManager.Instance.StartGame();
        this.gameOverMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }


    public void Quit()
    {
        GameManager.Instance.EndGame();
        Time.timeScale = 1f;
    }
    
}

