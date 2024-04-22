using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatingMove : MonoBehaviour
{
    [SerializeField, Header("1�t���[���ɂ��X�V����p�x(360�x)")]
    private float updateAngle = 1.0f;
    [SerializeField, Header("�v���C���[�̉�]�ړ��̔��a")]
    private float radius = 1.0f;

    private bool MoveKill = false; //- �ړ���~�t���O
    private float nowAngle = 0;    //- ���݂̊p�x

    void Start()
    {       
        //- �v���C���[�̍��W�擾
        Vector3 pos = this.transform.position;

        //- �ŏ��ɉ�]�ړ��̒��S�ɗ���悤�ɁA���a���������ɂ��炷
        pos.y -= radius;

        //- �v���C���[�̍��W�K�p
        this.transform.position = pos;
    }

    void FixedUpdate()
    {
        if (MoveKill) return; //- �ړ���~�t���O��true�Ȃ烊�^�[��

        //- 1�t���[���O�̊p�x���������Ă���
        float oldAngle = nowAngle;
        //- ���݂̊p�x���X�V
        nowAngle += updateAngle;

        //- ���ꂼ���Y�̒l�����߂�
        float nowY = radius * Mathf.Sin(nowAngle * Mathf.Deg2Rad);
        float oldY = radius * Mathf.Sin(oldAngle * Mathf.Deg2Rad);
        
        //- �v���C���[�̍��W�擾
        Vector3 pos = this.transform.position;

        //- ���݂�1�t���[���O�̒l�̍������A�v���C���[���ړ�������
        pos.y += nowY - oldY;

        //- �v���C���[�̍��W�K�p
        this.transform.position = pos;
    }

    public void StopMove()
    {
        MoveKill = true;
    }
}
