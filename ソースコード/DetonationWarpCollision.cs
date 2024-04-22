using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonationWarpCollision : MonoBehaviour
{
    public Vector3 RayStartPos = new Vector3(0, 0, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Fireworks") return;
        float markdis = 2.0f;
        Vector3 markpos = RayStartPos;
        Debug.DrawRay(markpos, new Vector3(+markdis, +markdis, 0), Color.blue, 3.0f);
        Debug.DrawRay(markpos, new Vector3(+markdis, -markdis, 0), Color.blue, 3.0f);
        Debug.DrawRay(markpos, new Vector3(-markdis, +markdis, 0), Color.blue, 3.0f);
        Debug.DrawRay(markpos, new Vector3(-markdis, -markdis, 0), Color.blue, 3.0f);
        CheckHitRayStage(other.gameObject);
    }

    private void CheckHitRayStage(GameObject obj)
    {
        // ���g����ԉ΂Ɍ������������v�Z
        Vector3 direction = obj.transform.position - RayStartPos;
        // ���g�Ɖԉ΂̒������v�Z
        float DisLength = direction.magnitude;
        // ���g����ԉ΂Ɍ��������C���쐬
        Ray ray = new Ray(RayStartPos, direction);
        // ���������I�u�W�F�N�g���i�[���邽�߂̕ϐ�
        var HitList = new List<RaycastHit>();

        // ���C�����������I�u�W�F�N�g�����ׂď��ԂɊm�F���Ă���
        foreach (RaycastHit hit in Physics.RaycastAll(ray, DisLength))
        {
            //- �ŏ��̃I�u�W�F�N�g�Ȃ疳�����Ŋi�[
            if (HitList.Count == 0)
            {
                HitList.Add(hit);
                continue;
            }

            //- �i�[�t���O
            bool bAdd = false;
            //- �i�[�ϐ��Ɠ��������I�u�W�F�N�g�̔�r
            for (int i = 0; i < HitList.Count; i++)
            {
                //- �i�[�t���O�`�F�b�N
                if (bAdd) break;
                //- �������i�[�ӏ��f�[�^�̋�����蒷����΃��^�[��
                if (HitList[i].distance < hit.distance) continue;
                //- ���̃f�[�^����ԍŌ�Ɋi�[
                HitList.Add(new RaycastHit());
                //- �Ōォ��i�[�ꏊ�܂Ńf�[�^�����炷
                for (int j = HitList.Count - 1; j > i; j--)
                {
                    //- �f�[�^����ړ�
                    HitList[j] = HitList[j - 1];
                }
                //- �i�[�ꏊ�Ɋi�[
                HitList[i] = hit;
                bAdd = true;
            }

            //- �i�[�t���O�������Ă��Ȃ���΁A��ԋ����������I�u�W�F�N�g�Ȃ̂�
            //- �z��̈�ԍŌ�Ɋi�[����
            if (!bAdd) HitList.Add(hit);
        }
        bool WarpCheck = false;
        //- �������Z�����̂��璲�ׂ�
        for (int i = 0; i < HitList.Count; i++)
        {
            RaycastHit hit = HitList[i];

            //- �����蔻��̃f�o�b�O�\��
             float markdis = 0.1f;
             Debug.DrawRay(transform.position, direction, Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(+markdis, +markdis, 0), Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(+markdis, -markdis, 0), Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(-markdis, +markdis, 0), Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(-markdis, -markdis, 0), Color.green, 3.0f);
            if (hit.collider.gameObject.tag != "Warphole") continue; //- �X�e�[�W�I�u�W�F�N�g�ȊO�Ȃ玟��
            if (hit.distance > DisLength) continue;               //- �ԉ΋ʂ��X�e�[�W�I�u�W�F�N�g�����ɂ���Ύ���

            WarpCheck = true;
        }
        if (!WarpCheck) return;
        //- ���������I�u�W�F�N�g��FireworksModule�̎擾
        FireworksModule module = obj.GetComponent<FireworksModule>();
        //- ���������I�u�W�F�N�g�̉ԉ΃^�C�v�ɂ���ď����𕪊�
        if (module.Type == FireworksModule.FireworksType.Boss)
            module.IgnitionBoss(obj);
        else if (module.Type != FireworksModule.FireworksType.ResurrectionPlayer)
            module.Ignition(transform.position);
        else if (module.Type == FireworksModule.FireworksType.ResurrectionPlayer)
            if (module.GetIsInv() == false)
            { module.Ignition(transform.position); }

        // �����蔻��p�̃I�u�W�F�N�g���������珈��
        if (obj.transform.GetChild(0).name == "Collision")
        {
            //- �ړ��X�N���v�g������Ώ���
            if (obj.GetComponent<MovementManager>())
                obj.GetComponent<MovementManager>().SetStopFrag(true);
        }

        //- �X�e�[�W�I�u�W�F�N�g�ɓ������Ă��Ȃ�
        return;
    }
}
