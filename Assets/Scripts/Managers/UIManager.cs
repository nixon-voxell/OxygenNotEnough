using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private OxygenUI m_OxygenUI;

    public OxygenUI OxygenUI => this.m_OxygenUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("There is probably more than one instance.", Instance);
            Object.Destroy(this);
        }
    }
}
