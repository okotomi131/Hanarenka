/*
 ===================
 基盤制作：大川
 クリア時のテキストアニメーションを行うスクリプト
 ===================
 */
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TMPAnime : MonoBehaviour
{
    private enum E_TEXTCOLOR
    {
        Clear,  //無色透明
        Black,  //黒
        Blue,   //青
        Cyan,   //シアン
        Gray,   //灰色
        Green,  //緑
        Magenta,//マゼンタ
        Red,    //赤
        White,  //白
        Yellow  //黄色
    }

    [SerializeField, Header("アニメーションさせるテキスト")]       private TextMeshProUGUI TMP;
    [SerializeField, Header("テキスト初期カラー")]                private E_TEXTCOLOR textcolor = E_TEXTCOLOR.Black;
    [SerializeField, Header("テキストアニメカラー")]              private E_TEXTCOLOR textAnicolor = E_TEXTCOLOR.Black;
    [SerializeField, Header("回転秒")]                           private float RotateTime = 0.0f;
    [SerializeField, Header("波の高さ")]                         private float WaveTop = 0.0f;
    [SerializeField, Header("波移動完了までの時間")]              private float WaveTime = 0.0f;
    [SerializeField, Header("文字フェード完了までの時間")]         private float FadeTime = 0.0f;
    [SerializeField, Header("カラー遅延時間")]                    private float DelayColor = 0.0f;
    [SerializeField, Header("ループ遅延時間")]                    private float DelayLoop = 0.0f;

    private float EaseTime = 2.0f;
    private bool bPermissionClearSE = false;    //クリア音の再生が許可されているか
    private bool first = false;
    private Vector3 initialScale;               //初期サイズ

    private void OnEnable()
    {
        TMP.DOFade(0f, 0f);
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        StartCoroutine(AnimationCoroutine());
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        TMP.DOFade(0f, 0f);
    }

    private void OnSceneUnloaded(Scene scene)
    {    DOTween.KillAll(); }

    IEnumerator AnimationCoroutine()
    {
        DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(TMP);
        //- クリア音再生
        if (bPermissionClearSE)
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Clear);
        else
            bPermissionClearSE = true;

        while (tmpAnimator.textInfo.characterCount == 0)
        {
            yield return null;
        }
        for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
        {
            FirstAnime(tmpAnimator, i);
        }
        while (true)
        {
            yield return new WaitForSeconds(DelayLoop);
            for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
            {   LoopAnime(tmpAnimator, i);  }
        }

    }

    private void FirstAnime(DOTweenTMPAnimator tmpAnimator, int i)
    { 
        //- 初めのテキストを90度回転させておく
        tmpAnimator.DORotateChar(i, Vector3.up * 90, 0);
        //- 指定された文字に対してアニメーションを設定する
        Vector3 currCharOffset = tmpAnimator.GetCharOffset(i);
        DOTween.Sequence()
            .Append(tmpAnimator.DORotateChar(i, Vector3.zero, RotateTime))
            .Append(tmpAnimator.DOOffsetChar(i, currCharOffset + new Vector3(0, WaveTop, 0), WaveTime).SetEase(Ease.OutFlash, EaseTime))
            .Join(tmpAnimator.DOFadeChar(i, 1, FadeTime))
            .AppendInterval(DelayColor)
            .Append(tmpAnimator.DOColorChar(i, GetColor(textAnicolor), 0.2f).SetLoops(2, LoopType.Yoyo))
            .SetDelay(0.05f * i)
            .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)    
            .SetLink(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpAnimator"></param>
    /// <param name="i"></param>
    private void LoopAnime(DOTweenTMPAnimator tmpAnimator, int i)
    {
        //- 指定された文字に対してアニメーションを設定する
        Vector3 currCharOffset = tmpAnimator.GetCharOffset(i);
        DOTween.Sequence()
            .Append(tmpAnimator.DOOffsetChar(i, currCharOffset + new Vector3(0, WaveTop, 0), WaveTime).SetEase(Ease.OutFlash, EaseTime))
            .Append(tmpAnimator.DOFadeChar(i, 1, FadeTime))
            .AppendInterval(DelayColor)
            .Append(tmpAnimator.DOColorChar(i, GetColor(textAnicolor), 0.2f).SetLoops(2, LoopType.Yoyo))
            .SetDelay(0.05f * i)
            .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
            .SetLink(gameObject);

    }

    /// <summary>
    /// テキストの色を取得する
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private Color GetColor(E_TEXTCOLOR color)
    {
        switch (color)
        {
            case E_TEXTCOLOR.Clear:     return new Color(1f, 1f, 1f, 0f);
            case E_TEXTCOLOR.Black:     return Color.black;
            case E_TEXTCOLOR.Blue:      return Color.blue;
            case E_TEXTCOLOR.Cyan:      return Color.cyan;
            case E_TEXTCOLOR.Gray:      return Color.gray;
            case E_TEXTCOLOR.Green:     return Color.green;
            case E_TEXTCOLOR.Magenta:   return Color.magenta;
            case E_TEXTCOLOR.Red:       return Color.red;
            case E_TEXTCOLOR.White:     return Color.white;
            case E_TEXTCOLOR.Yellow:    return Color.yellow;
            default:                    return Color.black;
        }
    }


}
