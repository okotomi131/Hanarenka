/*
 ===================
 基盤制作：大川
 ボスステージのカットインを行うスクリプト
 ===================
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

//- カットインアニメーションを処理するクラス
public class CutIn : MonoBehaviour
{
    //- どのステージのボスか
    private enum E_BOSS_CUTIN
    {
        StageNo10_Boss,
        StageNo20_Boss,
        StageNo30_Boss,
        StageNo40_Boss,
    }

    [SerializeField] private E_BOSS_CUTIN Boss = E_BOSS_CUTIN.StageNo10_Boss;
    [SerializeField] private Image BossImg;         //ボス画像
    [SerializeField] private Image BossBack;        //ボス画像の縁用
    [SerializeField] private Image TutuB;           //30ボス用
    [SerializeField] private Image TutuC;           //30ボス用
    [SerializeField] private Image SmallCrystal;    //10ボス用
    [SerializeField] private Image BigCrystal;      //10・20ボス用
    [SerializeField] private Image TextBack;        //カットイン背景
    [SerializeField] private TextMeshProUGUI tmp;   //カットインメッセージ

    public static bool MoveCompleat = false;        //アニメーション終了フラグ

    private Vector3 InitPos = new Vector3(9999.0f, 9999.0f);    //初期位置
    private Vector3 InitTextPos;                                //初期テキスト位置
    private Dictionary<string,Dictionary<string, Vector3>> InitValues;  //配置値保存変数

    private void Awake()
    {
        //- ボス別に初期値を保存
        switch (Boss)
        {
            case E_BOSS_CUTIN.StageNo10_Boss:
                InitSaveBoss10or20();
                break;
            case E_BOSS_CUTIN.StageNo20_Boss:
                InitSaveBoss10or20();
                break;
            case E_BOSS_CUTIN.StageNo30_Boss:
                InitSaveBoss30();
                break;
            case E_BOSS_CUTIN.StageNo40_Boss:
                InitSaveBoss40();
                break;
        }
    }

    /// <summary>
    /// 10ステージ又は20ステージのボスの値を保存する関数
    /// </summary>
    private void InitSaveBoss10or20()
    {
        //- 初期値保存
        InitValues = new Dictionary<string, Dictionary<string, Vector3>>
        {{ "ボス", new Dictionary<string, Vector3>
             {{ "開始位置", Vector3.zero },
             { "終点位置", BossImg.transform.localPosition },
             { "大きさ", BossImg.transform.localScale },}
        }};
        if (SmallCrystal)
        {
            InitValues.Add("バリア小", new Dictionary<string, Vector3> {
            { "開始位置", Vector3.zero },
            { "終点位置", SmallCrystal.transform.localPosition },
            { "大きさ", SmallCrystal.transform.localScale },});
        }
        if (BigCrystal)
        {
            InitValues.Add("バリア大", new Dictionary<string, Vector3> {
            { "開始位置", Vector3.zero },
            { "終点位置", BigCrystal.transform.localPosition },
            { "大きさ", BigCrystal.transform.localScale },});
        }

        //- サイズと位置をアニメーション開始時の値に設定
        BossImg.rectTransform.localScale = Vector3.zero;
        BossImg.rectTransform.localPosition = InitPos;
        if (SmallCrystal)
        {
            SmallCrystal.rectTransform.localScale = Vector3.zero;
            SmallCrystal.rectTransform.localPosition = InitPos;
            SmallCrystal.rectTransform.localRotation = Quaternion.Euler(0, 0, 90.0f);
        }
        if (BigCrystal)
        {
            BigCrystal.rectTransform.localScale = Vector3.zero;
            BigCrystal.rectTransform.localRotation = Quaternion.Euler(0, 0, 90.0f);
            BigCrystal.rectTransform.localPosition = InitPos;
        }
        InitTextPos = tmp.rectTransform.localPosition;
        tmp.rectTransform.localPosition = InitPos;
    }

    /// <summary>
    /// 30ステージのボスの値を保存する関数
    /// </summary>
    private void InitSaveBoss30()
    {     
        //- 初期値保存
        InitValues = new Dictionary<string, Dictionary<string, Vector3>>
        {{ "ボスA", new Dictionary<string, Vector3>
             {{ "開始位置", Vector3.zero },
             { "終点位置", BossImg.transform.localPosition },
             { "大きさ", BossImg.transform.localScale },}
        }};
        InitValues.Add("ボスB", new Dictionary<string, Vector3> {
            { "開始位置", Vector3.zero },
            { "終点位置", TutuB.transform.localPosition },
            { "大きさ", TutuB.transform.localScale },});
        InitValues.Add("ボスC", new Dictionary<string, Vector3> {
            { "開始位置", Vector3.zero  },
            { "終点位置", TutuC.transform.localPosition },
            { "大きさ", TutuC.transform.localScale },});

        //- サイズと位置をアニメーション開始時の値に設定
        BossImg.rectTransform.localScale = Vector3.zero;    //A
        BossImg.rectTransform.localPosition = InitPos;
        TutuB.rectTransform.localScale = Vector3.zero;      //B
        TutuB.rectTransform.localPosition = InitPos;
        TutuC.rectTransform.localScale = Vector3.zero;      //C
        TutuC.rectTransform.localPosition = InitPos;
        InitTextPos = tmp.rectTransform.localPosition;
        tmp.rectTransform.localPosition = InitPos;
    }

    /// <summary>
    /// 40ステージのボスの値を保存する関数
    /// </summary>
    private void InitSaveBoss40()
    {
        //- 初期値保存
        InitValues = new Dictionary<string, Dictionary<string, Vector3>>
        {{ "ボス", new Dictionary<string, Vector3>
             {{ "開始位置", Vector3.zero },
             { "終点位置", BossImg.transform.localPosition },
             { "大きさ", BossImg.transform.localScale },}
        }};
        InitValues.Add("ボス背景", new Dictionary<string, Vector3> {
            { "開始位置", Vector3.zero },
            { "終点位置", BossBack.transform.localPosition },
            { "大きさ", BossBack.transform.localScale },});
        //- サイズと位置をアニメーション開始時の値に設定
        BossImg.rectTransform.localScale = Vector3.zero;
        BossImg.rectTransform.localPosition = InitPos;
        BossBack.rectTransform.localScale = Vector3.zero;
        BossBack.rectTransform.localPosition = InitPos;
        InitTextPos = tmp.rectTransform.localPosition;
        tmp.rectTransform.localPosition = InitPos;
    }

    /// <summary>
    /// カットイン挙動を行う
    /// </summary>
    public void MoveCutIn()
    {
        switch (Boss)
        {
            case E_BOSS_CUTIN.StageNo10_Boss:
                MoveBoss10();
                break;
            case E_BOSS_CUTIN.StageNo20_Boss:
                MoveBoss20();
                break;
            case E_BOSS_CUTIN.StageNo30_Boss:
                MoveBoss30();
                break;
            case E_BOSS_CUTIN.StageNo40_Boss:
                MoveBoss40();
                break;
        }

    }

    /// <summary>
    /// ステージ10のボスカットイン処理
    /// </summary>
    private void MoveBoss10()
    {
        var DoCutIn = DOTween.Sequence();
        //- 初めのテキストを90度回転させておく
        DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(tmp);
        for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
        { tmpAnimator.DORotateChar(i, Vector3.up * 90, 0); }
        //- ボス画像のサイズが0から元設置サイズに
        DoCutIn
            .OnPlay(() =>
            {
                BossImg.transform.localPosition = InitValues["ボス"]["開始位置"];
                SmallCrystal.transform.localPosition = InitValues["バリア小"]["開始位置"];
                BigCrystal.transform.localPosition = InitValues["バリア大"]["開始位置"];
            })
            .AppendInterval(0.5f)                                                            //遅延
            .Append(BossImg.transform.DOScale(InitValues["ボス"]["大きさ"], 0.1f).SetEase(Ease.InOutCubic))            //アニメーション開始
            .Append(SmallCrystal.transform.DOScale(InitValues["バリア小"]["大きさ"], 0.1f).SetEase(Ease.InOutCubic))
            .Append(BigCrystal.transform.DOScale(InitValues["バリア大"]["大きさ"], 0.1f).SetEase(Ease.InOutCubic))
            .Append(SmallCrystal.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.InOutCubic))
            .Append(BigCrystal.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.InOutCubic))
            .AppendInterval(0.25f)
            .Append(BossImg.transform.DOMove(InitValues["ボス"]["終点位置"], 0.5f).SetRelative(true).SetEase(Ease.InOutCubic))
            .Join(SmallCrystal.transform.DOMove(InitValues["バリア小"]["終点位置"], 0.5f).SetRelative(true).SetEase(Ease.InOutCubic))
            .Join(BigCrystal.transform.DOMove(InitValues["バリア大"]["終点位置"], 0.5f).SetRelative(true).SetEase(Ease.InOutCubic))
            .OnComplete(() =>
            {
                DOTween.Sequence()
                .Append(TextBack.DOFillAmount(1.0f, 0.25f))
                .OnPlay(() => { tmp.transform.localPosition = InitTextPos; });

                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Letterapp); // 文字出現音
                for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
                {
                    DOTween.Sequence()
                        .Append(tmpAnimator.DORotateChar(i, Vector3.zero, 0.55f));
                }
                DoCutIn.Kill();

                var DoOut = DOTween.Sequence();
                DoOut
                    .AppendInterval(1.5f)
                    .Append(BossImg.DOFade(0.0f, 0.2f))
                    .Join(SmallCrystal.DOFade(0.0f, 0.2f))
                    .Join(BigCrystal.DOFade(0.0f, 0.2f))
                    .Join(tmp.DOFade(0.0f, 0.2f))
                    .Join(TextBack.DOFade(0.0f, 0.2f))
                    .OnComplete(() => {
                        MoveCompleat = true;//初回挙動完了処理
                        DoOut.Kill();
                    });

            });
    }

    /// <summary>
    /// ステージ20のボスカットイン処理
    /// </summary>
    private void MoveBoss20()
    {
        var DoCutIn = DOTween.Sequence();
        //- 初めのテキストを90度回転させておく
        DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(tmp);
        for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
        { tmpAnimator.DORotateChar(i, Vector3.up * 90, 0); }
        //- ボス画像のサイズが0から元設置サイズに
        DoCutIn
            .OnPlay(() =>
            {BossImg.transform.localPosition = InitValues["ボス"]["開始位置"];
             BigCrystal.transform.localPosition = InitValues["バリア大"]["開始位置"];})
            .AppendInterval(0.5f)
            .Append(BossImg.transform.DOScale(InitValues["ボス"]["大きさ"], 0.1f))
            .Append(BigCrystal.transform.DOScale(InitValues["バリア大"]["大きさ"], 0.1f))
            .Append(BigCrystal.transform.DORotate(Vector3.zero, 0.3f))
            .AppendInterval(0.25f)
            .Append(BossImg.transform.DOMove(InitValues["ボス"]["終点位置"], 0.5f).SetRelative(true))
            .Join(BigCrystal.transform.DOMove(InitValues["バリア大"]["終点位置"], 0.5f).SetRelative(true))
            .OnComplete(() =>
            {
                DOTween.Sequence()
                .Append(TextBack.DOFillAmount(1.0f, 0.25f))
                .OnPlay(() => { tmp.transform.localPosition = InitTextPos; });

                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Letterapp); // 文字出現音
                for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
                {
                    DOTween.Sequence()
                        .Append(tmpAnimator.DORotateChar(i, Vector3.zero, 0.55f));
                }
                DoCutIn.Kill();

                var DoOut = DOTween.Sequence();
                DoOut
                    .AppendInterval(1.5f)
                    .Append(BossImg.DOFade(0.0f, 0.2f))
                    .Join(BigCrystal.DOFade(0.0f, 0.2f))
                    .Join(tmp.DOFade(0.0f, 0.2f))
                    .Join(TextBack.DOFade(0.0f, 0.2f))
                    .OnComplete(() => {
                        MoveCompleat = true;
                        DoOut.Kill();
                    });

            });
    }

    /// <summary>
    /// ステージ30のボスカットイン処理
    /// </summary>
    private void MoveBoss30()
    {
        var DoCutIn = DOTween.Sequence();
        //- 初めのテキストを90度回転させておく
        DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(tmp);
        for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
        { tmpAnimator.DORotateChar(i, Vector3.up * 90, 0); }

        DoCutIn
            .OnPlay(() =>
            {

                //- 開始位置にスタンバイ
                Vector3 pos = InitValues["ボスB"]["開始位置"];
                pos.y -= 50.0f;
                BossImg.transform.localPosition = pos;

                pos = InitValues["ボスB"]["開始位置"];
                pos.x += 350.0f;
                pos.y -= 50.0f;
                TutuB.transform.localPosition = pos;

                pos = InitValues["ボスC"]["開始位置"];
                pos.x += 195.0f;
                pos.y += 150.0f;
                TutuC.transform.localPosition = pos;
            })
            .AppendInterval(0.5f)   //遅延
            .Append(BossImg.transform.DOScale(InitValues["ボスA"]["大きさ"], 0.1f).SetEase(Ease.InOutCubic))
            .Append(TutuB.transform.DOScale(InitValues["ボスB"]["大きさ"], 0.1f).SetEase(Ease.InOutCubic))
            .Append(TutuC.transform.DOScale(InitValues["ボスC"]["大きさ"], 0.1f).SetEase(Ease.InOutCubic))
            .AppendInterval(0.25f)
            .Append(BossImg.transform.DOLocalMove(InitValues["ボスA"]["終点位置"], 0.5f).SetEase(Ease.InOutCubic))
            .Join(TutuB.transform.DOLocalMove(InitValues["ボスB"]["終点位置"], 0.5f).SetEase(Ease.InOutCubic))
            .Join(TutuC.transform.DOLocalMove(InitValues["ボスC"]["終点位置"], 0.5f).SetEase(Ease.InOutCubic))
            .OnComplete(() =>
            {

                DOTween.Sequence()
                .Append(TextBack.DOFillAmount(1.0f, 0.25f))
                .OnPlay(() => { tmp.transform.localPosition = InitTextPos; });

                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Letterapp); // 文字出現音
                for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
                {
                    DOTween.Sequence()
                        .Append(tmpAnimator.DORotateChar(i, Vector3.zero, 0.55f));
                }
                DoCutIn.Kill();

                var DoOut = DOTween.Sequence();
                DoOut
                    .AppendInterval(1.5f)
                    .Append(BossImg.DOFade(0.0f, 0.2f))
                    .Join(TutuB.DOFade(0.0f, 0.2f))
                    .Join(TutuC.DOFade(0.0f, 0.2f))
                    .Join(tmp.DOFade(0.0f, 0.2f))
                    .Join(TextBack.DOFade(0.0f, 0.2f))
                    .OnComplete(() =>
                    {
                        MoveCompleat = true;//初回挙動完了処理
                        DoOut.Kill();
                    });
            });


    }

    private void MoveBoss40()
    {
        var DoCutIn = DOTween.Sequence();
        //- 初めのテキストを90度回転させておく
        DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(tmp);
        for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
        { tmpAnimator.DORotateChar(i, Vector3.up * 90, 0); }
        //- ボス画像のサイズが0から元設置サイズに
        DoCutIn
            .OnPlay(() =>
            {
                BossImg.transform.localPosition = InitValues["ボス"]["開始位置"];
                BossBack.transform.localPosition = InitValues["ボス背景"]["開始位置"];
                BossImg.DOColor(Color.black,0.0f);
            })
            .AppendInterval(0.5f)
            .Append(BossImg.transform.DOScale(InitValues["ボス"]["大きさ"], 0.1f))
            .Join(BossBack.transform.DOScale(InitValues["ボス背景"]["大きさ"], 0.1f))
            .AppendInterval(0.3f)
            .Append(BossImg.DOColor(Color.white, 0.5f))
            .AppendInterval(0.25f)
            .Append(BossImg.transform.DOMove(InitValues["ボス"]["終点位置"], 0.5f).SetRelative(true))
            .Join(BossBack.transform.DOMove(InitValues["ボス背景"]["終点位置"], 0.5f).SetRelative(true))
            .OnComplete(() =>
            {
                DOTween.Sequence()
                .Append(TextBack.DOFillAmount(1.0f, 0.25f))
                .OnPlay(() => { tmp.transform.localPosition = InitTextPos; });

                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Opening); // 開幕音流用
                for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
                {
                    DOTween.Sequence()
                        .Append(tmpAnimator.DORotateChar(i, Vector3.zero, 0.55f));
                }
                DoCutIn.Kill();

                var DoOut = DOTween.Sequence();
                DoOut
                    .AppendInterval(1.5f)
                    .Append(BossImg.DOFade(0.0f, 0.2f))
                    .Join(BossBack.DOFade(0.0f,0.2f))
                    .Join(tmp.DOFade(0.0f, 0.2f))
                    .Join(TextBack.DOFade(0.0f, 0.2f))
                    .OnComplete(() => {
                        MoveCompleat = true;
                        DoOut.Kill();
                    });

            });
    }

    /// <summary>
    /// 動作が完了したかのフラグを返却
    /// </summary>
    /// <returns> 動作完了フラグ </returns>
    public static void ResetMoveComplete()
    { MoveCompleat = false; }
}
