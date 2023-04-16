using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_Agent;
    private bool m_HasTarget;

    public bool HasTarget => this.m_HasTarget;

    private void Update()
    {
        this.MoveToTarget(GameManager.Instance.Player.transform.position);
    }

    public void MoveToTarget(Vector3 target)
    {
        this.m_Agent.SetDestination(target);
        this.m_HasTarget = true;
    }

    public void RemoveTarget()
    {
        this.m_Agent.ResetPath();
        this.m_HasTarget = false;
    }
}
