using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private Volume m_GlobalVolume;

    private Vignette m_Vignette;

    public Camera MainCamera => this.m_MainCamera;
    public Volume GlobalVolume => this.m_GlobalVolume;
    public Vignette Vignette => this.m_Vignette;

    private void Awake()
    {
        this.m_GlobalVolume.profile.TryGet<Vignette>(out this.m_Vignette);
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogWarning("There is probably more than one instance.", Instance);
            Object .Destroy(this);
        }
    }

    public void SetVignetteIntensity(float intensity)
    {
        this.Vignette.intensity.value = intensity;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
