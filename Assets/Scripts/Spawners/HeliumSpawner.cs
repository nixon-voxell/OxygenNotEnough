using System.Collections;
using UnityEngine;

public class HeliumSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private int m_TankNumber;
    [SerializeField] private OxygenTank m_HeliumTankPrefab;
    [SerializeField] private float m_SpawnInterval;

    private void Start()
    {
        GameManager.Instance.HeliumSpawner = this;
    }

    public void Spawn()
    {
        this.StartCoroutine(this.SpawnHeliumTank(this.m_TankNumber));
    }

    public void Despawn()
    {
        
    }

    public IEnumerator SpawnHeliumTank(int times)
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;

        for (int i = 0; i < times; i++)
        {
            OxygenTank heliumTank = Object.Instantiate(
                this.m_HeliumTankPrefab,
                mazeGenerator.GetRandomWorldPosition(), Quaternion.identity,
                this.transform
            );

            yield return new WaitForSeconds(this.m_SpawnInterval);
        }
    }
}
