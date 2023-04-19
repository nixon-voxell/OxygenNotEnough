using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private float m_SpawnInterval;
    [SerializeField] private ObjectPool<Enemy> m_EnemyPool;

    private void Start()
    {
        GameManager.Instance.EnemySpawner = this;
        this.m_EnemyPool.Initialize(this.transform, false);
    }

    public void Spawn()
    {
        // spawn enemies
        this.StartCoroutine(this.SpawnEnemies());
    }

    public void Despawn()
    {
        for (int e = 0; e < this.m_EnemyPool.Count; e++)
        {
            Enemy enemy = this.m_EnemyPool.GetNextObject();
            enemy.SpawnOut();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;

        for (int e = 0; e < this.m_EnemyPool.Count; e++)
        {
            Enemy enemy = this.m_EnemyPool.GetNextObject();
            Transform trans = enemy.transform;

            trans.position = mazeGenerator.GetRandomWorldPosition();
            trans.rotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
            // start spawn in animation
            enemy.SpawnIn();

            yield return new WaitForSeconds(this.m_SpawnInterval);
        }
    }
}
