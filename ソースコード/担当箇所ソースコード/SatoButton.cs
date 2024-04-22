/*
 ===================
 基盤制作：大川
 追記者：髙橋
 ボタンを選択した際に動作するスクリプト
 ===================
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//- ボタン選択時に動作するクラス
public class SatoButton : MonoBehaviour
{
    [SerializeField, Header("シーン遷移先")] private SceneObject NextScene;          //シーン遷移先
    [SerializeField, Header("フェード開始遅延時間")] private float DelayTime;         //フェードが呼び出されるまでの遅延時間
    [SerializeField, Header("フェードオブジェクト")] private GameObject fadeObject;   //フェード用オブジェクト
    [SerializeField, Header("フェード完了までの時間")] private float FadeTime;        //フェード完了時間

    private BGMManager  bgmManager;
    private Button      button;
    private ButtonAnime buttonAnime;
    private AnimePlayer animePlayer;
    private bool Load        = false;  // 多重ロード抑止
    public static bool Input = false;  // 入力判定
    void Awake()
    {
        buttonAnime = GetComponent<ButtonAnime>();
        button      = GetComponent<Button>();
        bgmManager  = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        Input       = false;
    }
    
    /// <summary>
    /// シーン遷移を行う処理
    /// 里選択
    /// </summary>
    public void MoveSatoScene()
    {
        //- 他の入力が行われていたら処理しない
        if (BackScene.Input)
        { return; }
        //- 呼び出し済なら行わない
        if (Load)
        { return; }
        //- 呼び出し済にする
        Load = true;

        //- 入力があったらフラグ変更
        Input = true;
        //- 呼び出されたら上下左右選択を無効化
        Navigation NoneNavigation = button.navigation;
        NoneNavigation.selectOnUp = null;
        NoneNavigation.selectOnDown = null;
        NoneNavigation.selectOnLeft = null;
        NoneNavigation.selectOnRight = null;
        button.navigation = NoneNavigation;

        //- ボタンの入力を受け付けない
        button.interactable = false;

        //- 演出の描画フラグをリセット
        if (CutIn.MoveCompleat) { CutIn.ResetMoveComplete(); }
        if (BoardMove.MoveComplete) { BoardMove.ResetMoveComplete(); }
        if (OpeningAnime.MoveCompleat) { OpeningAnime.ResetMoveComplete(); }

        //- クリック音再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);

        //- プレイヤーアニメーションを再生する
        animePlayer = GetComponent<AnimePlayer>();
        animePlayer.SetAnime();

        //- 遅延後の処理
        DOVirtual.DelayedCall(DelayTime, () => fadeObject.GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime));
        DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager()).SetDelay(DelayTime);
        DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(NextScene)).SetDelay(DelayTime);
    }
}