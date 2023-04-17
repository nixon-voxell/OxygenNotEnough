using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_Agent;
    [Tooltip("Number of seconds that the enemy pause before going to another random location.")]
    [SerializeField] private float m_PauseDuration;
    [Tooltip("Number of seconds that the enemy pursue the player before giving up.")]
    [SerializeField] private float m_PursuitDuration;
    [SerializeField] private float m_VisualRange;

    private float m_PursuitTime;
    private bool m_PursuitEndReset;
    private Vector3 m_Target;
    private Coroutine m_FindRandLocRoutine;

    private void Start()
    {
        this.m_PursuitTime = 0.0f;
        this.m_PursuitEndReset = false;
    }

    private void Update()
    {
        Transform selfTrans = this.transform;

        // if in pursuit of enemy, set target destination to player location
        if (this.m_PursuitTime > 0.0f)
        {
            Player player = GameManager.Instance.Player;
            this.MoveToTarget(player.transform.position);

            this.m_PursuitTime -= Time.deltaTime;
        } else if (this.m_PursuitEndReset == false) // if not pursuing and not reset yet, reset target
        {
            this.ResetTarget();
        }

        // check if player is infront of the enemy
        RaycastHit hit;
        if (Physics.SphereCast(
            selfTrans.position, 0.3f, selfTrans.forward, out hit, this.m_VisualRange
        )) {
            if (hit.collider.CompareTag("Player"))
            {
                this.PursuitPlayer();
            }
        }

        // choose a random position when we reach the target position
        if (Vector3.SqrMagnitude(this.transform.position - this.m_Target) < 0.1f)
        {
            this.RestartFindRandLocRoutine();
        }
    }

    public void MoveToTarget(Vector3 target)
    {
        this.m_Target = target;
        this.m_Agent.SetDestination(target);
    }

    public void ResetTarget()
    {
        this.m_Target = this.transform.position;
        this.m_Agent.ResetPath();
        this.m_PursuitEndReset = true;
    }

    public void PursuitPlayer()
    {
        this.EndFindRandLocRoutine();
        this.m_PursuitTime = this.m_PursuitDuration;
        this.m_PursuitEndReset = false;
    }

    private void EndFindRandLocRoutine()
    {
        if (this.m_FindRandLocRoutine != null)
        {
            this.StopCoroutine(this.m_FindRandLocRoutine);
            this.m_FindRandLocRoutine = null;
        }
    }

    private void RestartFindRandLocRoutine()
    {
        this.EndFindRandLocRoutine();
        this.m_FindRandLocRoutine = this.StartCoroutine(this.FindRandomLocation());
    }

    private IEnumerator FindRandomLocation()
    {
        yield return new WaitForSeconds(this.m_PauseDuration);

        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;
        this.m_Target = mazeGenerator.GetRandomWorldPosition();
        this.m_Agent.SetDestination(this.m_Target);

        // set coroutine to null when it ends
        this.m_FindRandLocRoutine = null;
    }
}
