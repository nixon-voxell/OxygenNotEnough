using UnityEngine;
using Voxell.Util;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas[] m_Canvases;

    [InspectOnly] public OxygenUI OxygenUI;
    [InspectOnly] public HeliumUI HeliumUI;

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

    private void Start()
    {
        for (int c = 0; c < this.m_Canvases.Length; c++)
        {
            this.m_Canvases[c].worldCamera = GameManager.Instance.MainCamera;
        }
    }
}
