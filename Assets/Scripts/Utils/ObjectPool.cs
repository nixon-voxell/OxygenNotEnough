using UnityEngine;

[System.Serializable]
public class ObjectPool<T> where T : MonoBehaviour
{
    [SerializeField] private T m_Original;
    [SerializeField] private int m_Count;
    private T[] m_Pool;

    public int Count => this.m_Count;

    private int m_CurrIdx;

    public void Initialize(Transform parent, bool active)
    {
#if UNITY_EDITOR
        Debug.Assert(this.m_Original != null, "Original object cannot be null.");
#endif
        this.m_Pool = new T[this.Count];
        for (int i = 0; i < this.Count; i++)
        {
            this.m_Pool[i] = Object.Instantiate(this.m_Original, Vector3.zero, Quaternion.identity, parent);
            this.m_Pool[i].gameObject.SetActive(active);
        }
    }

    public void Initialize()
    {
#if UNITY_EDITOR
        Debug.Assert(this.m_Original != null, "Original object cannot be null.");
#endif
        this.m_Pool = new T[this.Count];
        for (int i = 0; i < this.Count; i++)
        {
            this.m_Pool[i] = Object.Instantiate(this.m_Original, Vector3.zero, Quaternion.identity);
        }
    }

    public T GetNextObject()
    {
        T nextObj = this.m_Pool[this.m_CurrIdx];
        this.m_CurrIdx = this.GetNextIdx();
        return nextObj;
    }

    private int GetNextIdx()
    {
        return (this.m_CurrIdx + 1) % this.m_Count;
    }
}
