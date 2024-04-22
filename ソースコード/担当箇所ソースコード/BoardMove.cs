/*
 ===================
 制作：大川
 ギミックガイドアニメーションを管理するスクリプト
 ===================
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Video;
#if UNITY_EDITOR
//- デプロイ時にEditorスクリプトが入るとエラー。UNITY_EDITORで括る
using UnityEditor;
#endif

//- ギミック説明看板のアニメーション
public class BoardMove : MonoBehaviour
{
    private enum E_OUTDIRECTION
    {
        [Header("左")]
        LEFT,
        [Header("右")]
        RIGHT,
        [Header("上")]
        UP,
        [Header("下")]
        DOWN,
    }

    private const float LEFT = -2500.0f;
    private const float RIGHT = 3500.0f;
    private const float TOP = 1200.0f;
    private const float DOWN = -1200.0f;

    [SerializeField] private Image img;
    [SerializeField] private VideoPlayer movie;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private float IntervalTime = 0.0f;
    [SerializeField] private Image padbutton;
    [SerializeField] private TextMeshProUGUI padtmp;
    private Dictionary<string, Dictionary<string, Vector3>> InitValues;
    public static bool MoveComplete = false;

    private bool LoadStart = false;
    private bool LoadOut = false;
    private bool ReInMove;     //登場ボタン入力フラグ
    private bool ReOutMove;    //撤退ボタン入力フラグ
    private bool NowMove = false; //動作中フラグ
    private bool NotInput = false;//入力受付フラグ
    private bool StartComplete = false;//登場処理が完了したか

    private void Awake()
    {
        //- 初期値登録
        InitValues = new Dictionary<string, Dictionary<string, Vector3>>
        {{"動画",new Dictionary<string, Vector3>{{"位置",movie.transform.position},}}};
        InitValues.Add("文字", new Dictionary<string, Vector3> { { "位置", tmp.transform.position } });
        InitValues.Add("背景", new Dictionary<string, Vector3> { { "位置", img.transform.position } });
        //- 初期位置更新
        img.transform.localPosition = new Vector3(LEFT + img.transform.localPosition.x, img.transform.localPosition.y);
        movie.transform.localPosition = new Vector3(LEFT + img.transform.localPosition.x, movie.transform.localPosition.y);
        tmp.transform.localPosition = new Vector3(LEFT + img.transform.localPosition.x, tmp.transform.localPosition.y);
        //- 待機時は動画停止
        movie.Pause();
    }
    
    /// <summary>
    /// 登場挙動を行う
    /// </summary>
    public void StartMove()
    {
        //- 初回読み込み
        if(!LoadStart)
        {
            //- 読み込み済にする
            LoadStart = true;
            //- 登場処理に入ったらボタン入力を非入力状態に
            ReInMove = false;
            //- 登場する際に動画再生
            movie.Play();
            //- シーケンス作成
            var InAnime = DOTween.Sequence();
            //- 登場挙動
            InAnime.AppendInterval(IntervalTime)
            .Append(img.transform.DOMove(InitValues["背景"]["位置"], 0.5f))
            .Join(movie.transform.DOMove(InitValues["動画"]["位置"], 0.525f))
            .Join(tmp.transform.DOMove(InitValues["文字"]["位置"], 0.5f))
            .OnComplete(() => {
                //- 登場挙動完了したら撤退可能にする
                LoadOut = false;
                //-　表示処理が完了
                StartComplete = true;
                //- 挙動が完成したらアニメーション削除
                InAnime.Kill();
            });
        }
    }

    /// <summary>
    /// 撤退挙動を行う
    /// </summary>
    public void OutMove()
    {
        //- 初回読み込み
        if(!LoadOut)
        {
            //- 読み込み済にする
            LoadOut = true;
            //- 連打防止フラグ
            NowMove = false;
            //- 撤退する前に動画の再生を止める
            movie.Stop();
            //- シーケンス作成
            var OutAnime = DOTween.Sequence();

            //- 撤退処理動作
                OutAnime.Append(movie.transform.DOMoveX(RIGHT, 0.3f))
                .Join(img.transform.DOMoveX(RIGHT, 0.3f))
                .Join(tmp.transform.DOMoveX(RIGHT, 0.3f))
                .OnComplete(() =>
                {
                    //- 初期位置更新
                    img.transform.localPosition = new Vector3(LEFT, img.transform.localPosition.y);
                    movie.transform.localPosition = new Vector3(LEFT, movie.transform.localPosition.y);
                    tmp.transform.localPosition = new Vector3(LEFT, tmp.transform.localPosition.y);
                    //- 撤退挙動完了したらTips再表示可能にする
                    LoadStart = false;
                    //- 一連の挙動が終了
                    MoveComplete = true;
                    //- アニメーション削除
                    OutAnime.Kill();
                });
        }
    }

    /// <summary>
    ///  初回動作完了フラグをリセット
    /// </summary>
    public static void ResetMoveComplete()
    {   MoveComplete = false;    }
    
    /// <summary>
    /// 再登場フラグ返却
    /// </summary>
    public bool GetInDrawButtonPush()
    {   return ReInMove;     }

    /// <summary>
    /// 再撤退フラグ返却
    /// </summary>
    public bool GetOutDrawButtonPush()
    { return ReOutMove;      }  

    /// <summary>
    /// 登場フラグ返却
    /// </summary>
    public bool GetLoadStart()
    { return LoadStart; }
    
    /// <summary>
    /// 退場フラグ返却
    /// </summary>
    public bool GetLoadOut()
    { return LoadOut; }

    public bool GetStartComplete()
    { return StartComplete; }

    /// <summary>
    /// 再登場入力
    /// </summary>
    /// <param name="context"></param>
    public void OnInTips(InputAction.CallbackContext context)
    {
        //- 入力受付フラグが立っていたら処理しない
        if (NotInput) { return; }
        //- 動作中であれば実行しない
        if (NowMove)  { return; }
        NowMove = true;

        //- Tips再描画フラグをオンにする
        if (context.started && !SceneChange.bIsChange)
        {
            //- 再登場時は待機時間を無くす
            IntervalTime = 0.0f;
            ReInMove = true;
        }
    }

    /// <summary>
    /// Tips表示を受け付けるか
    /// </summary>
    /// <param name="flag"></param>
    public void SetReceiptInput(bool flag)
    {   NotInput = flag;    }

    public void SetButtonColor(Color color)
    {
        //- すでに同じ色なら処理しない
        if(padbutton.color == color)
        { return; }
        //- 指定色に変更
        padbutton.color = color;
        padtmp.color = color;
    }

    /// <summary>
    /// 再撤退入力
    /// </summary>
    /// <param name="context"></param>
    public void OnOutTips(InputAction.CallbackContext context)
    {
        //- クリアしていない際にボタン入力を受け付ける
        if (context.started && !SceneChange.bIsChange)
        {   ReOutMove = true;  }//入力中
        if(context.canceled && !SceneChange.bIsChange)
        {   ReOutMove = false; }//入力中止
    }

    
}

