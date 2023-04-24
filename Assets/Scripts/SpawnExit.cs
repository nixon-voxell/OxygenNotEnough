using UnityEngine;

public class SpawnExit : MonoBehaviour
{
    void Start()
    {
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;

        mazeGenerator.PlaceObject(this.transform, mazeGenerator.Width, mazeGenerator.Height);
        // mazeGenerator.SetSize(this.transform, 0.25f, 1.0f, 1.5f);
        this.transform.localScale = new Vector3(0.25f, 1.0f, 1.5f);
    }
}
