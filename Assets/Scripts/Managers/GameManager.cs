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
    [SerializeField] private MazeGenerator m_MazeGenerator;
    [SerializeField] private Player m_PlayerPrefab;

    [Header("Enemy")]
    [SerializeField] private Enemy m_EnemyPrefab;
    [SerializeField] private int m_EnemyCount;

    [SerializeField] private SpawnOxygen m_SpawnOxygen;

    private Vignette m_Vignette;
    private float m_TargetVigIntensity;
    private GameState m_GameState;
    private Player m_Player;
    private Enemy[] m_Enemies;

    public Camera MainCamera => this.m_MainCamera;
    public Volume GlobalVolume => this.m_GlobalVolume;
    public Vignette Vignette => this.m_Vignette;
    public MazeGenerator MazeGenerator => this.m_MazeGenerator;
    public GameState GameState => this.m_GameState;
    public Player Player => this.m_Player;
    public SpawnOxygen SpawnOxygen => this.m_SpawnOxygen;

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

        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        this.MazeGenerator.GenerateMaze();

        // spawn player
        this.m_Player = Object.Instantiate(this.m_PlayerPrefab);

        this.MazeGenerator.PlaceObject(this.m_Player.transform, 0, 0);
        Vector3 playerPos = this.m_Player.transform.position;
        playerPos.y = 0.5f;
        this.m_Player.transform.position = playerPos;

        // spawn enemy
        this.m_Enemies = new Enemy[this.m_EnemyCount];
        for (int e = 0; e < this.m_EnemyCount; e++)
        {
            this.m_Enemies[e] = Object.Instantiate(this.m_EnemyPrefab);

            int x, y;
            this.MazeGenerator.GetRandomGridPosition(out x, out y);
            this.MazeGenerator.PlaceObject(this.m_Enemies[e].gameObject.transform, x, y);
            Vector3 enemyPos = this.m_Enemies[e].transform.position;
            this.m_Enemies[e].MoveToTarget(enemyPos);
        }

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

    private void OnDestroy()
    {
        Instance = null;
    }

    public void Restart()
    {
        this.m_GameState = GameState.Lose;
        SceneManager.LoadScene(1);
    }
    public void Won(){
        this.m_GameState = GameState.Win;
        SceneManager.LoadScene(0);
    }
}
