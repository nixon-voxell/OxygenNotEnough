using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Spawn Animation")]
    [SerializeField] private float m_SpawnStartY;
    [SerializeField] private float m_SpawnEndY;
    [SerializeField] private float m_SpawnAnimSpeed;

    [Header("Agent")]
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

    public float PauseDuration => this.m_PauseDuration;

    private void Start()
    {
        Debug.Log("Start");
        this.m_PursuitTime = 0.0f;
        this.m_PursuitEndReset = false;

        this.m_Agent.enabled = false;
        Vector3 position = this.transform.position;
        position.y = this.m_SpawnStartY;
        this.transform.position = position;
        this.StartCoroutine(AnimUtil.FloatUp(
            this.transform, this.m_SpawnEndY, this.m_SpawnAnimSpeed,
            () =>
            {
                this.m_Agent.enabled = true;
            }
        ));
    }

    private void Update()
    {
        if (this.m_Agent.enabled == false) return;

        Transform selfTrans = this.transform;

        // if in pursuit of enemy, set target destination to player location
        if (this.m_PursuitTime > 0.0f)
        {
            Player player = GameManager.Instance.Player;
            this.MoveToTarget(player.transform.position);

            this.m_PursuitTime -= Time.deltaTime;
        } else
        {
            // if not pursuing and not reset yet, reset target
            if (this.m_PursuitEndReset == false)
            {
                this.ResetTarget();
            }

            // check if player is in front of the enemy
            RaycastHit hit;
            if (Physics.SphereCast(
                selfTrans.position, 0.3f, selfTrans.forward, out hit, this.m_VisualRange
            ) && hit.collider.CompareTag("Player")) {
                this.PursuitPlayer();
            // choose a random position when we reach the target position
            } else
            {
                Vector2 selfVec2 = new Vector2(selfTrans.position.x, selfTrans.position.z);
                Vector2 targetVec2 = new Vector2(this.m_Target.x, this.m_Target.z);
                if (Vector2.SqrMagnitude(selfVec2 - targetVec2) < 0.5f)
                {
                    this.RestartFindRandLocRoutine();
                }
            }
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
        Debug.Log("Pursuit Player");
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
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;
        this.m_Target = mazeGenerator.GetRandomWorldPosition();

        yield return new WaitForSeconds(this.m_PauseDuration);

        this.m_Agent.SetDestination(this.m_Target);

        // set coroutine to null when it ends
        this.m_FindRandLocRoutine = null;
    }
}