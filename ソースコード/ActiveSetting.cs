/*
 ===================
 ����F���
 �A�N�e�B�u��Ԃ��Ǘ�����X�N���v�g
 ===================
 */

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//- �ݒ莖�ɃI�u�W�F�N�g�̃A�N�e�B�u���Ǘ�����N���X
public class ActiveSetting : MonoBehaviour
{
    /*  �񋓑̐錾��  */
    //- �A�N�e�B�u�ɂ��邩���Ȃ���
    private enum E_ACTIVE_OPTION
    {
        [InspectorName("��A�N�e�B�u���A�N�e�B�u")]
        NoActiveToActive,
        [InspectorName("�A�N�e�B�u����A�N�e�B�u")]
        ActiveToNoActive,
    }

    //- �A�N�e�B�u�ɂȂ鏇��
    private enum E_STARTUP_SETTING
    {
        [InspectorName("����")]
        AllAtOnce,
        [InspectorName("���X�g�̏ォ��")]
        FromTop,
    }
    

    /*  �ϐ��錾��  */
    [SerializeField,Header("�A�N�e�B�u�Ǘ�����I�u�W�F�N�g")] List<GameObject> ActiveObjs;   //�A�N�e�B�u���Ǘ�����I�u�W�F�N�g���X�g
    
    [HideInInspector, SerializeField] private float FirstDirayTime = 0.0f;   //���X�g�̏��߂̃I�u�W�F�N�g���A�N�e�B�u�ɂȂ�܂ł̎���
    [HideInInspector, SerializeField] private float NextDirayTime = 0.0f;    //���̃��X�g�̃I�u�W�F�N�g���A�N�e�B�u�ɂȂ�܂ł̎���
    [HideInInspector, SerializeField] private E_STARTUP_SETTING ActiveState = E_STARTUP_SETTING.AllAtOnce;   //�A�N�e�B�u�ɂȂ鏇��
    [HideInInspector, SerializeField] private E_ACTIVE_OPTION Option = E_ACTIVE_OPTION.NoActiveToActive;     //�A�N�e�B�u��Ԃ̐ݒ�
    [HideInInspector, SerializeField] private bool AutoClear = false;   //�N���A���Ɏ����ŏ������邩�̃t���O

    private bool Active = false;        
    private bool ListFirstActive = false;
    private bool SetFlag = false;
    private float CurrentTime = 0.0f;
    private int cnt = 0;

    private void Start()
    {
        //- �J�n���̃A�N�e�B�u��Ԃ�ݒ肷��
        switch (Option)
        {
            case E_ACTIVE_OPTION.NoActiveToActive:
                //- ���X�g�ɂ���I�u�W�F�N�g��S�Ĕ�A�N�e�B�u�ɂ���
                foreach (GameObject obj in ActiveObjs)
                { obj.SetActive(false); }
                SetFlag = true;
                break;

            case E_ACTIVE_OPTION.ActiveToNoActive:
                //- �J�n���ɂ̓A�N�e�B�u�̂܂�
                SetFlag = false;
                break;
        }
    }

    private void Update()
    {
        //- �����N���A�t���O�������Ă��Ȃ���Ώ������Ȃ�
        if(!AutoClear)
        {   return; }

        //- �N���A���Ɏ����ōs��
        if (SceneChange.bIsChange && !Active)
        {
            switch (ActiveState)
            {
            //- ������
            case E_STARTUP_SETTING.AllAtOnce:
                AllAtOnce();
                break;
            //- ���X�g�̏ォ��
            case E_STARTUP_SETTING.FromTop:
                FromTop();
                break;
            }
        }
    }
    
    /// <summary>
    /// ���X�g�̒��g����ĂɃA�N�e�B�u�Ǘ�����
    /// </summary>
    public void AllAtOnce()
    {
        CurrentTime += Time.deltaTime;
        //- �x�����Ԃ��o�߂�����
        if (CurrentTime >= FirstDirayTime)
        {
            foreach (GameObject obj in ActiveObjs)
            { obj.SetActive(SetFlag); }
            Active = true;
        }
    }

    /// <summary>
    /// ���X�g�̏����ɃA�N�e�B�u�Ǘ����s��
    /// </summary>
    public void FromTop()
    {
        CurrentTime += Time.deltaTime;
        //- ���߂̓ǂݍ��ݒx�����Ԃ��o�߂������A���X�g�̏��߂�ǂݍ��񂾂�
        if (CurrentTime >= FirstDirayTime && !ListFirstActive)
        {
            //- �z��͂��߂�ݒ�
            ActiveObjs[0].SetActive(SetFlag);
            //- �J�E���g����
            cnt++;
            //- ���ԃ��Z�b�g
            CurrentTime = 0.0f;
            //- ���X�g�̏��߂�ǂݍ���
            ListFirstActive = true;
        }
        else if (CurrentTime >= NextDirayTime && //�x�����Ԍo��
                cnt < ActiveObjs.Count &&        //�v�f���𒴂��Ă��Ȃ���
                ListFirstActive)                 //�ǂݍ��ݍςł͂Ȃ���
        {
            ActiveObjs[cnt].SetActive(SetFlag);
            cnt++;
            CurrentTime = 0.0f;
            //- �S�ẴI�u�W�F�N�g��0�ɂȂ�����ȍ~�������Ȃ�
            if (cnt == ActiveObjs.Count)
            {
                Active = true;
                cnt = 0;
            }
        }
    }
    

    /*�@���[�[�[�[�[�[�g���R�[�h�[�[�[�[�[�[���@*/
#if UNITY_EDITOR
    //- Inspector�g���N���X
    [CustomEditor(typeof(ActiveSetting))]
    public class ActiveSettingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            {
                EditorGUI.BeginChangeCheck();
                ActiveSetting active = target as ActiveSetting;
                /*�@���[�[�[�J�X�^���\���[�[�[���@*/
                active.AutoClear =
                    EditorGUILayout.Toggle("�N���A�������ŏ���", active.AutoClear);
                active.Option =
                    (ActiveSetting.E_ACTIVE_OPTION)
                    EditorGUILayout.EnumPopup("�A�N�e�B�u�ݒ�", active.Option);
                active.ActiveState =
                    (ActiveSetting.E_STARTUP_SETTING)
                    EditorGUILayout.EnumPopup("������", active.ActiveState);
                active.FirstDirayTime = 
                    EditorGUILayout.FloatField(
                        "���߂̃I�u�W�F�N�g���A�N�e�B�u�ɂȂ�܂ł̒x������", active.FirstDirayTime);
                active.NextDirayTime =
                    EditorGUILayout.FloatField(
                        "���̃I�u�W�F�N�g���A�N�e�B�u�ɂȂ�܂ł̒x������", active.NextDirayTime);
                EditorGUILayout.EndFoldoutHeaderGroup();
                //- �C���X�y�N�^�[�̍X�V
                if (GUI.changed)
                { EditorUtility.SetDirty(target); }
            }
        }
    }
#endif  //UNITY_EDITOR
}
