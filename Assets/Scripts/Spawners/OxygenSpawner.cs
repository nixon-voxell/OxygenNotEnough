using System.Collections;
using UnityEngine;

public class OxygenSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private int m_TankNumber;
    [SerializeField] private OxygenTank m_OxygenTankPrefab;
    [SerializeField] private float m_SpawnInterval;

    private void Start()
    {
        this.Spawn();
    }

    public void Spawn()
    {
        this.StartCoroutine(this.SpawnOxygenTank(this.m_TankNumber));
    }

    public IEnumerator SpawnOxygenTank(int times)
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;

        for (int i = 0; i < times; i++)
        {
            OxygenTank oxygenTank = Object.Instantiate(
                this.m_OxygenTankPrefab,
                mazeGenerator.GetRandomWorldPosition(), Quaternion.identity,
                this.transform
            );

            yield return new WaitForSeconds(this.m_SpawnInterval);
        }
    }
}
