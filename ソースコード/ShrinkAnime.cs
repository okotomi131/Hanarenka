using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShrinkAnime : MonoBehaviour
{
    [SerializeField, Header("�⊮�^�C�v")]
    private Ease EaseType;
    [SerializeField, Header("�g�k��̃X�P�[��")]
    private Vector3 AfterScale;
    [SerializeField, Header("�g�k����(�b)")]
    private float ShrinkTime;
    [SerializeField, Header("�x������(�b)")]
    private float DelayTime;

    void Start()
    {
        transform.DOScale(AfterScale, ShrinkTime).SetEase(EaseType).SetDelay(DelayTime);
    }
}
