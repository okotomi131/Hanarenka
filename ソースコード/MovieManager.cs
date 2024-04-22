using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MovieManager : MonoBehaviour
{
    [SerializeField, Header("フェードの長さ(秒)")]
    private float FadeTime;

    [SerializeField, Header("フェード後打ち上げるまでの時間(秒)")]
    private float DelayBeltTime;

    [SerializeField, Header("打ち上げ後、爆発するまでの時間(秒)")]
    private float DelayFireflowerTime;
    

    [SerializeField, Header("爆発後、フェードが始まるまでの時間(秒)")]
    private float DelayFadeTime;

    [SerializeField, Header("ステージ描画オブジェクト")]
    private GameObject StageDrawObj;

    [SerializeField, Header("ぬし花火の動きの補完")]
    private Ease easy;

    [SerializeField, Header("ロードするステージ番号")]
    private int LoadStageNum;

    private SceneChange sceneChange; // コントローラーの振動用
    private bool bPlayMovie = false; // 演出中かどうか
    private ObjectFade fade; // フェード用のスプライト
    private GameObject movieObj; // 演出シーン内のオブジェクト
    private bool bPlayRequest; //- 演出開始申請 
    private CountEnemy countEnemy;

    void Start()
    {
        //- カメラの取得
        GameObject camera = GameObject.Find("Main Camera");
        //- メインカメラからシーン変更スクリプト取得
        sceneChange = camera.GetComponent<SceneChange>();
        //- フェード用スクリプトの取得
        fade = GameObject.Find("FadeImage").GetComponent<ObjectFade>();
        //- 花火玉をカウントする処理
        countEnemy = camera.GetComponent<CountEnemy>();
    }
    
    void Update()
    {
        // 現在の敵の数を更新
        int EnemyNum = countEnemy.GetCurrentCountNum();
        if (bPlayMovie && EnemyNum == 0) sceneChange.ResetCurrentTime(); //- 演出時、クリア演出が出ないようにする
        if (bPlayRequest) CheckCount(); //- 演出開始申請があれば呼び出される処理
    }

    //- 演出フラグを変更する関数
    public void SetMovieFlag(bool bFlag)
    {
        bPlayMovie = bFlag;
    }

    //- 演出処理を開始する関数
    public void StartVillageMovie()
    {
        bPlayRequest = true;
    }

    private void CheckCount()
    {
        // 現在の敵の数を更新
        int EnemyNum = countEnemy.GetCurrentCountNum();

        if (EnemyNum != 0) return; //- 花火玉が残っていればリターン
        
        bPlayRequest = false;
        StartCoroutine(MovieSequence());
    }

    private IEnumerator MovieSequence()
    {
        bPlayMovie = true; //- 演出フラグ変更

        //- フェードを登場させる
        fade.SetFade(ObjectFade.FadeState.In, FadeTime);
        yield return new WaitForSeconds(FadeTime);

        //- 演出シーンを追加ロード,フェードを退場させる
        LoadMovieScene();
        fade.SetFade(ObjectFade.FadeState.Out, FadeTime);
        yield return new WaitForSeconds(FadeTime);

        //- 演出シーン内オブジェクトを検索
        InitMovieObject();

        //- 一定時間後、ぬし花火を打ち上げる
        if (LoadStageNum != 4)
        {
            yield return new WaitForSeconds(DelayBeltTime);
            SetActiveMovieObject(0, true);
            AnimeBossFireflower(0);
        }

        //- 一定時間後、花火を発生させる
        yield return new WaitForSeconds(DelayFireflowerTime);
        //- 里に応じて流す効果音を変える
        if (LoadStageNum != 4) { SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion); }
        else { SEManager.Instance.EffectSetPlaySE(SEManager.E_SoundEffect.DragonFire, 0.1f); }
        SetActiveMovieObject(0, false); //- ぬし花火の退場
        SetActiveMovieObject(1, true);  //- エフェクト登場

        if (LoadStageNum != 4)
        {
            //- 一定時間後、フェードを登場させる
            yield return new WaitForSeconds(DelayFadeTime);
            fade.SetFade(ObjectFade.FadeState.In, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //- 演出シーンをアンロード
            UnloadMovieScene();
            fade.SetFade(ObjectFade.FadeState.Out, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopClear(false);
            scenechange.RequestStopClear(false);
            scenechange.RequestStopClear(false);
            scenechange.RequestStopMiss(false);
            bPlayMovie = false; //- 演出フラグ変更
        }
        else
        {
            //- 一定時間後、フェードを登場させる
            yield return new WaitForSeconds(DelayFadeTime);
            fade.SetColor(Color.white, 0.0f);
            fade.SetFade(ObjectFade.FadeState.In, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //- 演出シーンをアンロード
            UnloadMovieScene();
            fade.SetFade(ObjectFade.FadeState.Out, FadeTime);
            GameObject.Find("BGMManager").GetComponent<BGMManager>().DestroyPossible(); // BGMManagerを削除可能にする
            SceneChange.bIsChange = true;
            GameObject.Find("Main Camera").GetComponent<SceneChange>().Change(); // シーン切り替え
            yield return new WaitForSeconds(FadeTime);
        }
    }

    //- 演出用シーンのロードを行う関数
    private void LoadMovieScene()
    {
        StageDrawObj.SetActive(false); //- ステージオブジェクトの描画をやめる
        SceneManager.LoadScene("MovieVillage" + LoadStageNum.ToString(), LoadSceneMode.Additive); //- 演出用シーンを追加ロード   
    }

    //- 演出シーンロード後、オブジェクトを入手するための初期化関数
    private void InitMovieObject()
    {
        movieObj = GameObject.Find("MovieObject"); //- オブジェクトの検索
    }

    //- ぬし花火用の演出処理
    private void AnimeBossFireflower(int childNum)
    {
        //- ぬし花火の置き換え
        GameObject obj = movieObj.transform.GetChild(childNum).gameObject;
        //- ぬし花火のスケールフェード
        obj.transform.DOScale(new Vector3(1, 1, 1), DelayFireflowerTime).SetEase(easy);
    }

    //- 特定のオブジェクトのフラグを変更する関数
    private void SetActiveMovieObject(int childNum, bool bFlag)
    {
        movieObj.transform.GetChild(childNum).gameObject.SetActive(bFlag); //- フラグ変更
    }

    //- 演出用シーンのアンロードを行う関数
    private void UnloadMovieScene()
    {
        StageDrawObj.SetActive(true); //- ステージオブジェクトの描画を再開
        SceneManager.UnloadScene("MovieVillage" + LoadStageNum.ToString()); //- 演出用シーンのアンロード
    }
}