using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_Panel;
    public void PlayGame()
    {
        this.m_Panel.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void ExitGame()
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}
