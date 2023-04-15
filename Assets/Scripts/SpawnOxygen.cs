using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOxygen : MonoBehaviour
{
    public GameObject cubePrefab;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        SpawnCube(1);
    }

    public void SpawnCube(int times)
    {
        int width = gameManager.MazeGenerator.Width;
        int height = gameManager.MazeGenerator.Height;
        for (int i = 0; i <times;i++)
        {
            Vector3 randomSpawnpos = new Vector3(Random.Range(-width,height+1),1,Random.Range(-width,height+1));
            Instantiate(cubePrefab, randomSpawnpos,Quaternion.identity);
        }
    }
}
