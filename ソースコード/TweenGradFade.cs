using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TweenGradFade{

    [HideInInspector]
    static public float currentTime = 0;

    static private int progress = Shader.PropertyToID("_Progress");

    public static Tweener FadeOut(this Material material, float duration)
    {
        Tweener tweener = DOTween.To(() => currentTime, (x) => currentTime = x, 0.0f, duration);
        return tweener;
    }

    public static Tweener FadeIn(this Material material, float duration)
    {
        Tweener tweener = DOTween.To(() => currentTime, (x) => currentTime = x, 1.0f, duration);
        return tweener;
    }
}
