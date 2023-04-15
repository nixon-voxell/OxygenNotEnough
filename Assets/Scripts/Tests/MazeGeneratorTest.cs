using UnityEngine;

public class MazeGeneratorTest : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    [SerializeField] private Vector2Int m_GridLocation;

    private void Update()
    {
        this.m_MazeGenerator.PlaceObject(this.transform, this.m_GridLocation.x, this.m_GridLocation.y);
    }
}
