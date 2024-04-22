using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*
 ===================
 制作：髙橋
 概要：Movieシーンの里アイコンの挙動スクリプト
 ===================
 */

public class FadeIcon : MonoBehaviour
{
    [SerializeField, Header("遅延時間(秒)")] private float DelayTime = 2.0f;
    [SerializeField, Header("フェード時間(秒)")] private float FadeTime = 2.0f;
    [SerializeField, Header("フェード後のアルファ値")] private float FadeAlpha = 0.0f;
    [SerializeField, Header("フェード前のアイコン")] private Image preFadeIcon;
    [SerializeField, Header("フェード後のアイコン")] private Image postFadeIcon;

    private void Start()
    {
        //- フェード前のアイコンを表示
        preFadeIcon.gameObject.SetActive(true);
        //- フェード後のアイコンを非表示にする
        postFadeIcon.gameObject.SetActive(false);
        //- フェード前のアイコンを初期状態にする
        preFadeIcon.canvasRenderer.SetAlpha(1.0f);

        //=== フェード前のアイコンを徐々にフェードアウトさせる ===
        preFadeIcon.DOFade(FadeAlpha, FadeTime) // 指定したAlphaの値になるまでのフェード時間
        .SetDelay(DelayTime) // フェードが始まるまでの遅延時間
        .OnComplete(() =>
        {
            // === フェードが完了したら、フェード後のアイコンを表示してフェードインさせる ===
            //- フェードが完了した時点で、フェード前のアイコンはフェードアウトが終わっているため、非表示に
            preFadeIcon.gameObject.SetActive(false);
            //- フェード後のアイコンはフェードインさせる前なので、表示
            postFadeIcon.gameObject.SetActive(true);
            //- フェード後のアイコンのアルファ値を0に設定
            postFadeIcon.canvasRenderer.SetAlpha(0.0f);
            //- フェード後のアイコンを指定された時間でフェードインさせる
            postFadeIcon.CrossFadeAlpha(1.0f, FadeTime, false);
        });
    }
}
