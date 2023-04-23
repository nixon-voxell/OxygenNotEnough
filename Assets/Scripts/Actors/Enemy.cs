using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Voxell.Util;

public class Enemy : MonoBehaviour, IActor
{
    [Header("Spawn Animation")]
    [SerializeField] private float m_SpawnStartY;
    [SerializeField] private float m_SpawnEndY;
    [SerializeField] private float m_SpawnAnimSpeed;
    [SerializeField] private MeshRenderer m_Renderer;

    [Header("Agent")]
    [SerializeField] private NavMeshAgent m_Agent;
    [Tooltip("Number of seconds that the enemy pause before going to another random location.")]
    [SerializeField] private float m_PauseDuration;
    [Tooltip("Number of seconds that the enemy pursue the player before giving up.")]
    [SerializeField] private float m_PursuitDuration;
    [SerializeField] private float m_VisualRange;
    [SerializeField] private float m_VisualRadius;

    [SerializeField, InspectOnly] private float m_PursuitTime;
    private bool m_PursuitEndReset = true;
    private Vector3 m_Target;
    private Coroutine m_FindRandLocRoutine;
    [SerializeField, InspectOnly] private Material m_BodyMat;

    public float PauseDuration => this.m_PauseDuration;

    public void SpawnIn()
    {
        this.gameObject.SetActive(true);
        this.m_PursuitTime = 0.0f;
        this.m_PursuitEndReset = false;
        this.m_Agent.enabled = false;

        this.StartCoroutine(AnimUtil.MoveUp(
            this.transform, this.m_SpawnStartY, this.m_SpawnEndY, this.m_SpawnAnimSpeed,
            () =>
            {
                this.m_Agent.enabled = true;
                this.ResetTarget();
            }
        ));
    }

    public void SpawnOut()
    {
        this.ResetTarget();
        this.m_Agent.enabled = false;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.m_Agent.enabled == false) return;

        Transform selfTrans = this.transform;

        // check if player is in front of the enemy
        RaycastHit hit;
        if (Physics.SphereCast(
                selfTrans.position,
                this.m_VisualRadius,
                selfTrans.forward,
                out hit,
                this.m_VisualRange
            ) && hit.collider.CompareTag("Player")
        ) {
            this.PursuitPlayer();
        }

        // if in pursuit of enemy, set target destination to player location
        if (this.m_PursuitTime > 0.0f)
        {
            this.AngryMaterial();
            Player player = GameManager.Instance.Player;
            this.MoveToTarget(player.transform.position);

            this.m_PursuitTime -= Time.deltaTime;
        } else
        {
            this.NormalMaterial();
            // if not pursuing and not reset yet, reset target
            if (this.m_PursuitEndReset == false)
            {
                this.ResetTarget();
            }

            // choose a random position when we reach the target position
            Vector2 selfVec2 = new Vector2(selfTrans.position.x, selfTrans.position.z);
            Vector2 targetVec2 = new Vector2(this.m_Target.x, this.m_Target.z);
            if (Vector2.SqrMagnitude(selfVec2 - targetVec2) < 0.5f)
            {
                this.RestartFindRandLocRoutine();
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Transform selfTrans = this.transform;

        RaycastHit hit;
        bool successHit = Physics.SphereCast(
            selfTrans.position, this.m_VisualRadius, selfTrans.forward, out hit, this.m_VisualRange
        );

        Gizmos.color = Color.red;
        if (successHit)
        {
            Gizmos.DrawLine(selfTrans.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.3f);
        }
    }
#endif

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
        MazeGenerator mazeGenerator = GameManager.Instance.MazeGenerator;
        this.m_Target = mazeGenerator.GetRandomWorldPosition();

        yield return new WaitForSeconds(this.m_PauseDuration);

        this.m_Agent.SetDestination(this.m_Target);

        // set coroutine to null when it ends
        this.m_FindRandLocRoutine = null;
    }

    private void NormalMaterial()
    {
        this.m_BodyMat.EnableKeyword("_STATE_NORMAL");
        this.m_BodyMat.DisableKeyword("_STATE_ANGRY");
        this.m_BodyMat.DisableKeyword("_STATE_AFRAID");
    }

    private void AngryMaterial()
    {
        this.m_BodyMat.DisableKeyword("_STATE_NORMAL");
        this.m_BodyMat.EnableKeyword("_STATE_ANGRY");
        this.m_BodyMat.DisableKeyword("_STATE_AFRAID");
    }

    private void AfraidMaterial()
    {
        this.m_BodyMat.DisableKeyword("_STATE_NORMAL");
        this.m_BodyMat.DisableKeyword("_STATE_ANGRY");
        this.m_BodyMat.EnableKeyword("_STATE_AFRAID");
    }

    private void Awake()
    {
        this.m_BodyMat = this.m_Renderer.materials[0];
    }
}
