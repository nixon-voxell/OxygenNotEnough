using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOxygen : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private MazeGenerator m_MazeGenerator;
    private int width;
    private int height;

    public void SpawnCube(int times)
    {
        width = Random.Range(0,GameManager.Instance.MazeGenerator.Width);
        height = Random.Range(0,GameManager.Instance.MazeGenerator.Height);
        for (int i = 0; i <times;i++)
        {
            this.m_MazeGenerator.PlaceObject(this.transform, width,height);
        }
    }
}
