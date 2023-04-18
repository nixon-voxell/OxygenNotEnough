using UnityEngine;

public class MazeGeneratorTest : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    [SerializeField] private Vector2Int m_GridLocation;
    // [SerializeField] private int m_GridLocation.x = Random.Range(0,width+1),;
    // [SerializeField] private int m_GridLocation.y = Random.Range(0,height+1);


    private void Update()
    {
        this.m_MazeGenerator.PlaceObject(this.transform, this.m_GridLocation.x, this.m_GridLocation.y);
        // this.m_MazeGenerator.PlaceObject(this.transform, Random.Range(0,11), Random.Range(0,11));

    }
}
