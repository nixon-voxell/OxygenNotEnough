using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public OxygenUI OxygenUI;
    public HeliumUI HeliumUI;

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
