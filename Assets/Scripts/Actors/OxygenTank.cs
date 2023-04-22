using UnityEngine;

public class OxygenTank : MonoBehaviour, IActor
{
    [SerializeField] private float m_SpawnStartY;
    [SerializeField] private float m_SpawnEndY;
    [SerializeField] private float m_SpawnAnimSpeed;
    [SerializeField] private float m_OxygenAmount;
    [SerializeField] private BoxCollider m_BoxCollider;

    private Vector3 m_OriginScale;

    public float OxygenAmount => this.m_OxygenAmount;

    public void SpawnIn()
    {
        this.gameObject.SetActive(true);

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
                    Transform trans = this.transform;

                    trans.localScale = this.m_OriginScale;
                    trans.position = GameManager.Instance.MazeGenerator.GetRandomWorldPosition();
                    trans.rotation = Quaternion.identity;
                    this.SpawnIn();
                }
            )
        );
    }

    private void Start()
    {
        this.m_OriginScale = this.transform.localScale;
    }
}
