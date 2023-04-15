using UnityEngine;
using Unity.AI.Navigation;

public class EnemyPathAI : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] m_NavMeshSurfaces;

    private void Start()
    {
        
    }

    public void BakeAllSurface()
    {
        for (int n = 0; n < this.m_NavMeshSurfaces.Length; n++)
        {
            this.m_NavMeshSurfaces[n].BuildNavMesh();
        }
    }
}
