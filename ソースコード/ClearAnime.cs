/*
 ===================
 基盤制作：大川
 クリア演出を行うスクリプト
 ===================
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ClearAnime : MonoBehaviour
{
    private enum E_Directions
    {
        CENTER,
        TOP,
        LOWER,
        RIGHT,
        LEFT,
    }

    private enum E_POSSTATE
    {
        Rect,
        local,
    }


    [HeaderAttribute("---座標設定---")]
    [SerializeField]
    private E_POSSTATE PosState = E_POSSTATE.Rect;
    [HeaderAttribute("---フェード設定---")]
    [SerializeField, Header("移動処理を実行する:チェックで実行")]
    private bool UseFade = false;
    [SerializeField, Header("開始のアルファ値")]
    private float StartAlpha = 1.0f;
    [SerializeField, Header("終了のアルファ値")]
    private float EndAlpha = 0.0f;
    [SerializeField, Header("フェード完了までの時間:float")]
    private float FadeTime = 0.0f;
    [SerializeField, Header("ディレイ:float")]
    private float FadeDelay = 0.0f;


    [HeaderAttribute("---移動設定--")]
    [SerializeField, Header("移動処理を実行する:チェックで実行")]
    private bool UseMove = false;
    [SerializeField, Header("どこから現在位置に向かってくるか")]
    private E_Directions StartPos;
    [SerializeField, Header("移動完了までの時間:float")]
    private float MoveTime = 0.0f;
    [SerializeField, Header("ディレイ:float")]
    private float Delay = 0.0f;
    private Vector3 InitPos;

    [HeaderAttribute("---ポップ設定---")]
    [SerializeField, Header("ポップ処理を実行する：チェックで実行")]
    private bool UsePop = false;
    [SerializeField, Header("ポップの最大倍率：float")]
    private Vector2 PopSize;
    [SerializeField, Header("ポップの最大倍率までの時間：float")]
    private float PopMaxSizeTime = 1.0f;
    //[SerializeField, Header("ポップ後元サイズに戻るまでの時間：float")]
    //private float PopInitSizeTime = 1.0f;

    static int animeObjNum = 0;
    static bool isAnime = false;
    private bool isOnce = false;

    IgnoreMouseInputModule inputModule;

    private void Start()
    {
        inputModule = GameObject.Find("EventSystem").GetComponent<IgnoreMouseInputModule>();
        inputModule.enabled = false;
        isAnime = true;
        if (UseFade)
        { DoFade(); }
        if (UseMove)
        {   PosMove();   }
        if(UsePop)
        { DoPop(); }
    }

    private void Awake()
    {
        animeObjNum = 0;
    }

    private void Update()
    {
        if ( isAnime && animeObjNum <= 0) {
            //isOnce = true;
            inputModule.enabled = true;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void PosMove()
    {
        animeObjNum++;
        switch (PosState)
        {
            case E_POSSTATE.Rect:
                RectTransform trans = GetComponent<RectTransform>();
                //- 初期位置を保存
                InitPos = trans.anchoredPosition;
                //- 状態に合わせてスタート位置を変更
                switch (StartPos)
                {
                    case E_Directions.CENTER:
                        trans.anchoredPosition = new Vector2(0.0f, 0.0f);
                        break;
                    case E_Directions.TOP:
                        trans.anchoredPosition = new Vector2(InitPos.x, Screen.height);
                        break;
                    case E_Directions.LOWER:
                        trans.anchoredPosition = new Vector2(InitPos.x, -Screen.height);
                        break;
                    case E_Directions.RIGHT:
                        trans.anchoredPosition = new Vector2(Screen.width, InitPos.y);
                        break;
                    case E_Directions.LEFT:
                        trans.anchoredPosition = new Vector2(-Screen.width, InitPos.y);
                        break;
                }
                
                //- 初期位置にむかって移動
                transform.DOLocalMove(InitPos, MoveTime)
                    .SetEase(Ease.OutSine)
                    .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
                    .SetDelay(Delay)
                    .OnComplete(() => { animeObjNum--; });
                break;

            case E_POSSTATE.local:
                Transform localTrans = GetComponent<Transform>();
                //- 初期位置を保存
                InitPos = localTrans.position;
                //- 状態に合わせてスタート位置を変更
                switch (StartPos)
                {
                    case E_Directions.CENTER:
                        localTrans.position = new Vector3(0.0f, 0.0f);
                        break;
                    case E_Directions.TOP:
                        localTrans.position = new Vector3(InitPos.x, 30.0f,-5.0f);
                        break;
                    case E_Directions.LOWER:
                        localTrans.position = new Vector3(InitPos.x, -30.0f, -5.0f);
                        break;
                    case E_Directions.RIGHT:
                        localTrans.position = new Vector3(Screen.width, InitPos.y);
                        break;
                    case E_Directions.LEFT:
                         localTrans.position = new Vector3(-Screen.width, InitPos.y);
                         localTrans.position = new Vector3(-Screen.width, InitPos.y);
                        break;
                }



                //- 初期位置にむかって移動
                transform.DOMove(InitPos, MoveTime)
                    .SetEase(Ease.OutSine)
                    .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
                    .SetDelay(Delay)
                    .OnComplete(() => { animeObjNum--; });
                break;
        }
    }



/// <summary>
/// フェード処理
/// </summary>
private void DoFade()
    {
        animeObjNum++;

        Image image = GetComponent<Image>();
        //- 指定したアルファ値で開始
        image.color = new Color(image.color.r, image.color.g, image.color.b, StartAlpha);
        //- フェードを行う
        image.DOFade(EndAlpha, FadeTime).SetDelay(FadeDelay)
            .SetLink(image.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
            .OnComplete(() => {
                animeObjNum--;
            });
    }


    private void DoPop()
    {
        animeObjNum++;
        Debug.Log(animeObjNum);

        Image image = GetComponent<Image>();
        Vector2 Initsize = this.gameObject.transform.localScale;
        transform.DOScale(new Vector3(Initsize.x * PopSize.x, Initsize.y * PopSize.y, 0.0f), PopMaxSizeTime)
            .SetEase(Ease.OutSine)
            .SetLink(image.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
            .OnComplete(() => 
            {
                transform.DOScale(new Vector3(Initsize.x, Initsize.y, 0.0f), PopMaxSizeTime)
                .SetEase(Ease.OutSine)
                .SetLink(image.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
                .OnComplete(() => {
                    animeObjNum--;
                    Debug.Log(animeObjNum);
                });
            });
    }
}
