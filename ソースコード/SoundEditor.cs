using UnityEngine;
using UnityEditor;

/*
 ===================
 制作：髙橋
 概要：SEを編集するスクリプト
 ===================
 */
[CustomEditor(typeof(SEManager))]
public class SoundEditor : Editor
{
    //- SEManagerを取得する変数
    private SEManager _se;

    //- 初期状態ではインスペクターで展開されている状態
    private bool seFoldOut = true;

    public override void OnInspectorGUI()
    {
        _se = target as SEManager;

        base.OnInspectorGUI();

        //- フォールドアウト表示用のGUIコンポーネントを作成
        seFoldOut = EditorGUILayout.Foldout(seFoldOut, "===== SE関連 =====");

        //- 折り畳まれていない時に表示されるGUIコンポーネントを作成する
        if (seFoldOut)
        {
            EditorGUILayout.LabelField("--- 花火関連 ---");
            _se.explosion  = EditorGUILayout.ObjectField("爆発音", _se.explosion, typeof(AudioClip), false) as AudioClip;
            _se.yanagifire = EditorGUILayout.ObjectField("柳花火音", _se.yanagifire, typeof(AudioClip), false) as AudioClip;
            _se.tonbofire  = EditorGUILayout.ObjectField("トンボ花火音", _se.tonbofire, typeof(AudioClip), false) as AudioClip;
            _se.dragonfire = EditorGUILayout.ObjectField("ドラゴン花火音", _se.dragonfire, typeof(AudioClip), false) as AudioClip;
            _se.barrierdes = EditorGUILayout.ObjectField("バリア破壊音", _se.barrierdes, typeof(AudioClip), false) as AudioClip;
            _se.belt       = EditorGUILayout.ObjectField("打ち上げ音", _se.belt, typeof(AudioClip), false) as AudioClip;
            _se.bossbelt   = EditorGUILayout.ObjectField("ボス打ち上げ音", _se.bossbelt, typeof(AudioClip), false) as AudioClip;

            EditorGUILayout.LabelField("--- クラッカー関連 ---");
            _se.brust     = EditorGUILayout.ObjectField("破裂音", _se.brust, typeof(AudioClip), false) as AudioClip;
            _se.reservoir = EditorGUILayout.ObjectField("溜め音", _se.reservoir, typeof(AudioClip), false) as AudioClip;
            _se.ignition  = EditorGUILayout.ObjectField("着火音", _se.ignition, typeof(AudioClip), false) as AudioClip;

            EditorGUILayout.LabelField("--- 復活箱関連 ---");
            _se.generated  = EditorGUILayout.ObjectField("生成音", _se.generated, typeof(AudioClip), false) as AudioClip;
            _se.extinction = EditorGUILayout.ObjectField("消滅音", _se.extinction, typeof(AudioClip), false) as AudioClip;

            EditorGUILayout.LabelField("--- シーン関連 ---");
            _se.click   = EditorGUILayout.ObjectField("クリック音", _se.click, typeof(AudioClip), false) as AudioClip;
            _se.select  = EditorGUILayout.ObjectField("ボタン選択音", _se.select, typeof(AudioClip), false) as AudioClip;
            _se.clear   = EditorGUILayout.ObjectField("クリア音", _se.clear, typeof(AudioClip), false) as AudioClip;
            _se.failure = EditorGUILayout.ObjectField("失敗音", _se.failure, typeof(AudioClip), false) as AudioClip;
            _se.slide   = EditorGUILayout.ObjectField("スライド音",_se.slide, typeof(AudioClip),false) as AudioClip;

            EditorGUILayout.LabelField("--- 開幕演出関連 ---");
            _se.opening = EditorGUILayout.ObjectField("開幕音", _se.opening, typeof(AudioClip), false) as AudioClip;

            EditorGUILayout.LabelField("--- カットイン関連 ---");
            _se.letterapp = EditorGUILayout.ObjectField("文字出現音", _se.letterapp, typeof(AudioClip), false) as AudioClip;

            EditorGUILayout.LabelField("--- 設定項目 ---");
            _se.volume  = EditorGUILayout.Slider("Volume", _se.volume, 0f, 1f);
            _se.pitch   = EditorGUILayout.Slider("Pitch", _se.pitch, 0f, 1f);
        }
        //- インスペクターの更新
        if (GUI.changed)
        { EditorUtility.SetDirty(target); }
    }
}