using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

/*
 ===================
 制作：髙橋
 追記：牧野、大川
 概要：クリック時シーン遷移するスクリプト
 ===================
 */
public class BackScene : MonoBehaviour
{
    [SerializeField, Header("オプション操作にかかる時間(秒)")]
    private float OptionTime = 1.0f;
    [SerializeField, Header("シーン移動時のフェード時間(秒)")]
    private float FadeTime   = 1.0f;
    [SerializeField, Header("シーン移動時のBGM切り替え時間(秒)")]
    private float disBGMTime = 1.0f;
    [SerializeField, Header("戻る先のシーンを設定")]
    private SceneObject backScene;

    public static bool Input = false;   //入力判定

    //- スクリプト用の変数
    BGMManager bgmManager;
    //- イメージのコンポーネント
    Image imageInGame;

    //- ボタンが押されているかどうか
    bool bIsPushBack = false;

    //- ボタンが押された時間
    float bPushTimeBack = 0;

    //- シーン移動が始まったかどうかのフラグ
    bool bIsStartInGame = false;

    private void Start()
    {
        //- コンポーネントの取得
        bgmManager  = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        Input = false;
    }

    private void Update()
    {
        //- シーン移動が始まったかどうかのフラグを取得
        bool bIsMoveScene = false;

        //- シーン移動のフラグ更新 
        if (bIsStartInGame) bIsMoveScene = true;

        //＝＝＝＝＝　戻るボタン　＝＝＝＝＝
        //- 「シーン移動ボタンが押されてる」かつ「シーン移動が始まっていない」時
        if (bIsPushBack && !bIsMoveScene)
        {
            if (bIsStartInGame == true) return; // リセット開始フラグがたっていればリターン
            bIsStartInGame = true; // シーン開始フラグをたてる
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click); // クリック音再生
            GameObject.Find("ColorFadeImage").GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime); // フェード開始
            DOVirtual.DelayedCall(disBGMTime, () => bgmManager.DestroyBGMManager());  // シーンを変える前にBGMを消す
            DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(backScene)); // シーンのロード(遅延あり)
        }
    }

    /// <summary>
    /// コントローラーを取得する関数
    /// </summary>
    /// <param name="context"></param>
    public void OnInBack(InputAction.CallbackContext context)
    {
        //- 選択ボタンの入力が行われたら処理を行わない
        if (SelectButton.Input)
        { return; }

        //- ボタンが押されている間、変数を設定
        if (context.started)
        {
            bIsPushBack = true;
            Input = true;       // 入力フラグ変更
        }
        if (context.canceled) { bIsPushBack = false; }
    }

    /// <summary>
    /// ポップ時にタイトルに戻る処理
    /// </summary>
    public void VillagePoPToTitle()
    {
        if (bIsStartInGame == true) return; // リセット開始フラグがたっていればリターン
        bIsStartInGame = true; // シーン開始フラグをたてる
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click); // クリック音再生
        GameObject.Find("ColorFadeImage").GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime); // フェード開始
        DOVirtual.DelayedCall(disBGMTime, () => bgmManager.DestroyBGMManager());  // シーンを変える前にBGMを消す
        DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(backScene)); // シーンのロード(遅延あり)
    }
}