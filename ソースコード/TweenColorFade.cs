using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TweenColorFade{

    public static Tweener FadeOut(this Image image, float duration)
    {
        return image.DOFade(0.0F, duration);
    }

    public static Tweener FadeIn(this Image image, float duration)
    {
        return image.DOFade(1.0F, duration);
    }
}
