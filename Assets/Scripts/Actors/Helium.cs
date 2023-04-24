using System.Collections;
using UnityEngine;

public class Helium : MonoBehaviour, IActor
{
    [SerializeField] private float m_AnimSpeed;
    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private ParticleSystem[] m_ParticleSystems;

    private Vector3 m_OriginScale;
    private Material m_mat_Balloon;

    private void Awake()
    {
        this.m_OriginScale = this.transform.localScale;
        this.m_mat_Balloon = this.m_Renderer.material;
    }

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

    private IEnumerator DetonateSequence()
    {
        yield return null;

        for (int p = 0; p < this.m_ParticleSystems.Length; p++)
        {
            this.m_ParticleSystems[p].Play();
        }
    }

    public void SpawnOut()
    {
        this.StopAllCoroutines();
        Object.Destroy(this);
    }
}
