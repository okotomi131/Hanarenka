using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPlayer : MonoBehaviour
{
    [SerializeField, Header("�ŏ�����ҋ@��Ԃ��ǂ���")]
    private bool bIsWaitInit = true;
    [SerializeField, Header("��莞�Ԍ�A����\�ɂ��邩")]
    private bool bIsDelayWait = true;
    [SerializeField, Header("����\�ɂȂ�܂ł̎���(�b)")]
    private float MaxWaitTime = 1.5f;

    private bool bIsWaitNow;            //- �v���C���[���ҋ@��Ԃ��ǂ���
    private float WaitTimeCount = 0.0f; //- �^�C���J�E���^
    private PController pCnt;           //- �v���C���[����X�N���v�g
    
    void Start()
    {
        //- �R���|�[�l���g�̎擾
        pCnt = gameObject.GetComponent<PController>();
        //- �R���|�[�l���g�̃A�N�e�B�u�ݒ�
        pCnt.SetWaitFlag(bIsWaitInit);
        bIsWaitNow = bIsWaitInit;
    }

    void FixedUpdate()
    {
        if (!bIsDelayWait) return; //- �A�N�e�B�u�t���O�x���ݒ���������Ȃ����^�[��
        if (!bIsWaitNow)   return; //- �v���C���[���ҋ@��ԂłȂ��Ȃ烊�^�[��

        //- ���ԍX�V
        WaitTimeCount += Time.deltaTime;
        //- �ҋ@���Ԃ��߂��Ă��Ȃ���΃��^�[��
        if (WaitTimeCount < MaxWaitTime) return;

        //- �v���C���[�𑀍�\�ɕύX
        pCnt.SetWaitFlag(false);
        bIsWaitNow = false;
    }

    //- �v���C���[���ҋ@��Ԃ��ǂ����̃A�N�e�B�u�؂�ւ����s���֐�
    public void SetWaitPlayer(bool Flag)
    {
        pCnt.SetWaitFlag(Flag);
    }
}