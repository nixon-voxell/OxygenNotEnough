using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private Volume m_GlobalVolume;
    [SerializeField] private float m_VigAnimaSpeed;
    [SerializeField] private MazeGenerator m_MazeGenerator;

    private Vignette m_Vignette;
    private float m_TargetVigIntensity;

    public Camera MainCamera => this.m_MainCamera;
    public Volume GlobalVolume => this.m_GlobalVolume;
    public Vignette Vignette => this.m_Vignette;
    public MazeGenerator MazeGenerator => this.m_MazeGenerator;

    private void Awake()
    {
        this.m_GlobalVolume.profile.TryGet<Vignette>(out this.m_Vignette);
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("There is probably more than one instance.", Instance);
            Object.Destroy(this);
        }
    }

    public void Update()
    {
        float vigIntensity = this.Vignette.intensity.value;

        // simple lerp animation (fast when difference is big, slow when difference is small)
        vigIntensity = math.lerp(vigIntensity, this.m_TargetVigIntensity, Time.deltaTime  * this.m_VigAnimaSpeed);

        this.Vignette.intensity.value = vigIntensity;
    }

    public void SetVignetteIntensity(float intensity)
    {
        this.m_TargetVigIntensity = intensity;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
