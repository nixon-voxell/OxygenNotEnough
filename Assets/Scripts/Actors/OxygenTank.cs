using UnityEngine;

public class OxygenTank : MonoBehaviour
{
    [SerializeField] private float m_SpawnStartY;
    [SerializeField] private float m_SpawnEndY;
    [SerializeField] private float m_SpawnAnimSpeed;

    private void Start()
    {
        Transform selfTrans = this.transform;
        Vector3 position = selfTrans.position;
        position.y = this.m_SpawnStartY;
        selfTrans.position = position;

        this.StartCoroutine(AnimUtil.FloatUp(
            selfTrans, this.m_SpawnEndY, this.m_SpawnAnimSpeed
        ));
    }
}
