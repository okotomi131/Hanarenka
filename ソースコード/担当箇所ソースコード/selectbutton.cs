/*
 ===================
 基盤制作：大川
 追記者：井上・牧野・辻・髙橋・寺前
 ボタンを選択した際に動作するスクリプト
 ===================
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

//- ボタン選択時に動作するクラス
public class SelectButton : MonoBehaviour
{
    [SerializeField, Header("シーン遷移先")] private SceneObject NextScene;           //シーン遷移先
    [SerializeField, Header("フェード開始遅延時間")] private float DelayTime;         //フェードが呼び出されるまでの遅延時間
    [SerializeField, Header("フェードオブジェクト")] private GameObject fadeObject;   //フェード用オブジェクト
    [SerializeField, Header("フェード完了までの時間")] private float FadeTime;        //フェード完了時間
    
    private BGMManager bgmManager;
    private Button button;
    private ButtonAnime buttonAnime;
    private SelectMovePlayer SelectPlayer;
    private bool Load = false;                      //多重ロード抑止
    public static bool Input = false;            //入力判定
    void Awake()
    {
        buttonAnime = GetComponent<ButtonAnime>();
        button = GetComponent<Button>();
        bgmManager  = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        Input = false;
        //- タイトルシーンでは戻るボタンがないので入力状態を初期化する
        if(SceneManager.GetActiveScene().name == "0_TitleScene")
        {   BackScene.Input = false;    }
    }
    
    /// <summary>
    /// シーン遷移を行う処理
    /// タイトル・里選択・インゲーム
    /// </summary>
    public void MoveScene()
    {
        //- 他の入力が行われていたら処理しない
        if(BackScene.Input)
        {
            Debug.Log("BackScene.Input" + BackScene.Input);
            return;
        }

        //- 呼び出し済なら処理しない
        if (Load)
        { return; }
        

        //- 呼び出し済にする
        Load = true;
        //- 入力があったらフラグ変更
        Input = true;

        //- ボタンアニメが存在したら処理
        if(buttonAnime)
        {
            //- フラグ変更をボタンアニメに送信する
            buttonAnime.SetbSelect(true);
        }
        
        //- 呼び出されたら上下左右選択を無効化
        Navigation NoneNavigation = button.navigation;
        NoneNavigation.selectOnUp = null;
        NoneNavigation.selectOnDown = null;
        NoneNavigation.selectOnLeft = null;
        NoneNavigation.selectOnRight = null;
        button.navigation = NoneNavigation;

        //- ボタンの入力を受け付けない
        button.interactable = false;

        //- ボタンアニメが存在していたら処理
        if (buttonAnime)
        { 
            //- ボタン入力アニメーション
            buttonAnime.PushButtonAnime();
        }

        //- クリック音再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);

        //- 演出の描画フラグをリセット
        if (CutIn.MoveCompleat) { CutIn.ResetMoveComplete(); }
        if (BoardMove.MoveComplete) { BoardMove.ResetMoveComplete(); }
        if (OpeningAnime.MoveCompleat) { OpeningAnime.ResetMoveComplete(); }
        
        //- 遅延後の処理
        DOVirtual.DelayedCall(DelayTime, ()=> fadeObject.GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime));
        DOVirtual.DelayedCall (FadeTime, ()=> bgmManager.DestroyBGMManager()).SetDelay(DelayTime); 
        DOVirtual.DelayedCall (FadeTime, ()=> SceneManager.LoadScene(NextScene)).SetDelay(DelayTime);
    }

    /// <summary>
    /// シーン遷移を行う処理
    /// 会場選択
    /// </summary>
    public void MoveSelectScene()
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
        
        //- シーン遷移用アニメーションを再生する
        SelectPlayer = GetComponent<SelectMovePlayer>();
        SelectPlayer.InStageMove();

        //- ゲームパッドの取得
        Gamepad gamepad = Gamepad.current;

        //- 遅延後の処理
        if (gamepad != null) DOVirtual.DelayedCall(FadeTime, () => gamepad.SetMotorSpeeds(0.0f, 0.0f)); //- 振動を止める
        DOVirtual.DelayedCall(DelayTime, () => fadeObject.GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime));
        DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager()).SetDelay(DelayTime);
        DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(NextScene)).SetDelay(DelayTime);
    }
}