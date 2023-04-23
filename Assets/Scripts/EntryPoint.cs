using UnityEngine;
using UnityEngine.SceneManagement;
using Voxell.Util;

public class EntryPoint : MonoBehaviour
{
    [SerializeField, SceneRef] private string m_Menu;
    [SerializeField, SceneRef] private string m_Maze;

    public void Awake()
    {
        Scene[] loadedScenes = new Scene[SceneManager.sceneCount];
        for (int s = 0; s < loadedScenes.Length; s++)
        {
            loadedScenes[s] = SceneManager.GetSceneAt(s);
        }

        this.LoadSceneIfNotExists(loadedScenes, this.m_Menu, LoadSceneMode.Additive);
        this.LoadSceneIfNotExists(loadedScenes, this.m_Maze, LoadSceneMode.Additive);
    }

    private void LoadSceneIfNotExists(Scene[] loadedScenes, string sceneName, LoadSceneMode mode)
    {
        if (!System.Array.Exists(loadedScenes, (m) => m.name == sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName, mode);
        }
    }
}
