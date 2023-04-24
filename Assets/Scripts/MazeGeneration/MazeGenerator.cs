using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int m_Width;
    [SerializeField] private int m_Height;
    [SerializeField] private uint m_Seed = 1;

    [SerializeField] private GameObject m_TilePrefab;
    [SerializeField] private GameObject m_WallPrefab;
    [SerializeField] private GameObject m_SurroundWallPrefab;

    [SerializeField, Range(0.0f, 1.0f)] private float m_WallPercentage;

    private GameObject m_Tile;
    private GameObject[] m_WallPool;
    private GameObject[] m_SurroundWallPool;

    public int Width => this.m_Width;
    public int Height => this.m_Height;
    public Vector3 TileOffset => new Vector3(-this.m_Width + 0.5f, 0.0f, -this.m_Height);

    private void Start()
    {
        GameManager.Instance.MazeGenerator = this;
    }

    public void GetRandomGridPosition(out int x, out int y)
    {
        x = UnityEngine.Random.Range(0, this.Width);
        y = UnityEngine.Random.Range(0, this.Height);
    }

    public Vector3 GridToWorldPosition(int x, int y)
    {
        Vector3 position = new Vector3(x * 2 + 1, 0.5f, y * 2 + 1);
        position += this.TileOffset;
        return position;
    }

    public Vector3 GetRandomWorldPosition()
    {
        int x, y;
        this.GetRandomGridPosition(out x, out y);

        return this.GridToWorldPosition(x, y);
    }

    public void PlaceObject(Transform trans, int x, int y)
    {
        Vector3 position = this.GridToWorldPosition(x, y);
        trans.position = position;
    }

    public void GenerateMaze()
    {
        int cellCount = this.m_Width * this.m_Height;
        int wallCount = (this.m_Width - 1) * this.m_Height + this.m_Width * (this.m_Height - 1);

        this.m_Tile = Object.Instantiate(this.m_TilePrefab, this.transform);

        const int SURROUND_WALL_COUNT = 4;
        this.m_SurroundWallPool = new GameObject[SURROUND_WALL_COUNT];
        for (int w = 0; w < SURROUND_WALL_COUNT; w++)
        {
            this.m_SurroundWallPool[w] = Object.Instantiate(this.m_SurroundWallPrefab);
        }

        this.m_WallPool = new GameObject[wallCount];

        for (int w = 0; w < this.m_WallPool.Length; w++)
        {
            this.m_WallPool[w] = Object.Instantiate(this.m_WallPrefab, this.transform);
        }

        this.HideAll();

        int verticalWallStartIdx = (this.m_Width - 1) * this.m_Height;

        // clear memory to make sure that all initial data is false
        NativeArray<bool> na_cellStates = new NativeArray<bool>(cellCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);
        NativeArray<bool> na_wallStates = new NativeArray<bool>(wallCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);

        GenerateMazeJob generateMazeJob = new GenerateMazeJob
        {
            Width = this.m_Width,
            Height = this.m_Height,
            StartCell = int2.zero,
            Seed = math.max(1, this.m_Seed),

            na_CellStates = na_cellStates,
            na_WallStates = na_wallStates,
        };

        JobHandle jobHandle = generateMazeJob.Schedule();
        jobHandle.Complete();

        int wallPoolIdx = 0;

        // horizontal walls
        for (int x = 0; x < this.m_Width - 1; x++)
        {
            for (int y = 0; y < this.m_Height; y++)
            {
                int wallIdx = x + y * this.m_Width;

                if (na_wallStates[wallIdx] == false)
                {
                    Vector3 localPosition = new Vector3(x * 2.0f + 1.0f, 1.0f, y * 2.0f);
                    localPosition += new Vector3(1.0f, 0.0f, 1.0f);
                    Quaternion rotation = Quaternion.identity;

                    float probability = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (probability >= this.m_WallPercentage) continue;

                    GameObject wall = this.m_WallPool[wallPoolIdx++];
                    wall.SetActive(true);
                    Transform wallTransform = wall.transform;
                    wallTransform.localPosition = localPosition;
                    wallTransform.rotation = rotation;
                }
            }
        }

        // vertical walls
        for (int x = 0; x < this.m_Width; x++)
        {
            for (int y = 0; y < this.m_Height - 1; y++)
            {
                int wallIdx = x + y * this.m_Width;
                wallIdx += verticalWallStartIdx;

                if (na_wallStates[wallIdx] == false)
                {
                    Vector3 localPosition = new Vector3(x * 2.0f, 1.0f, y * 2.0f + 1.0f);
                    localPosition += new Vector3(1.0f, 0.0f, 1.0f);
                    Quaternion rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);

                    float probability = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (probability >= this.m_WallPercentage) continue;

                    GameObject wall = this.m_WallPool[wallPoolIdx++];
                    wall.SetActive(true);
                    Transform wallTransform = wall.transform;
                    wallTransform.position = localPosition;
                    wallTransform.rotation = rotation;
                }
            }
        }

        // resize tile to size of maze
        this.m_Tile.transform.localPosition = -this.TileOffset;
        this.m_Tile.transform.localScale = new Vector3(this.Width * 2 + 1, 1.0f, this.Height * 2 + 1);
        this.m_Tile.SetActive(true);

        // resize surround wall to surround maze
        const float WALL_WIDTH = 2.0f;
        const float WALL_HEIGHT = 3.0f;
        // left
        GameObject surroundWall = this.m_SurroundWallPool[0];
        surroundWall.transform.localPosition = new Vector3(-this.Width - WALL_WIDTH * 0.5f - 0.5f, 0.0f, 0.0f);
        surroundWall.transform.localScale = new Vector3(WALL_WIDTH, WALL_HEIGHT, this.Width * 3.0f);
        surroundWall.SetActive(true);
        // right
        surroundWall = this.m_SurroundWallPool[1];
        surroundWall.transform.localPosition = new Vector3(this.Width + WALL_WIDTH * 0.5f + 0.5f, 0.0f, 0.0f);
        surroundWall.transform.localScale = new Vector3(WALL_WIDTH, WALL_HEIGHT, this.Width * 3.0f);
        surroundWall.SetActive(true);
        // top
        surroundWall = this.m_SurroundWallPool[2];
        surroundWall.transform.localPosition = new Vector3(0.0f, 0.0f, this.Height + WALL_WIDTH * 0.5f + 0.5f);
        surroundWall.transform.localScale = new Vector3(this.Width * 3.0f, WALL_HEIGHT, WALL_WIDTH);
        surroundWall.SetActive(true);
        // down
        surroundWall = this.m_SurroundWallPool[3];
        surroundWall.transform.localPosition = new Vector3(0.0f, 0.0f, -this.Height - WALL_WIDTH * 0.5f - 0.5f);
        surroundWall.transform.localScale = new Vector3(this.Width * 3.0f, WALL_HEIGHT, WALL_WIDTH);
        surroundWall.SetActive(true);

        NavMeshSurface surface = this.m_Tile.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        na_cellStates.Dispose();
        na_wallStates.Dispose();

        // center the maze
        this.transform.position = new Vector3(-this.m_Width + 0.5f, 0.0f, -this.m_Height);
        // change seed to a different number
        this.m_Seed = (uint)UnityEngine.Random.Range(0, 1000000);
    }

    /// <summary>Hide all tiles and walls.</summary>
    public void HideAll()
    {
        this.ResetTransform(this.transform);
        this.m_Tile.SetActive(false);

        for (int w = 0; w < this.m_WallPool.Length; w++)
        {
            this.ResetTransform(this.m_WallPool[w].transform);
            this.m_WallPool[w].SetActive(false);
        }

        for (int w = 0; w < this.m_SurroundWallPool.Length; w++)
        {
            this.ResetTransform(this.m_SurroundWallPool[w].transform);
            this.m_SurroundWallPool[w].SetActive(false);
        }
    }

    private void ResetTransform(Transform trans)
    {
        trans.position = Vector3.zero;
        trans.rotation = Quaternion.identity;
    }
}
