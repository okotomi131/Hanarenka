using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChargeColorCracker : MonoBehaviour
{
    [SerializeField, Header("変化させるゲームオブジェクト")]
    private GameObject obj;
    [SerializeField, Header("取得するマテリアル番号")]
    private int MatNum = 0;
    [SerializeField, Header("通常時のカラー")]
    private Color DefaultColor;
    [SerializeField, Header("チャージ中のカラー")]
    private Color ChargeColor;
    [SerializeField, Header("色のフェード時間(秒)")]
    private float FadeTime = 0.5f;
    [SerializeField, Header("チャージ中の色の時間(秒)")]
    private float ChargeTime = 0.5f;

    void Start()
    {
        //- マテリアルの取得
        Material material = obj.GetComponent<Renderer>().materials[MatNum];
        //- 通常時の色に変更
        material.DOColor(DefaultColor, 0.0f);
        //- チャージ色のフェード処理
        material.DOColor(ChargeColor, FadeTime);
        //- 通常色に戻す
        //material.DOColor(DefaultColor, FadeTime).SetDelay(FadeTime + ChargeTime);
    }
}