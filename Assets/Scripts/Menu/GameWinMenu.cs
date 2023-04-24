using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinMenu : MonoBehaviour
{
    public GameWinMenu gameWinMenu;

    public void GameWin()
    {
        this.gameWinMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        GameManager.Instance.StartGame();
        this.gameWinMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }


    public void Quit()
    {
        GameManager.Instance.EndGame();
        Time.timeScale = 1f;
    }
}
