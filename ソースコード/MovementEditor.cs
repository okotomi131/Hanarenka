using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 ===================
 ����F����
 �T�v�F�I�u�W�F�N�g�̋�����ҏW����X�N���v�g
 ===================
*/
[CustomEditor(typeof(MovementManager))]
public class MovementEditor : Editor
{
    //- MovementManager���擾����ϐ�
    private MovementManager _move;

    public override void OnInspectorGUI()
    {
        _move = target as MovementManager;

        base.OnInspectorGUI();

        //- ��s�󂯂� 
        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        switch (_move.Type)
        {
            case MovementManager.E_MovementType.ThreewayBehaviour:
                _move._moveDirection = (MovementManager.E_MoveDirection)EditorGUILayout.EnumPopup("�ړ�����", _move.MoveDirection);
                _move._moveDistance  = EditorGUILayout.FloatField("�ړ�����", _move.MoveDistance);
                _move._travelTime    = EditorGUILayout.FloatField("�ړ�����", _move.TravelTime);
                break;
            case MovementManager.E_MovementType.ThreepointBehaviour:
                _move._startPoint   = EditorGUILayout.Vector3Field("�n�_", _move.StartPoint);
                _move._halfwayPoint = EditorGUILayout.Vector3Field("���ԓ_", _move.HalfwayPoint);
                _move._endPoint     = EditorGUILayout.Vector3Field("�I�_", _move.EndPoint);
                _move._moveSpeed    = EditorGUILayout.FloatField("�ړ����x", _move.MoveSpeed);
                _move._endWaitTime  = EditorGUILayout.FloatField("�I�_���B���̑ҋ@����", _move.EndWaitTime);
                break;
            case MovementManager.E_MovementType.ThreepointWaitBehaviour:
                _move._startPoint   = EditorGUILayout.Vector3Field("�n�_", _move.StartPoint);
                _move._halfwayPoint = EditorGUILayout.Vector3Field("���ԓ_", _move.HalfwayPoint);
                _move._endPoint     = EditorGUILayout.Vector3Field("�I�_", _move.EndPoint);
                _move._moveSpeed    = EditorGUILayout.FloatField("�ړ����x", _move.MoveSpeed);
                _move._waitTime     = EditorGUILayout.FloatField("�e�|�C���g���B���̑ҋ@����", _move.WaitTime);
                break;
            case MovementManager.E_MovementType.CicrleBehaviour:
                _move._rotaDirection  = (MovementManager.E_RotaDirection)EditorGUILayout.EnumPopup("��]����",_move.RotaDirection);
                _move._center         = EditorGUILayout.Vector3Field("���S�_", _move.Center);
                _move._axis           = EditorGUILayout.Vector3Field("��]��", _move.Axis);
                _move._radius         = EditorGUILayout.FloatField("���a�̑傫��", _move.Radius);
                _move._periodTime     = EditorGUILayout.FloatField("������̂ɂ����鎞��(�b)", _move.PeriodTime);
                _move._updateRotation = EditorGUILayout.Toggle("�������X�V���邩���Ȃ���",_move._updateRotation);
                break;
            case MovementManager.E_MovementType.SmoothCircularBehaviour:
                _move._rotaDirection  = (MovementManager.E_RotaDirection)EditorGUILayout.EnumPopup("��]����", _move.RotaDirection);
                _move._center         = EditorGUILayout.Vector3Field("���S�_", _move.Center);
                _move._axis           = EditorGUILayout.Vector3Field("��]��", _move.Axis);
                _move._radius         = EditorGUILayout.FloatField("���a�̑傫��", _move.Radius);
                _move._startTime      = EditorGUILayout.FloatField("�J�n���ɂ��炷����(�b)", _move.StartTime);
                _move._periodTime     = EditorGUILayout.FloatField("������̂ɂ����鎞��(�b)", _move.PeriodTime);
                _move._updateRotation = EditorGUILayout.Toggle("�������X�V���邩���Ȃ���", _move._updateRotation);
                break;
        }
        //- �C���X�y�N�^�[�̍X�V
        if (GUI.changed)
        { EditorUtility.SetDirty(target); }
    }
}