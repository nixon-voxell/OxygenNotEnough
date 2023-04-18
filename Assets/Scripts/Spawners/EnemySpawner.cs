using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private Enemy m_EnemyPrefab;
    [SerializeField] private int m_EnemyCount;

    private Enemy[] m_Enemies;

    private void Start()
    {
        this.Spawn();
    }

    public void Spawn()
    {
        // spawn enemies
        this.StartCoroutine(this.SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;
        this.m_Enemies = new Enemy[this.m_EnemyCount];

        for (int e = 0; e < this.m_EnemyCount; e++)
        {
            Enemy enemy = Object.Instantiate(
                this.m_EnemyPrefab,
                mazeGenerator.GetRandomWorldPosition(),
                Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f),
                this.transform
            );

            enemy.ResetTarget();
            this.m_Enemies[e] = enemy;

            yield return new WaitForSeconds(enemy.PauseDuration);
        }
    }
}
