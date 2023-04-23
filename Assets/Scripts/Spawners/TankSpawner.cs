using System.Collections;
using UnityEngine;

public class TankSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private SpawnerData<Tank>[] m_SpawnerData;
    // clear if number is this object's instance ID, else, occupied
    private int[,] m_MazeOccupancy;

    public void Spawn()
    {
        // snapshot maze width height
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;
        this.m_MazeOccupancy = new int[mazeGenerator.Width, mazeGenerator.Height];
        // clear occupancy
        for (int x = 0; x < mazeGenerator.Width; x++)
        {
            for (int y = 0; y < mazeGenerator.Height; y++)
            {
                this.m_MazeOccupancy[x, y] = this.GetInstanceID();
            }
        }

        // spawn tanks
        for (int s = 0; s < this.m_SpawnerData.Length; s++)
        {
            this.StartCoroutine(this.SpawnTanks(this.m_SpawnerData[s]));
        }
    }

    public void Despawn()
    {
        for (int s = 0; s < this.m_SpawnerData.Length; s++)
        {
            // spawn out all items in the pool
            SpawnerData<Tank> spawnerData = this.m_SpawnerData[s];
            for (int e = 0; e < spawnerData.Pool.Count; e++)
            {
                Tank tank = spawnerData.Pool.GetNextObject();
                tank.SpawnOut();
            }
        }
    }

    public IEnumerator SpawnTanks(SpawnerData<Tank> spawnerData)
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;

        for (int o = 0; o < spawnerData.Pool.Count; o++)
        {
            Tank tank = spawnerData.Pool.GetNextObject();

            // start spawn in animation
            tank.SpawnIn();

            yield return new WaitForSeconds(spawnerData.SpawnInterval);
        }
    }

    public void Occupy(int x, int y, Object obj)
    {
        this.m_MazeOccupancy[x, y] = obj.GetInstanceID();
    }

    public void Free(int x, int y, Object obj)
    {
        // free only if the space is being occupied by the object itself
        int objInstanceID = obj.GetInstanceID();
        if (this.m_MazeOccupancy[x, y] == objInstanceID)
        {
            this.m_MazeOccupancy[x, y] = this.GetInstanceID();
        }
    }

    public bool IsOccupied(int x, int y)
    {
        int width = this.m_MazeOccupancy.GetLength(0) - 1;
        int height = this.m_MazeOccupancy.GetLength(1) - 1;
        bool occupied = this.m_MazeOccupancy[x, y] != this.GetInstanceID();
        // start location
        occupied = occupied && (x != 0 && y != 0);
        // exit location
        occupied = occupied && (x != width && y != height);

        return occupied;
    }

    private void Start()
    {
        GameManager.Instance.TankSpawner = this;
        // initialize tanks
        for (int s = 0; s < this.m_SpawnerData.Length; s++)
        {
            this.m_SpawnerData[s].Pool.Initialize(this.transform, false);
        }
    }
}
