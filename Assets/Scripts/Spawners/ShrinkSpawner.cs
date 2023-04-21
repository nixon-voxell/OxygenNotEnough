using System;
using System.Collections;
using UnityEngine;

public class ShrinkSpawner : MonoBehaviour
{

    [SerializeField] private float m_ShrinkSpeed;

    private void Start()
    {
        this.Shrink();
    }

    public void Shrink()
    {
        this.StartCoroutine(this.ScaleOxygenTank(transform, Vector3.zero, m_ShrinkSpeed));
    }

    public IEnumerator ScaleOxygenTank(Transform trans, Vector3 targetScale, float animSpeed, Action endAction = null)
    {
        Vector3 scale = transform.localScale;

        while (scale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        endAction?.Invoke();
    }
}

