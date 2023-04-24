using System.Collections;
using UnityEngine;

public class Helium : MonoBehaviour, IActor
{
    [SerializeField] private float m_AnimSpeed;
    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private ParticleSystem[] m_ParticleSystems;

    [Header("Buildup")]
    [SerializeField] private float m_ColorMultiplierStart;
    [SerializeField] private float m_ColorMultiplierEnd;
    [SerializeField] private float m_SpeedStart;
    [SerializeField] private float m_SpeedEnd;
    [SerializeField] private float m_NoiseStart;
    [SerializeField] private float m_NoiseEnd;
    [SerializeField] private float m_BuildupDuration;

    [Header("Stunt")]
    [SerializeField] private float m_StuntDuration;
    [SerializeField] private float m_StuntRadius;

    private Vector3 m_OriginScale;
    private Material m_mat_Balloon;
    private Material m_mat_Ribbons;

    [ContextMenu("SpawnIn")]
    public void SpawnIn()
    {
        this.StartCoroutine(
            AnimUtil.ScaleUp(
                this.transform,
                Vector3.zero, this.m_OriginScale,
                this.m_AnimSpeed,
                () =>
                {
                    this.StartCoroutine(this.DetonateSequence());
                }
            )
        );
    }

    public void SpawnOut()
    {
        this.StopAllCoroutines();
        Object.Destroy(this.gameObject);
    }

    private IEnumerator DetonateSequence()
    {
        float timeAccum = 0.0f;

        while (timeAccum < this.m_BuildupDuration)
        {
            float percentage = timeAccum / this.m_BuildupDuration;
            this.m_mat_Balloon.SetFloat(
                "_ColorMultiplier", Mathf.Lerp(
                    this.m_ColorMultiplierStart, this.m_ColorMultiplierEnd, percentage
                )
            );
            this.m_mat_Balloon.SetFloat(
                "_Speed", Mathf.Lerp(
                    this.m_SpeedStart, this.m_SpeedEnd, percentage
                )
            );
            this.m_mat_Ribbons.SetFloat(
                "_NoiseScale", Mathf.Lerp(
                    this.m_NoiseStart, this.m_NoiseEnd, percentage
                )
            );
            timeAccum += Time.deltaTime;
            yield return null;
        }

        // make balloon disappear
        this.m_Renderer.enabled = false;

        for (int p = 0; p < this.m_ParticleSystems.Length; p++)
        {
            this.m_ParticleSystems[p].Play();
        }

        timeAccum = 0.0f;
        while (timeAccum < this.m_StuntDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(
                this.transform.position, this.m_StuntRadius
            );

            for (int c = 0; c < colliders.Length; c++)
            {
                Collider collider = colliders[c];
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    enemy.Stunt();
                }
            }

            timeAccum += Time.deltaTime;
            yield return null;
        }

        this.SpawnOut();
    }

    private void Awake()
    {
        this.m_OriginScale = this.transform.localScale;
        Material[] materials = this.m_Renderer.materials;
        this.m_mat_Balloon = materials[0];
        this.m_mat_Ribbons = materials[1];
    }
}
