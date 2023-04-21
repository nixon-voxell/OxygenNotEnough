using System;
using System.Collections;
using UnityEngine;

public static class AnimUtil
{
    public static IEnumerator MoveY(Transform trans, float startY, float targetY, float animSpeed, Action endAction = null)
    {
        Vector3 position = trans.position;
        position.y = startY;

        while (position.y < targetY - 0.01f)
        {
            position.y = Mathf.Lerp(position.y, targetY, Time.deltaTime * animSpeed);
            trans.position = position;
            yield return new WaitForEndOfFrame();
        }

        endAction?.Invoke();
    }

    public static IEnumerator ScaleTo(Transform trans, Vector3 targetScale, float animSpeed, Action endAction = null)
    {
        Vector3 scale = trans.localScale;

        while (scale != targetScale)
        {
            trans.localScale = Vector3.Lerp(trans.localScale, targetScale, Time.deltaTime * animSpeed);
            yield return new WaitForEndOfFrame();
        }

        endAction?.Invoke();
    }
}
