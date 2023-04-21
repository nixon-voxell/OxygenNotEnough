using UnityEngine;
using UnityEngine.SceneManagement;
using Voxell.Util;

public class EntryPoint : MonoBehaviour
{
    [SerializeField, SceneRef] private string m_Menu;
    [SerializeField, SceneRef] private string m_Maze;
}
