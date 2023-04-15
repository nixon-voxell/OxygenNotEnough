using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

public class Test : MonoBehaviour
{
    public int Width;
    public int Height;
    public uint Seed;

    public GameObject TilePrefab;
    public GameObject WallPrefab;

    public GameObject[] TilePool;
    public GameObject[] WallPool;

    private void Start()
    {
        int cellCount = this.Width * this.Height;
        int wallCount = (this.Width - 1) * this.Height + this.Width * (this.Height - 1);

        this.TilePool = new GameObject[cellCount];
        this.WallPool = new GameObject[wallCount];

        for (int t = 0; t < this.TilePool.Length; t++)
        {
            this.TilePool[t] = Object.Instantiate(this.TilePrefab, this.transform);
        }

        for (int w = 0; w < this.WallPool.Length; w++)
        {
            this.WallPool[w] = Object.Instantiate(this.WallPrefab, this.transform);
        }

        this.HideAll();
        this.GenerateMaze();
    }

    public void GenerateMaze()
    {
        int cellCount = this.Width * this.Height;
        int wallCount = (this.Width - 1) * this.Height + this.Width * (this.Height - 1);
        int verticalWallStartIdx = (this.Width - 1) * this.Height;

        // clear memory to make sure that all initial data is false
        NativeArray<bool> na_cellStates = new NativeArray<bool>(cellCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);
        NativeArray<bool> na_wallStates = new NativeArray<bool>(wallCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);

        GenerateMazeJob generateMazeJob = new GenerateMazeJob
        {
            Width = this.Width,
            Height = this.Height,
            StartCell = int2.zero,
            Seed = math.max(1, this.Seed),

            na_CellStates = na_cellStates,
            na_WallStates = na_wallStates,
        };

        JobHandle jobHandle = generateMazeJob.Schedule();
        jobHandle.Complete();

        int tilePoolIdx = 0;

        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                // int flattenIdx = MazeUtil.FlattenIndex(x, y, this.Width);
                // this.Tiles[flattenIdx] =
                Vector3 position = new Vector3(x, 0.0f, y);
                Quaternion rotation = Quaternion.identity;

                GameObject tile = this.TilePool[tilePoolIdx++];
                tile.SetActive(true);
                Transform tileTransform = tile.transform;
                tileTransform.position = position;
                tileTransform.rotation =  rotation;
            }
        }

        int wallPoolIdx = 0;

        // horizontal walls
        for (int x = 0; x < this.Width - 1; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                int wallIdx = x + y * this.Width;

                if (na_wallStates[wallIdx] == false)
                {
                    Vector3 position = new Vector3(x + 0.5f, 1.0f, y);
                    Quaternion rotation = Quaternion.identity;

                    GameObject wall = this.WallPool[wallPoolIdx++];
                    wall.SetActive(true);
                    Transform wallTransform = wall.transform;
                    wallTransform.position = position;
                    wallTransform.rotation = rotation;
                }
            }
        }

        // vertical walls
        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height - 1; y++)
            {
                int wallIdx = x + y * this.Width;
                wallIdx += verticalWallStartIdx;

                if (na_wallStates[wallIdx] == false)
                {
                    Vector3 position = new Vector3(x, 1.0f, y + 0.5f);
                    Quaternion rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);

                    GameObject wall = this.WallPool[wallPoolIdx++];
                    wall.SetActive(true);
                    Transform wallTransform = wall.transform;
                    wallTransform.position = position;
                    wallTransform.rotation = rotation;
                }
            }
        }

        na_cellStates.Dispose();
        na_wallStates.Dispose();
        this.Seed = (uint)UnityEngine.Random.Range(0, 1000000);
    }

    /// <summary>Hide all tiles and walls.</summary>
    public void HideAll()
    {
        for (int t = 0; t < this.TilePool.Length; t++)
        {
            this.TilePool[t].SetActive(false);
        }

        for (int w = 0; w < this.WallPool.Length; w++)
        {
            this.WallPool[w].SetActive(false);
        }
    }
}
