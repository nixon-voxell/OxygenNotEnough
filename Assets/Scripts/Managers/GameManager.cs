using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Voxell.Util;

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

    [InspectOnly] public MazeGenerator MazeGenerator;
    [InspectOnly] public EnemySpawner EnemySpawner;
    [InspectOnly] public TankSpawner TankSpawner;
    // [InspectOnly] public OxygenSpawner OxygenSpawner;
    // [InspectOnly] public HeliumSpawner HeliumSpawner;

    private Vignette m_Vignette;
    private float m_CurrVigIntensity;
    private float m_TargetVigIntensity;
    private GameState m_GameState;
    private Player m_Player;

    public GameState GameState => this.m_GameState;

    public Camera MainCamera => this.m_MainCamera;
    public Volume GlobalVolume => this.m_GlobalVolume;
    public Vignette Vignette => this.m_Vignette;

    // game world actors
    public Player Player => this.m_Player;

    public void SetVignetteIntensity(float intensity)
    {
        this.m_TargetVigIntensity = intensity;
    }

    public void StartGame()
    {
        this.m_GameState = GameState.InProgress;

        if (this.m_Player == null)
        {
            // instantiate player
            this.m_Player = Object.Instantiate(this.m_PlayerPrefab);
            // move player to Maze scene
            SceneManager.MoveGameObjectToScene(
                this.m_Player.gameObject, this.MazeGenerator.gameObject.scene
            );
        }

        this.MazeGenerator.GenerateMaze();
        this.EnemySpawner.Spawn();
        this.TankSpawner.Spawn();
        this.Player.SpawnIn();
    }

    public void EndGame()
    {
        this.m_GameState = GameState.Idle;

        this.MazeGenerator.HideAll();
        this.EnemySpawner.Despawn();
        this.TankSpawner.Despawn();
        this.Player.SpawnOut();
    }

    public void Win()
    {
        this.m_GameState = GameState.Win;
        SoundEffect.Instance.Win();

        // show win ui
    }

    public void Lose()
    {
        this.m_GameState = GameState.Lose;
        SoundEffect.Instance.Lose();
        
        // show lose ui
        this.EndGame();
        this.StartGame();
    }

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

        // find vignette volume profile
        if (!this.m_GlobalVolume.profile.TryGet<Vignette>(out this.m_Vignette))
        {
            Debug.LogWarning("Vignette post-process override not found");
        } else
        {
            this.m_CurrVigIntensity = this.m_Vignette.intensity.value;
            this.m_TargetVigIntensity = this.m_CurrVigIntensity;
        }

        // default to idle (main menu)
        this.m_GameState = GameState.Idle;
    }

    private void Update()
    {
        if (this.m_GameState == GameState.InProgress)
        {
            // simple lerp animation (fast when difference is big, slow when difference is small)
            this.m_CurrVigIntensity = math.lerp(this.m_CurrVigIntensity, this.m_TargetVigIntensity, Time.deltaTime  * this.m_VigAnimaSpeed);

            this.Vignette.intensity.value = this.m_CurrVigIntensity;
        } else
        {
            // hard code to 0.1f, find this value not bad
            this.Vignette.intensity.value = 0.1f;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
