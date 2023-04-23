using UnityEngine;

[System.Serializable]
public struct SpawnerData<T> where T : MonoBehaviour, IActor
{
    public float SpawnInterval;
    public ObjectPool<T> Pool;
}
