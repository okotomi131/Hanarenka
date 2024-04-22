/*
 ===================
 制作：大川
 開始時の演出を行うスクリプト
 ===================
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
#if UNITY_EDITOR
//- デプロイ時にEditorスクリプトが入るとエラー。UNITY_EDITORで括る
using UnityEditor;
#endif

public class OpeningAnime: MonoBehaviour
{
    [SerializeField] private Image TextBack;
    [SerializeField] private TextMeshProUGUI tmp;
    private bool InFirst = false;

    private Vector3 InitTextBackPos;
    private Vector3 InittmpPos;

    public static bool MoveCompleat = false;
    private void Awake()
    {
        //- 初期位置を保存
        InitTextBackPos = TextBack.transform.localPosition;
        InittmpPos = tmp.transform.localPosition;
        //- 開始時は外に移動
        TextBack.transform.localPosition = new Vector3(9999.0f, 9999.0f);
        tmp.transform.localPosition = new Vector3(9999.0f, 9999.0f);
        //- 変数初期化
        TextBack.fillAmount = 0;
    }

    /// <summary>
    /// 開始演出
    /// </summary>
    public void StartMove()
    {
        if(!InFirst)
        {
            InFirst = true;
            //- テキストアニメ用変数宣言
            DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(tmp);
            //- テキストを90度回転させる
            for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
            {   tmpAnimator.DORotateChar(i, Vector3.up * 90, 0);    }


            var In = DOTween.Sequence();

            In.AppendInterval(0.5f) //開始待機時間
               .OnPlay(() => {      //設置位置に移動
                TextBack.transform.localPosition = InitTextBackPos;
                tmp.transform.localPosition = InittmpPos;})
              .Append(TextBack.DOFillAmount(1.0f, 0.25f))   //背景出現
              .OnComplete(() => 
              {
                //- テキスト表示
                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Opening); // 開幕音再生
                for (int i = 0; i < tmpAnimator.textInfo.characterCount; i++)
                {DOTween.Sequence().Append(tmpAnimator.DORotateChar(i, Vector3.zero, 0.55f));}
                DOTween.To(() => tmp.characterSpacing, value => tmp.characterSpacing = value, 2.0f, 3.0f).SetEase(Ease.OutQuart);//拡大
                In.Kill();

                var Out = DOTween.Sequence();
                Out.AppendInterval(1.25f)                //撤収待機時間
                    .Append(TextBack.DOFade(0.0f, 0.2f)) //フェード
                    .Join(tmp.DOFade(0.0f, 0.2f))        //〃
                    .OnComplete(()=> {
                        MoveCompleat = true;
                    });
              });
        }
    }
   
    /// <summary>
    ///  描画フラグをリセットする
    /// </summary>
    public static void ResetMoveComplete()
    { MoveCompleat = false; }

    /*　◇ーーーーーー拡張コードーーーーーー◇　*/
#if UNITY_EDITOR
    //- Inspector拡張クラス
    [CustomEditor(typeof(OpeningAnime))] 
    public class TargetDescriptionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            OpeningAnime td = target as OpeningAnime;
            EditorGUI.BeginChangeCheck();
            td.TextBack
                = (Image)EditorGUILayout.ObjectField("動作する画像", td.TextBack, typeof(Image), true);
            td.tmp
                = (TextMeshProUGUI)EditorGUILayout.ObjectField("テキスト", td.tmp, typeof(TextMeshProUGUI), true);

            //- インスペクターの更新
            if (GUI.changed)
            { EditorUtility.SetDirty(target); }
        }
    }
        
#endif
}
