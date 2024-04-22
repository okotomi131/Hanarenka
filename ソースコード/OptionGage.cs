using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class OptionGage : MonoBehaviour
{
    //[SerializeField, Header("敵をカウントしているオブジェクト")]
    //private GameObject CountObject;

    [SerializeField, Header("オプション操作にかかる時間(秒)")]
    private float OptionTime = 1.0f;
    [SerializeField, Header("シーン移動時のフェード時間(秒)")]
    private float FadeTime = 1.0f;
    [SerializeField, Header("セレクトシーン")]
    private SceneObject SelectScene;

    //- ボタンが押されているかどうか
    bool bIsPushRetry = false;
    bool bIsPushInGame = false;

    //- ボタンが押された時間
    float bPushTimeRetry = 0;
    float bPushTimeInGame = 0;

    //- イメージのコンポーネント
    Image imageRetry;
    Image imageInGame;

    //- シーン移動が始まったかどうかのフラグ
    bool bIsStartRetry = false;
    bool bIsStartInGame = false;

    // CountEnemyスクリプトを入れる変数
    CountEnemy countEnemy;

    //- プレイヤーの情報を取得する変数
    PController player;

    //- ゲームパッド
    Gamepad gamepad;

    void Start()
    {
        //- コンポーネントの取得
        imageRetry = GameObject.Find("ResetGage").GetComponent<Image>();
        imageInGame = GameObject.Find("InGameGage").GetComponent<Image>();
        //- 敵カウントUIの取得
        countEnemy = GameObject.Find("Main Camera").GetComponent<CountEnemy>();

        //- プレイヤーの情報を
        player = GameObject.Find("Player").GetComponent<PController>();
        //- ゲームパッドの取得
        gamepad = Gamepad.current;
    }
    
    void FixedUpdate()
    {

        // 現在の敵の数を更新
        int EnemyNum = countEnemy.GetCurrentCountNum();
        //- シーン移動が始まったかどうかのフラグを取得
        bool bIsMoveScene = false;

        //- シーン移動のフラグ更新 
        if (bIsStartInGame || bIsStartRetry) bIsMoveScene = true;

        //＝＝＝＝＝　リトライボタン　＝＝＝＝＝
        //- 「敵が残っている」「シーン移動ボタンが押されてる」「シーン移動が始まっていない」
        //- 上記をすべて満たせば処理する
        if (bIsPushRetry && EnemyNum > 0 && !bIsMoveScene)
        {
            bPushTimeRetry += Time.deltaTime;                    //- プッシュ時間の更新
            imageRetry.fillAmount = bPushTimeRetry / OptionTime; //- ゲージの更新
        }
        else if (!bIsMoveScene)
        {
            bPushTimeRetry = 0;        //- プッシュ時間のリセット
            imageRetry.fillAmount = 0; //- ゲージのリセット
        }
        //- 一定時間長押しされたら処理する
        if (bPushTimeRetry >= OptionTime)
        {
            if (bIsStartRetry == true) return; //- リセット開始フラグがたっていればリターン
            imageInGame.fillAmount = 0; //- 反対のゲージのリセット
            bIsStartRetry = true; //シーン開始フラグをたてる

            GameObject.Find("FadeImage").GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime); // フェード開始
            if (gamepad != null) DOVirtual.DelayedCall(FadeTime, () => gamepad.SetMotorSpeeds(0.0f, 0.0f)); //- 振動を止める
            DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name)); // シーンのロード(遅延あり)
        }

        //- シーン移動のフラグ更新 
        if (bIsStartInGame || bIsStartRetry) bIsMoveScene = true;

        //＝＝＝＝＝　インゲームボタン　＝＝＝＝＝
        //- 「敵が残っている」「シーン移動ボタンが押されてる」「シーン移動が始まっていない」
        //- 上記をすべて満たせば処理する
        if (bIsPushInGame && EnemyNum > 0 && !bIsMoveScene)
        {
            bPushTimeInGame += Time.deltaTime;                    //- プッシュ時間の更新
            imageInGame.fillAmount = bPushTimeInGame / OptionTime; //- ゲージの更新
        }
        else if (!bIsMoveScene)
        {
            bPushTimeInGame = 0;        //- プッシュ時間のリセット
            imageInGame.fillAmount = 0; //- ゲージのリセット
        }
        //- 一定時間長押しされたら処理する
        if (bPushTimeInGame >= OptionTime)
        {
            if (bIsStartInGame == true) return; //- リセット開始フラグがたっていればリターン
            imageRetry.fillAmount = 0; //- 反対のゲージのリセット
            bIsStartInGame = true; //シーン開始フラグをたてる

            GameObject.Find("FadeImage").GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime); // フェード開始
            BGMManager bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
            if (gamepad != null) DOVirtual.DelayedCall(FadeTime, () => gamepad.SetMotorSpeeds(0.0f, 0.0f)); //- 振動を止める
            DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager());
            DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(SelectScene)); // シーンのロード(遅延あり)
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        //- ボタンが押されている間、プレイヤーが動作可能時に変数を設定
        if (context.started && !player.GetWaitFlag()) { bIsPushRetry = true; }
        if (context.canceled && !player.GetWaitFlag()) { bIsPushRetry = false; }
    }

    public void OnInGame(InputAction.CallbackContext context)
    {
        //- ボタンが押されている間、、プレイヤーが動作可能時に変数を設定
        if (context.started && !player.GetWaitFlag()) { bIsPushInGame = true; }
        if (context.canceled && !player.GetWaitFlag()) { bIsPushInGame = false; }
    }
}
