using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Camera m_MainCamera;

    public Camera MainCamera => this.m_MainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogWarning("There is probably more than one instance.", Instance);
            Object .Destroy(this);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
