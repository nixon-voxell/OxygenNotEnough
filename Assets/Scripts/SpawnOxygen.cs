using UnityEngine;

public class SpawnOxygen : MonoBehaviour
{
    private GameManager gameManager;
    private int width;
    private int height;

    private void Start()
    {
        this.SpawnCube(1);
    }

    public void SpawnCube(int times)
    {
        width = Random.Range(0, GameManager.Instance.MazeGenerator.Width);
        height = Random.Range(0, GameManager.Instance.MazeGenerator.Height);

        for (int i = 0; i < times; i++)
        {
            GameManager.Instance.MazeGenerator.PlaceObject(
                this.transform, width, height
            );
        }
    }
}
