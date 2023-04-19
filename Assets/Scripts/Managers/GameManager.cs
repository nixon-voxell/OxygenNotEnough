using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public enum GameState
{
    Idle,
    InProgress,
    Win,
    Lose,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private Volume m_GlobalVolume;
    [SerializeField] private float m_VigAnimaSpeed;
    [SerializeField] private Player m_PlayerPrefab;

    public MazeGenerator MazeGenerator;
    public OxygenSpawner OxygenSpawner;
    public EnemySpawner EnemySpawner;

    private Vignette m_Vignette;
    private float m_TargetVigIntensity;
    private GameState m_GameState;
    private Player m_Player;

    public GameState GameState => this.m_GameState;

    public Camera MainCamera => this.m_MainCamera;
    public Volume GlobalVolume => this.m_GlobalVolume;
    public Vignette Vignette => this.m_Vignette;

    // game world actors
    public Player Player => this.m_Player;

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

        // instantiate player
        this.m_Player = Object.Instantiate(
            this.m_PlayerPrefab,
            this.MazeGenerator.GridToWorldPosition(0, 0), Quaternion.identity
        );

        // TODO: remove this
        this.StartGame();

        // default to idle (main menu)
        this.m_GameState = GameState.Idle;
        if (!this.m_GlobalVolume.profile.TryGet<Vignette>(out this.m_Vignette))
        {
            Debug.LogWarning("Vignette post-process override not found");
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

    public void StartGame()
    {
        this.m_GameState = GameState.InProgress;

        this.MazeGenerator.GenerateMaze();
        this.EnemySpawner.Spawn();
        this.OxygenSpawner.Spawn();
        this.Player.SpawnIn();
    }

    public void EndGame()
    {
        this.m_GameState = GameState.Idle;

        this.MazeGenerator.HideAll();
        this.EnemySpawner.Despawn();
        this.OxygenSpawner.Despawn();
        this.Player.SpawnOut();
    }

    public void Win()
    {
        this.m_GameState = GameState.Win;
    }

    public void Lose()
    {
        this.m_GameState = GameState.Lose;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
