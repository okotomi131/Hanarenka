using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 ===================
 制作：髙橋
 概要：オブジェクトの挙動を編集するスクリプト
 ===================
*/
[CustomEditor(typeof(MovementManager))]
public class MovementEditor : Editor
{
    //- MovementManagerを取得する変数
    private MovementManager _move;

    public override void OnInspectorGUI()
    {
        _move = target as MovementManager;

        base.OnInspectorGUI();

        //- 一行空ける 
        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        switch (_move.Type)
        {
            case MovementManager.E_MovementType.ThreewayBehaviour:
                _move._moveDirection = (MovementManager.E_MoveDirection)EditorGUILayout.EnumPopup("移動方向", _move.MoveDirection);
                _move._moveDistance  = EditorGUILayout.FloatField("移動距離", _move.MoveDistance);
                _move._travelTime    = EditorGUILayout.FloatField("移動時間", _move.TravelTime);
                break;
            case MovementManager.E_MovementType.ThreepointBehaviour:
                _move._startPoint   = EditorGUILayout.Vector3Field("始点", _move.StartPoint);
                _move._halfwayPoint = EditorGUILayout.Vector3Field("中間点", _move.HalfwayPoint);
                _move._endPoint     = EditorGUILayout.Vector3Field("終点", _move.EndPoint);
                _move._moveSpeed    = EditorGUILayout.FloatField("移動速度", _move.MoveSpeed);
                _move._endWaitTime  = EditorGUILayout.FloatField("終点到達時の待機時間", _move.EndWaitTime);
                break;
            case MovementManager.E_MovementType.ThreepointWaitBehaviour:
                _move._startPoint   = EditorGUILayout.Vector3Field("始点", _move.StartPoint);
                _move._halfwayPoint = EditorGUILayout.Vector3Field("中間点", _move.HalfwayPoint);
                _move._endPoint     = EditorGUILayout.Vector3Field("終点", _move.EndPoint);
                _move._moveSpeed    = EditorGUILayout.FloatField("移動速度", _move.MoveSpeed);
                _move._waitTime     = EditorGUILayout.FloatField("各ポイント到達時の待機時間", _move.WaitTime);
                break;
            case MovementManager.E_MovementType.CicrleBehaviour:
                _move._rotaDirection  = (MovementManager.E_RotaDirection)EditorGUILayout.EnumPopup("回転方向",_move.RotaDirection);
                _move._center         = EditorGUILayout.Vector3Field("中心点", _move.Center);
                _move._axis           = EditorGUILayout.Vector3Field("回転軸", _move.Axis);
                _move._radius         = EditorGUILayout.FloatField("半径の大きさ", _move.Radius);
                _move._periodTime     = EditorGUILayout.FloatField("一周回るのにかかる時間(秒)", _move.PeriodTime);
                _move._updateRotation = EditorGUILayout.Toggle("向きを更新するかしないか",_move._updateRotation);
                break;
            case MovementManager.E_MovementType.SmoothCircularBehaviour:
                _move._rotaDirection  = (MovementManager.E_RotaDirection)EditorGUILayout.EnumPopup("回転方向", _move.RotaDirection);
                _move._center         = EditorGUILayout.Vector3Field("中心点", _move.Center);
                _move._axis           = EditorGUILayout.Vector3Field("回転軸", _move.Axis);
                _move._radius         = EditorGUILayout.FloatField("半径の大きさ", _move.Radius);
                _move._startTime      = EditorGUILayout.FloatField("開始時にずらす時間(秒)", _move.StartTime);
                _move._periodTime     = EditorGUILayout.FloatField("一周回るのにかかる時間(秒)", _move.PeriodTime);
                _move._updateRotation = EditorGUILayout.Toggle("向きを更新するかしないか", _move._updateRotation);
                break;
        }
        //- インスペクターの更新
        if (GUI.changed)
        { EditorUtility.SetDirty(target); }
    }
}