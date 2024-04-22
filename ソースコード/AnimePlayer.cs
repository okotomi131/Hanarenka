using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/*
 ===================
 制作：髙橋
 概要：里内のプレイヤー挙動
 ===================
*/
public class AnimePlayer : MonoBehaviour
{
    [SerializeField, Header("スケール変更オブジェクト")] private GameObject scaleObj;
    [SerializeField, Header("遅延時間(秒)")] private float delayTime;
    [SerializeField, Header("縮んでいく時間(秒)")] private float shrinkTime;

    public void SetAnime()
    {
        //=== プレイヤーを徐々に縮めていくアニメーション ===
        scaleObj.transform.DOScale(Vector3.zero, shrinkTime) // スケールが0になるまでの縮小アニメーション時間
        .SetDelay(delayTime) // アニメーションが発生するまでの遅延時間
        .OnComplete(() =>
        {
            //- 縮小アニメーションが終了したらオブジェクトを非表示にする
            scaleObj.SetActive(false);
        });
    }
}