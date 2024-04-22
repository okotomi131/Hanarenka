using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("シーン遷移先")]
    private SceneObject NextScene;

    //[SerializeField, Header("敵をカウントしているオブジェクト")]
    //private GameObject CountObject;

    [SerializeField, Header("クリア時のシーン遷移を遅らす時間(秒)")]
    private float ClearDelayTime = 2.0f;

    [SerializeField, Header("リトライ時のサウンドが発生する時間(秒)")]
    private float SoundDelayTime = 2.0f;

    [SerializeField, Header("リトライ時のシーン遷移を遅らす時間(秒)")]
    private float RetryDelayTime = 2.0f;

    CountEnemy countEnemy;          // CountEnemyスクリプトを入れる変数

    private int EnemyNum;           // 敵の数
    private float CurrentTime;      // 現在の時間(敵が全滅してからカウント開始)
    private float CurrentParticleTime = 0.0f;   // パーティクルの現在の時間
    private float TotalParticleTime = 999.0f;   // パーティクルの総時間
    private bool bIsShotSound = false;          // 音が再生されたかどうか
    private ObjectFade fade;              // フェード用のスプライト
    private ClearManager clear;
    private bool bIsStopClear = false; //- 成功処理を一時停止するかどうか
    private bool bIsStopMiss =  false; //- 失敗処理を一時停止するかどうか    
    private int RequestCountClear = 0; //- 成功判定依頼が呼ばれている数
    private int RequestCountMiss  = 0; //- 失敗判定依頼が呼ばれている数

    public static bool bIsChange;   // 次のシーンに移動するかのフラグ
    public static bool bIsRetry;    // リトライするかのフラグ
    public bool bIsLife;     // プレイヤーが生存しているか

    // Start is called before the first frame update
    void Start()
    {
        bIsChange = false;
        bIsRetry = false;
        bIsLife = true;
        countEnemy = this.gameObject.GetComponent<CountEnemy>();
        fade = GameObject.Find("FadeImage").GetComponent<ObjectFade>();
        clear = GameObject.Find("ClearManager").GetComponent<ClearManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 現在の敵の数を更新
        EnemyNum = countEnemy.GetCurrentCountNum();
        if (bIsStopClear && bIsStopMiss) CurrentTime = 0;

        // パーティクルの再生が終わる + 敵を全滅させたら + 処理フラグがtrue
        if(EnemyNum <= 0 && !bIsStopClear)
        {
            // 現在の時間を更新
            CurrentTime += Time.deltaTime;
            // 現在の時間が遅延時間を超えたらシーン遷移フラグをtrueに変えて、音を再生する
            if (CurrentTime >= ClearDelayTime)
            {
                bIsChange = true;
            }
        }

        // パーティクルの再生が終わる + 敵が残っている + 処理フラグがtrue
        if (CurrentParticleTime == TotalParticleTime && EnemyNum > 0 && !bIsLife && !bIsStopMiss)
        {
            // 現在の時間を更新
            CurrentTime += Time.deltaTime;
            // 現在の時間が遅延時間を超えたらリトライフラグをtrueに変える
            if (CurrentTime >= RetryDelayTime)
            {
                CurrentTime = 0;
                bIsRetry = true;
            }
        }

        // パーティクルが再生中なら現在時間をリセット
        if (CurrentParticleTime != TotalParticleTime)
        {
            CurrentTime = 0;
        }

        // シーン遷移フラグがtrueならクリア情報を書き込む
        if (bIsChange)
        {
            clear.WriteClear();
        }

        // リトライフラグがtrueなら現在のシーンを再読み込み
        if (bIsRetry)
        {
            // 現在の時間を更新
            CurrentTime += Time.deltaTime;
            //- 1度だけ再生
            if (!bIsShotSound)
            {
                //- 変数の設定
                bIsShotSound = true;
                //- 失敗音の再生
                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Failure);
                // フェードの設定
                fade.SetFade(ObjectFade.FadeState.In, 1.0f);
            }
            if (!fade.isFade) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void SetParticleTime(float currentTime, float totalTime)
    {
        // パーティクルの現在の時間を代入
        CurrentParticleTime = currentTime;
        // パーティクルの総時間を代入
        TotalParticleTime = totalTime;
    }

    public void ResetCurrentTime()
    {
        CurrentTime = 0;
    }

    //public void SetStopClearFlag(bool flag)
    //{
    //    bIsStopClear = flag;
    //}

    //public void SetStopMissFlag(bool flag)
    //{
    //    bIsStopMiss = flag;
    //}

    public void RequestStopClear(bool flag)
    {
        //- リクエスト数の操作
        if (flag) RequestCountClear++;
        else      RequestCountClear--;

        //- リクエスト数に基づいてフラグの操作
        if (RequestCountClear > 0) bIsStopClear = true;
        else bIsStopClear = false;
    }

    public void RequestStopMiss(bool flag)
    {
        //- リクエスト数の操作
        if (flag) RequestCountMiss++;
        else 　　 RequestCountMiss--;

        //- リクエスト数に基づいてフラグの操作
        if (RequestCountMiss > 0) bIsStopMiss = true;
        else bIsStopMiss = false;
    }

    public void Change()
    {
        clear.WriteClear();
        SceneManager.LoadScene(NextScene);
    }
}