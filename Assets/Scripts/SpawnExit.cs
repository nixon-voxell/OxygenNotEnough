using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExit : MonoBehaviour
{
    [SerializeField] private MazeGenerator m_MazeGenerator;
    // Start is called before the first frame update
    void Start()
    {
        int width = GameManager.Instance.MazeGenerator.Width;
        int height = GameManager.Instance.MazeGenerator.Height;

        this.m_MazeGenerator.PlaceObject(this.transform, width,height-1);
        this.m_MazeGenerator.SetSize(this.transform, 0.25f,1.0f,1.5f);

    }
}
