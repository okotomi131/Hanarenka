using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JoyFace : MonoBehaviour
{
    [SerializeField, Header("表示時間(秒)")]
    private float Time = 1.5f;

    [SerializeField, Header("透明度が一番低い時のアルファ値(1.0～0.0)")]
    private float PeakAlpha = 0.7f;

    void Start()
    {
        //- マテリアルの取得
        Material material = GetComponent<Renderer>().material;
        //- マテリアルを透明にする
        material.color = new Color32(255, 255, 255, 0);

        //=== アニメーション ===
        //- 少しづつ大きくなる
        transform.DOScale(new Vector3(4, 4, 1), Time).SetEase(Ease.OutQuint);
        //- にじみ出てくる
        material.DOFade(PeakAlpha, Time / 2);
        //- 消える
        material.DOFade(0.0F, Time / 2).SetDelay(Time / 2);
    }
}
