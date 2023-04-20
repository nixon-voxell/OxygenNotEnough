using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private OxygenUI m_OxygenUI;
    [SerializeField] private HeliumUI m_HeliumUI;
    public OxygenUI OxygenUI => this.m_OxygenUI;
    public HeliumUI HeliumUI => this.m_HeliumUI;

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
