using UnityEngine;
using Voxell.Util;

public class Tank : MonoBehaviour, IActor
{
    // maze coordinate
    [SerializeField, InspectOnly] private int m_Gridx, m_Gridy;

    [SerializeField] private float m_SpawnStartY;
    [SerializeField] private float m_SpawnEndY;
    [SerializeField] private float m_SpawnAnimSpeed;
    [SerializeField] private int m_Amount;
    [SerializeField] private BoxCollider m_BoxCollider;

    private Vector3 m_OriginScale;

    public int Amount => this.m_Amount;

    public void SpawnIn()
    {
        this.gameObject.SetActive(true);
        // spawn into a random position
        Transform trans = this.transform;
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;
        TankSpawner tankSpawner = GameManager.Instance.TankSpawner;

        // reset scale to original size
        trans.localScale = this.m_OriginScale;

        // free up previous location
        tankSpawner.Free(this.m_Gridx, this.m_Gridy, this);

        // prevent spawning on occupied location
        do {
            mazeGenerator.GetRandomGridPosition(out this.m_Gridx, out this.m_Gridy);
        } while (tankSpawner.IsOccupied(this.m_Gridx, this.m_Gridy));

        // set location as occupied
        tankSpawner.Occupy(this.m_Gridx, this.m_Gridy, this);

        trans.position = GameManager.Instance.MazeGenerator.GetRandomWorldPosition();
        trans.rotation = Quaternion.identity;

        this.StartCoroutine(AnimUtil.MoveUp(
            this.transform, this.m_SpawnStartY, this.m_SpawnEndY, this.m_SpawnAnimSpeed
        ));
    }

    public void SpawnOut()
    {
        this.gameObject.SetActive(false);
    }

    public void SwitchLocation()
    {
        // prevent 2 collisions at once
        this.m_BoxCollider.enabled = false;

        this.StartCoroutine(
            AnimUtil.ScaleDown(
                this.transform, this.m_OriginScale, Vector3.zero, this.m_SpawnAnimSpeed,
                () =>
                {
                    this.m_BoxCollider.enabled = true;
                    this.SpawnIn();
                }
            )
        );
    }

    private void Awake()
    {
        this.m_OriginScale = this.transform.localScale;
    }
}
