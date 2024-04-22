using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ��Ր��� : ����
 * �ǋL : �q��A����
 * �T�v : �I�u�W�F�N�g�����[�v������X�N���v�g
 */
public class WarpGate : MonoBehaviour
{
    [SerializeField, Header("���[�v�Q�[�g")] private GameObject WarpHole;
    [SerializeField, Header("���[�v����ʒu")] private Transform warpPoint;
    [SerializeField, Header("���[�v�Q�[�g�͈̔�")] private float radius = 1.0f;

    private void OnTriggerStay(Collider other)
    {
        //- ���[�v�z�[���ɐڐG������w�肵���ʒu�Ƀ��[�v����(�v���C���[)
        if (other.gameObject.name == "Player")
        {
            //- �v���C���[�ƃ��[�v�Q�[�g�̋������v�Z
            Vector3 playerPos = other.transform.position;
            Vector3 gatePos   = transform.position;
            float distance = Vector3.Distance(playerPos, gatePos);
            //- ���[�v����o�Ă�������ƈʒu�����̂��߂ɓ��ˊp���擾
            Vector3 warpdistance = WarpHole.transform.position - other.transform.position;

            //- �v���C���[�ƃ��[�v�Q�[�g�̋������͈͓��Ȃ�
            if (distance < radius)
            {
                //- �R���g���[���[�̃R���|�[�l���g���擾
                var cc = other.gameObject.GetComponent<CharacterController>();
                cc.enabled = false; // �R���g���[���[�̔���𖳌��ɂ���
                //- �v���C���[���w�肵���ʒu�Ƀ��[�v������
                other.transform.position = warpPoint.position + warpdistance * 1.2f;
                Vector3 pos = other.transform.position;
                pos.z = 0.0f;
                other.transform.position = pos;
                cc.enabled = true; // �R���g���[���[�̔����L���ɂ���
            }
        }
        //- ���[�v�z�[���ɐڐG������w�肵���ʒu�Ƀ��[�v����(�����v���C���[)
        if (other.gameObject.name == "ResurrectionFireFlower(Clone)")
        {
            //- �����v���C���[�ƃ��[�v�Q�[�g�̋������v�Z
            Vector3 resPlayerPos = other.transform.position;
            Vector3 gatePos      = transform.position;
            float distance       = Vector3.Distance(resPlayerPos, gatePos);
            //- ���[�v����o�Ă�������ƈʒu�����̂��߂ɓ��ˊp���擾
            Vector3 warpdistance = WarpHole.transform.position - other.transform.position;

            //- �v���C���[�ƃ��[�v�Q�[�g�̋������͈͓��Ȃ�
            if (distance < radius)
            {
                var cc = other.gameObject.GetComponent<CharacterController>();
                cc.enabled = false; // �R���g���[���[�̔���𖳌��ɂ���
                //- �v���C���[���w�肵���ʒu�Ƀ��[�v������
                other.transform.position = warpPoint.position + warpdistance * 1.2f;
                Vector3 pos = other.transform.position;
                pos.z = 0.0f;
                other.transform.position = pos;
                cc.enabled = true; // �R���g���[���[�̔����L���ɂ���
            }
        }
        //- ���[�v�z�[���ɐڐG������w�肵���ʒu�Ƀ��[�v����(�g���{�ԉ�)
        if (other.gameObject.name == "DragonflyModule")
        {
            //- �g���{�ԉ΂ƃ��[�v�Q�[�g�̋������v�Z
            Vector3 DFPos   = other.transform.position;
            Vector3 gatePos = transform.position;
            float distance  = Vector3.Distance(DFPos, gatePos);
            //- ���[�v����o�Ă�������ƈʒu�����̂��߂ɓ��ˊp���擾
            Vector3 warpdistance = WarpHole.transform.position - other.transform.position;

            //- �v���C���[�ƃ��[�v�Q�[�g�̋������͈͓��Ȃ�
            if (distance < radius)
            {
                //- �v���C���[���w�肵���ʒu�Ƀ��[�v������
                other.transform.position = warpPoint.position + warpdistance;
                Vector3 pos = other.transform.position;
                pos.z = 0.0f;
                other.transform.position = pos;
            }
        }
    }
}