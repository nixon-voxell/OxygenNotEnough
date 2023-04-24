using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private int timeofESC=0;
    public void Resume()
    {
        GameManager.Instance.Resume();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause()
    {
        GameManager.Instance.Pause();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        GameManager.Instance.EndGame();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(timeofESC%2==0) this.Pause(); 
            else this.Resume();         
            timeofESC++;
        }
        
    }
}
