using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FireworksModule))]
public class FireworksModuleEditor : Editor {
    private FireworksModule _target;

    public override void OnInspectorGUI()
    {
        _target = target as FireworksModule;

        base.OnInspectorGUI();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        switch (_target.Type) {
        case FireworksModule.FireworksType.Normal:
            _target._collisionObject = (GameObject)EditorGUILayout.ObjectField("Collision Object", _target.CollisionObject, typeof(GameObject), true);
            _target._blastAfterTime = EditorGUILayout.FloatField("爆発当たり判定の存在時間(秒)", _target.BlastAfterTime);
            break;
        case FireworksModule.FireworksType.Cracker:
            _target._circleComplementNum = EditorGUILayout.IntField("円の分割数", _target.CircleComplementNum);
            _target._blastAngle = EditorGUILayout.FloatField("破裂角度範囲(0～180度)", _target.BlastAngle);
            _target._blastDis = EditorGUILayout.FloatField("射程", _target.BlastDis);
            _target._modelDeleteTime = EditorGUILayout.FloatField("モデルの残留時間(秒)", _target.ModelDeleteTime);
            _target._isDrawArea = EditorGUILayout.ToggleLeft("範囲表示", _target.IsDrawArea);
            break;
        case FireworksModule.FireworksType.Hard:
            _target._collisionObject = (GameObject)EditorGUILayout.ObjectField("Collision Object", _target.CollisionObject, typeof(GameObject), true);
            _target._firstInvTime = EditorGUILayout.FloatField("一回目の被弾後無敵時間(秒)", _target.FirstInvTime);
            _target._blastAfterTime = EditorGUILayout.FloatField("爆発当たり判定の存在時間(秒)", _target.BlastAfterTime);
            _target._blastNum = EditorGUILayout.IntField("爆破回数", _target.BlastNum);
            break;
        case FireworksModule.FireworksType.Double:
            _target._collisionObject = (GameObject)EditorGUILayout.ObjectField("Collision Object", _target.CollisionObject, typeof(GameObject), true);
            _target._firstInvTime = EditorGUILayout.FloatField("一回目の被弾後無敵時間(秒)", _target.FirstInvTime);
            _target._secondAfterTime = EditorGUILayout.FloatField("2回目の当たり判定の存在時間(秒)", _target.SecondAfterTime);
            _target._multiBlast = (GameObject)EditorGUILayout.ObjectField("二重花火の二回目のエフェクト", _target.MultiBlast, typeof(GameObject), true);
            EditorGUILayout.LabelField("--- 色関連 ---");
            _target._barrierObj      = (GameObject)EditorGUILayout.ObjectField("二重花火用のバリア", _target.BarrierObj, typeof(GameObject),true);
            _target._barrierColor    = EditorGUILayout.ColorField("二重花火のバリアの色", _target.BarrierColor);
            _target._barrierParticleObj = (GameObject)EditorGUILayout.ObjectField("バリア破壊のエフェクト", _target.BarrierParticleObj, typeof(GameObject), true);
            _target._parentFireObj   = (GameObject)EditorGUILayout.ObjectField("親花火玉用のオブジェクト", _target.ParentFireObj, typeof(GameObject), true);
            _target._parentFireColor = EditorGUILayout.ColorField("親花火玉の色", _target.ParentFireColor);
            _target._childFireObj    = (GameObject)EditorGUILayout.ObjectField("子花火玉用のオブジェクト", _target.ChildFireObj, typeof(GameObject), true);
            _target._childFireColor  = EditorGUILayout.ColorField("子花火玉の色", _target.ChildFireColor);
            break;
        case FireworksModule.FireworksType.ResurrectionBox:
            _target._playerPrefab = (GameObject)EditorGUILayout.ObjectField("生成するオブジェクト", _target.PlayerPrefab, typeof(GameObject), true);
            _target._delayTime = EditorGUILayout.FloatField("生成までの待ち時間(秒)", _target.DelayTime);
            _target._animationTime = EditorGUILayout.FloatField("アニメーション時間(秒)", _target.AnimationTime);
            _target._animationDelayTime = EditorGUILayout.FloatField("アニメーションの遅延時間(秒)", _target.AnimationDelayTime);
            _target._boxDisTime = EditorGUILayout.FloatField("箱の消滅時間(秒)", _target.BoxDisTime);
            break;
        case FireworksModule.FireworksType.ResurrectionPlayer:
            _target._collisionObject = (GameObject)EditorGUILayout.ObjectField("Collision Object", _target.CollisionObject, typeof(GameObject), true);
            _target._invTime = EditorGUILayout.FloatField("無敵時間(秒)", _target.InvTime);
            break;
        case FireworksModule.FireworksType.Boss:
            _target._ignitionMax = EditorGUILayout.IntField("爆発に必要な回数", _target.IgnitionMax);
            _target._movieObject = (GameObject)EditorGUILayout.ObjectField("演出を管理しているオブジェクト", _target.MovieObject, typeof(GameObject), true);
            _target._outsideBarrier = (GameObject)EditorGUILayout.ObjectField("外側のバリア", _target.OutsideBarrier, typeof(GameObject), true);
            _target._outsideBarrierColor = EditorGUILayout.ColorField("外側のバリアの色", _target.OutsideBarrierColor);
            _target._outsideBarrierParticleObj = (GameObject)EditorGUILayout.ObjectField("外側のバリア破壊のエフェクト", _target.OutsideBarrierParticleObj, typeof(GameObject), true);
            _target._insideBarrier =  (GameObject)EditorGUILayout.ObjectField("内側のバリア", _target.InsideBarrier,  typeof(GameObject), true);
            _target._insideBarrierColor = EditorGUILayout.ColorField("内側のバリアの色", _target.InsideBarrierColor);
            _target._insideBarrierParticleObj = (GameObject)EditorGUILayout.ObjectField("内側のバリア破壊のエフェクト", _target.InsideBarrierParticleObj, typeof(GameObject), true);
            _target._boss1Obj1 = (GameObject)EditorGUILayout.ObjectField("変化させるボスオブジェクト1", _target.Boss1Obj1, typeof(GameObject), true);
            _target._boss1Obj2 = (GameObject)EditorGUILayout.ObjectField("変化させるボスオブジェクト2", _target.Boss1Obj2, typeof(GameObject), true);
            _target._bossIgniteColor = EditorGUILayout.ColorField("引火時の色", _target.BossIgniteColor);
            _target._fadeTime = EditorGUILayout.FloatField("色のフェード時間(秒)", _target.FadeTime);
            break;
        case FireworksModule.FireworksType.Dragonfly:
            _target._tonboSpeed =  EditorGUILayout.FloatField("スピード", _target.TonboSpeed);
            _target._effectTonbo = (GameObject)EditorGUILayout.ObjectField("トンボ花火のエフェクト", _target.EffectTonbo, typeof(GameObject), true);
            break;
        case FireworksModule.FireworksType.Yanagi:
            _target._modelResidueTime = EditorGUILayout.FloatField("モデル残留時間", _target.ModelResidueTime);
            EditorGUILayout.LabelField("--- 色関連 ---");
            _target._yanagiobj   = (GameObject)EditorGUILayout.ObjectField("柳花火用のオブジェクト", _target.YanagiObj, typeof(GameObject),true);
            _target._yanagiColor = EditorGUILayout.ColorField("柳花火の色", _target.YanagiColor);
            _target._reafobj1    = (GameObject)EditorGUILayout.ObjectField("葉っぱ用のオブジェクト1", _target.ReafObj1, typeof(GameObject), true);
            _target._reafColor1  = EditorGUILayout.ColorField("葉っぱの色1", _target.ReafColor1);
            _target._reafobj2    = (GameObject)EditorGUILayout.ObjectField("葉っぱ用のオブジェクト2", _target.ReafObj2, typeof(GameObject), true);
            _target._reafColor2  = EditorGUILayout.ColorField("葉っぱの色2", _target.ReafColor2);
            break;
        case FireworksModule.FireworksType.Boss2:
            _target._movieObject = (GameObject)EditorGUILayout.ObjectField("演出を管理しているオブジェクト", _target.MovieObject, typeof(GameObject), true);
            _target._synchroTime = EditorGUILayout.FloatField("同時被弾の猶予時間(秒)", _target.SynchroTime);
            _target._boss2barrierObj = (GameObject)EditorGUILayout.ObjectField("バリアオブジェクト", _target.Boss2BarrierObj, typeof(GameObject), true);
            _target._boss2barrierColor = EditorGUILayout.ColorField("バリアの色", _target.Boss2BarrierColor);
            _target._boss2barrierParticleObj = (GameObject)EditorGUILayout.ObjectField("バリア破壊のエフェクト", _target.Boss2BarrierParticleObj, typeof(GameObject), true);
            _target._boss2Obj1 = (GameObject)EditorGUILayout.ObjectField("変化させるボスオブジェクト1", _target.Boss2Obj1, typeof(GameObject), true);
            _target._boss2Obj2 = (GameObject)EditorGUILayout.ObjectField("変化させるボスオブジェクト2", _target.Boss2Obj2, typeof(GameObject), true);
            _target._bossIgniteColor = EditorGUILayout.ColorField("引火時の色", _target.BossIgniteColor);
            _target._fadeTime = EditorGUILayout.FloatField("色のフェード時間(秒)", _target.FadeTime);
            break;
        case FireworksModule.FireworksType.Boss3:
            _target._movieObject = (GameObject)EditorGUILayout.ObjectField("演出を管理しているオブジェクト", _target.MovieObject, typeof(GameObject), true);
            _target._boss3obj = (GameObject)EditorGUILayout.ObjectField("変化させるボスオブジェクト", _target.Boss3Obj, typeof(GameObject), true);
            _target._bossIgniteColor = EditorGUILayout.ColorField("引火時の色", _target.BossIgniteColor);
            _target._fadeTime = EditorGUILayout.FloatField("色のフェード時間(秒)", _target.FadeTime);
            break;
        case FireworksModule.FireworksType.Boss4:
            _target._movieObject = (GameObject)EditorGUILayout.ObjectField("演出を管理しているオブジェクト", _target.MovieObject, typeof(GameObject), true);
            _target._motionObject = (GameObject)EditorGUILayout.ObjectField("モーションするオブジェクト", _target.MotionObject, typeof(GameObject), true);
            _target._materialObject = (GameObject)EditorGUILayout.ObjectField("マテリアルがあるオブジェクト", _target.MaterialObject, typeof(GameObject), true);
            break;
        }
        //- インスペクターの更新
        if (GUI.changed)
        { EditorUtility.SetDirty(target); }
    }
}
