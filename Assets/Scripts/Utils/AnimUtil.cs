using System;
using System.Collections;
using UnityEngine;

public static class AnimUtil
{
    public static IEnumerator MoveUp(Transform trans, float startY, float targetY, float animSpeed, Action endAction = null)
    {
        Vector3 position = trans.position;
        position.y = startY;

        while (position.y < targetY - 0.01f)
        {
            position.y = Mathf.Lerp(position.y, targetY, Time.deltaTime * animSpeed);
            trans.position = position;
            yield return null;
        }

        endAction?.Invoke();
    }

    public static IEnumerator ScaleDown(Transform trans, Vector3 startScale, Vector3 targetScale, float animSpeed, Action endAction = null)
    {
        Vector3 scale = trans.localScale;
        scale = startScale;

        while (
            scale.x > targetScale.x + 0.01f ||
            scale.y > targetScale.y + 0.01f ||
            scale.z > targetScale.z + 0.01f
        ) {
            scale = Vector3.Lerp(scale, targetScale, Time.deltaTime * animSpeed);
            trans.localScale = scale;
            yield return null;
        }

        endAction?.Invoke();
    }

    public static IEnumerator ScaleUp(Transform trans, Vector3 startScale, Vector3 targetScale, float animSpeed, Action endAction = null)
    {
        Vector3 scale = trans.localScale;
        scale = startScale;

        while (
            scale.x < targetScale.x - 0.01f ||
            scale.y < targetScale.y - 0.01f ||
            scale.z < targetScale.z - 0.01f
        ) {
            scale = Vector3.Lerp(scale, targetScale, Time.deltaTime * animSpeed);
            trans.localScale = scale;
            yield return null;
        }

        endAction?.Invoke();
    }
}
