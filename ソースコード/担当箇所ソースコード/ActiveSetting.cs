/*
 ===================
 制作：大川
 アクティブ状態を管理するスクリプト
 ===================
 */

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//- 設定事にオブジェクトのアクティブを管理するクラス
public class ActiveSetting : MonoBehaviour
{
    /*  列挙体宣言部  */
    //- アクティブにするかしないか
    private enum E_ACTIVE_OPTION
    {
        [InspectorName("非アクティブ→アクティブ")]
        NoActiveToActive,
        [InspectorName("アクティブ→非アクティブ")]
        ActiveToNoActive,
    }

    //- アクティブになる順番
    private enum E_STARTUP_SETTING
    {
        [InspectorName("同時")]
        AllAtOnce,
        [InspectorName("リストの上から")]
        FromTop,
    }
    

    /*  変数宣言部  */
    [SerializeField,Header("アクティブ管理するオブジェクト")] List<GameObject> ActiveObjs;   //アクティブを管理するオブジェクトリスト
    
    [HideInInspector, SerializeField] private float FirstDirayTime = 0.0f;   //リストの初めのオブジェクトがアクティブになるまでの時間
    [HideInInspector, SerializeField] private float NextDirayTime = 0.0f;    //次のリストのオブジェクトがアクティブになるまでの時間
    [HideInInspector, SerializeField] private E_STARTUP_SETTING ActiveState = E_STARTUP_SETTING.AllAtOnce;   //アクティブになる順番
    [HideInInspector, SerializeField] private E_ACTIVE_OPTION Option = E_ACTIVE_OPTION.NoActiveToActive;     //アクティブ状態の設定
    [HideInInspector, SerializeField] private bool AutoClear = false;   //クリア時に自動で処理するかのフラグ

    private bool Active = false;        
    private bool ListFirstActive = false;
    private bool SetFlag = false;
    private float CurrentTime = 0.0f;
    private int cnt = 0;

    private void Start()
    {
        //- 開始時のアクティブ状態を設定する
        switch (Option)
        {
            case E_ACTIVE_OPTION.NoActiveToActive:
                //- リストにあるオブジェクトを全て非アクティブにする
                foreach (GameObject obj in ActiveObjs)
                { obj.SetActive(false); }
                SetFlag = true;
                break;

            case E_ACTIVE_OPTION.ActiveToNoActive:
                //- 開始時にはアクティブのまま
                SetFlag = false;
                break;
        }
    }

    private void Update()
    {
        //- 自動クリアフラグが立っていなければ処理しない
        if(!AutoClear)
        {   return; }

        //- クリア時に自動で行う
        if (SceneChange.bIsChange && !Active)
        {
            switch (ActiveState)
            {
            //- 同時に
            case E_STARTUP_SETTING.AllAtOnce:
                AllAtOnce();
                break;
            //- リストの上から
            case E_STARTUP_SETTING.FromTop:
                FromTop();
                break;
            }
        }
    }
    
    /// <summary>
    /// リストの中身を一斉にアクティブ管理する
    /// </summary>
    public void AllAtOnce()
    {
        CurrentTime += Time.deltaTime;
        //- 遅延時間が経過したか
        if (CurrentTime >= FirstDirayTime)
        {
            foreach (GameObject obj in ActiveObjs)
            { obj.SetActive(SetFlag); }
            Active = true;
        }
    }

    /// <summary>
    /// リストの昇順にアクティブ管理を行う
    /// </summary>
    public void FromTop()
    {
        CurrentTime += Time.deltaTime;
        //- 初めの読み込み遅延時間を経過したか、リストの初めを読み込んだか
        if (CurrentTime >= FirstDirayTime && !ListFirstActive)
        {
            //- 配列はじめを設定
            ActiveObjs[0].SetActive(SetFlag);
            //- カウント増加
            cnt++;
            //- 時間リセット
            CurrentTime = 0.0f;
            //- リストの初めを読み込んだ
            ListFirstActive = true;
        }
        else if (CurrentTime >= NextDirayTime && //遅延時間経過
                cnt < ActiveObjs.Count &&        //要素数を超えていないか
                ListFirstActive)                 //読み込み済ではないか
        {
            ActiveObjs[cnt].SetActive(SetFlag);
            cnt++;
            CurrentTime = 0.0f;
            //- 全てのオブジェクトが0になったら以降処理しない
            if (cnt == ActiveObjs.Count)
            {
                Active = true;
                cnt = 0;
            }
        }
    }
    

    /*　◇ーーーーーー拡張コードーーーーーー◇　*/
#if UNITY_EDITOR
    //- Inspector拡張クラス
    [CustomEditor(typeof(ActiveSetting))]
    public class ActiveSettingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            {
                EditorGUI.BeginChangeCheck();
                ActiveSetting active = target as ActiveSetting;
                /*　◇ーーーカスタム表示ーーー◇　*/
                active.AutoClear =
                    EditorGUILayout.Toggle("クリア時自動で処理", active.AutoClear);
                active.Option =
                    (ActiveSetting.E_ACTIVE_OPTION)
                    EditorGUILayout.EnumPopup("アクティブ設定", active.Option);
                active.ActiveState =
                    (ActiveSetting.E_STARTUP_SETTING)
                    EditorGUILayout.EnumPopup("処理順", active.ActiveState);
                active.FirstDirayTime = 
                    EditorGUILayout.FloatField(
                        "初めのオブジェクトがアクティブになるまでの遅延時間", active.FirstDirayTime);
                active.NextDirayTime =
                    EditorGUILayout.FloatField(
                        "次のオブジェクトがアクティブになるまでの遅延時間", active.NextDirayTime);
                EditorGUILayout.EndFoldoutHeaderGroup();
                //- インスペクターの更新
                if (GUI.changed)
                { EditorUtility.SetDirty(target); }
            }
        }
    }
#endif  //UNITY_EDITOR
}
