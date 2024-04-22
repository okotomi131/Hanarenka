using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrinkAnime : MonoBehaviour
{
    [SerializeField, Header("•âŠ®ƒ^ƒCƒv")]
    private Ease EaseType;
    [SerializeField, Header("ŠgkŒã‚ÌƒXƒP[ƒ‹")]
    private Vector3 AfterScale;
    [SerializeField, Header("ŠgkŠÔ(•b)")]
    private float ShrinkTime;
    [SerializeField, Header("’x‰„ŠÔ(•b)")]
    private float DelayTime;

    void Start()
    {
        transform.DOScale(AfterScale, ShrinkTime).SetEase(EaseType).SetDelay(DelayTime);
    }
}
