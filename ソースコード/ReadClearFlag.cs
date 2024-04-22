/*
 ===================
 ����F���
 UI�A�j���[�V�����̍쓮�^�C�~���O���Ǘ�����X�N���v�g
 ===================
 */

using UnityEngine;

//- �N���A��Ԃ�ǂ�
public class ReadClearFlag : MonoBehaviour
{
    private enum E_ACITEVESETTING
    {
        [InspectorName("�N���A�ŃA�N�e�B�u")]    ToActive,
        [InspectorName("�N���A�Ŕ�A�N�e�B�u")]  ToNoActive,
    }

    [SerializeField, Header("�X�e�[�W��")]  �@    private int stagenum = -1;
    [SerializeField, Header("����������")]      private GameObject obj;
    [SerializeField, Header("�A�N�e�B�u�ݒ�")]    private E_ACITEVESETTING Active = E_ACITEVESETTING.ToActive;

    private SaveManager save;  
    private bool read = false;  //�ǂݍ��݃t���O
    private bool first = false; //���񏈗��t���O

    void Awake()
    {   
        //- �A�N�e�B�u�ݒ育�Ƃɏ�����ύX����
        switch (Active)
        {
            case E_ACITEVESETTING.ToActive:     //�N���A���ɃA�N�e�B�u
                //- ���߂ēǂݍ��ނ�
                if (!first)
                {
                    save = FindObjectOfType<SaveManager>();
                    //- �N���A���Ă��Ȃ���
                    if (!save.GetStageClear(stagenum))
                    {
                        //- �N���A���Ă��Ȃ���Δ�A�N�e�B�u�ɂ���
                        obj.SetActive(false);
                    }
                    else if (save.GetStageClear(stagenum))
                    {
                        //- �N���A�ςł���΃A�N�e�B�u�ɂ���
                        obj.SetActive(true);
                        //- �A�N�e�B�u�������̂ł���ȏ�ǂݍ��ݏ������s��Ȃ��悤�ɂ���
                        read = true;
                    }
                    //- ����ǂݍ��݂��I��
                    first = true;
                }

                //- �N���A���Ă���A�܂��ǂݍ��ݏ��������Ă��Ȃ�
                if (!read && save.GetStageClear(stagenum))
                {
                    //- �{�^�����A�N�e�B�u�ɂ���
                    obj.SetActive(true);
                    //- �ǂݍ��݃t���O�ύX
                    read = true;
                }
                break;
            case E_ACITEVESETTING.ToNoActive:   //�N���A���ɔ�A�N�e�B�u
                //- ���߂ēǂݍ��ނ�
                if (!first)
                {
                    save = FindObjectOfType<SaveManager>();
                    //- �N���A���Ă��Ȃ���
                    if (!save.GetStageClear(stagenum))
                    {
                        //- �N���A���Ă��Ȃ���΃A�N�e�B�u�ɂ���
                        obj.SetActive(true);
                    }
                    else if (save.GetStageClear(stagenum))
                    {
                        //- �N���A�ςł���Δ�A�N�e�B�u�ɂ���
                        obj.SetActive(false);
                        //- ��A�N�e�B�u�������̂ł���ȏ�ǂݍ��ݏ������s��Ȃ��悤�ɂ���
                        read = true;
                    }
                    //- ����ǂݍ��݂��I��
                    first = true;
                }

                //- �N���A���Ă���A�܂��ǂݍ��ݏ��������Ă��Ȃ�
                if (!read && save.GetStageClear(stagenum))
                {
                    //- �{�^�����A�N�e�B�u�ɂ���
                    obj.SetActive(false);
                    //- �ǂݍ��݃t���O�ύX
                    read = true;
                }
                break;
        }
    }
}
