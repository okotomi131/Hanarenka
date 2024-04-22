/*
 ===================
 基盤制作：大川
 追記：髙橋・牧野
 ボタン選択・非選択・押下時にアニメーションするスクリプト
 ===================
 */


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif


//- ボタンアニメーションクラス
public class ButtonAnime : MonoBehaviour,
    ISelectHandler,     //選択時
    IDeselectHandler,   //非選択時
    ISubmitHandler      //押下時
{

    [SerializeField] private Image image;            //ボタン押下時に動作する画像
    [SerializeField] private TextMeshProUGUI tmp;    //ボタン押下時に動作するテキスト
    [SerializeField] private Color OverTextColor;    //ボタン押下時にテキストを変色させる用
    [SerializeField] private float SelectTime = 0.25f;       //選択アニメが完了するまでの時間
    private Color BaseTextColor;                     //元色

    public bool bPermissionSelectSE = true;          // 選択SEの再生が許可されているか
    private Button button;                           
    private Vector2 BaseSize;                        //配置サイズ取得変数
    private bool bPush = false;                      //入力判定


    void Awake()
    {
        //- 動作画像があるか
        if (image == null)
        { return; }

        //- 動作用変数初期化
        button = GetComponent<Button>();
        image.fillAmount = 0;
        BaseTextColor = tmp.color;
    }


    /// <summary>
    /// ボタン選択時の処理
    /// </summary>
    /// <param name="eventData"></param>

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        //-選択音再生
        if (bPermissionSelectSE)
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Select);
        else
            bPermissionSelectSE = true;

        if (image == null)
        { return; }

        image.DOFillAmount(1.0f, SelectTime).SetEase(Ease.OutCubic).Play();  
        tmp.DOColor(OverTextColor, 0.25f).Play();
    }


    /// <summary>
    /// ボタン非選択時の処理
    /// </summary>
    /// <param name="eventData"></param>

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if (image == null)
        { return; }

        //- 連打対策フラグが立っていなければ処理
        if (!bPush)
        {
            image.DOFillAmount(0.0f, SelectTime).SetEase(Ease.OutCubic).Play();
            tmp.DOColor(BaseTextColor, 0.25f).Play();
        }
    }


    /// <summary>
    /// ボタン押下時の処理 
    /// </summary>
    /// <param name="eventData"></param>

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        //- 選択音再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);
    }

    /// <summary>
    /// ボタンが押下時にアニメーション処理を行う
    /// </summary>

    public void PushButtonAnime()
    {   image.DOColor(new Color(1.0f,0.5f,0.5f), 0.25f);    }


    /// <summary>
    /// 押下時の意図しない挙動を防ぐフラグ関数
    /// </summary>
    /// <param name="flag"></param>

    public void SetbSelect(bool flag)
    { bPush = flag; }

    /*　◇ーーーーーー拡張コードーーーーーー◇　*/
#if UNITY_EDITOR
    //- Inspector拡張クラス
    [CustomEditor(typeof(ButtonAnime))] //必須
    public class ButtonAnimeEditor : Editor //Editorの継承
    {
        public override void OnInspectorGUI()
        {
            ButtonAnime btnAnm = target as ButtonAnime;
            EditorGUI.BeginChangeCheck();
            btnAnm.image 
                = (Image)EditorGUILayout.ObjectField("動作する画像",btnAnm.image,typeof(Image),true);
            btnAnm.tmp
                = (TextMeshProUGUI)EditorGUILayout.ObjectField("テキスト", btnAnm.tmp, typeof(TextMeshProUGUI), true);
            btnAnm.OverTextColor
                = EditorGUILayout.ColorField("カラー", btnAnm.OverTextColor);
            btnAnm.SelectTime
                = EditorGUILayout.FloatField("アニメ完了までの時間", btnAnm.SelectTime);
            
            //- インスペクターの更新
            if(GUI.changed)
            {   EditorUtility.SetDirty(target); }
        }
    }
#endif

}
