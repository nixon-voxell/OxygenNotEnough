using System.Collections;
using UnityEngine;

public class OxygenSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private float m_SpawnInterval;
    [SerializeField] private ObjectPool<OxygenTank> m_OxygenTankPool;

    private void Start()
    {
        GameManager.Instance.OxygenSpawner = this;
        this.m_OxygenTankPool.Initialize(this.transform, false);
    }

    public void Spawn()
    {
        // spawn tanks
        this.StartCoroutine(this.SpawnOxygenTanks());
    }

    public void Despawn()
    {
        for (int e = 0; e < this.m_OxygenTankPool.Count; e++)
        {
            OxygenTank oxygenTank = this.m_OxygenTankPool.GetNextObject();
            oxygenTank.SpawnOut();
        }
    }

    public IEnumerator SpawnOxygenTanks()
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;

        for (int o = 0; o < this.m_OxygenTankPool.Count; o++)
        {
            OxygenTank oxygenTank = this.m_OxygenTankPool.GetNextObject();
            Transform trans = oxygenTank.transform;

            trans.position = mazeGenerator.GetRandomWorldPosition();
            trans.rotation = Quaternion.identity;
            // start spawn in animation
            oxygenTank.SpawnIn();

            yield return new WaitForSeconds(this.m_SpawnInterval);
        }
    }
}
