using UnityEngine;

public class OxygenTank : MonoBehaviour, IActor
{
    [SerializeField] private float m_SpawnStartY;
    [SerializeField] private float m_SpawnEndY;
    [SerializeField] private float m_SpawnAnimSpeed;
    [SerializeField] private float m_OxygenAmount;

    public float OxygenAmount => this.m_OxygenAmount;

    public void SpawnIn()
    {
        Transform selfTrans = this.transform;

        this.StartCoroutine(AnimUtil.MoveY(
            selfTrans, this.m_SpawnStartY, this.m_SpawnEndY, this.m_SpawnAnimSpeed
        ));
    }

    public void SpawnOut()
    {
        this.gameObject.SetActive(false);
    }
}
