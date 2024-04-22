using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private bool IsOnce = false;

    private void OnParticleCollision(GameObject other)
    {
        // ���������I�u�W�F�N�g�̃^�O���uFireworks�v�Ȃ�
        if (other.gameObject.tag == "Fireworks")
        {
            // �����蔻��p�̃I�u�W�F�N�g����������
            if (other.gameObject.transform.GetChild(0).name == "Collision")
            {
                //- �����蔻���L��������
                // ���������I�u�W�F�N�g��Collider��L���ɂ���
                other.gameObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                // �����蔻��̊g��p�R���|�[�l���g��L���ɂ���
                other.gameObject.transform.GetChild(0).GetComponent<DetonationCollision>().enabled = true;
                //- �ړ��X�N���v�g������Ώ���
                if (other.gameObject.GetComponent<MovementManager>())
                    other.gameObject.GetComponent<MovementManager>().SetStopFrag(true);
            }
            // ���������I�u�W�F�N�g�̔����t���O�𗧂Ă�
            other.gameObject.GetComponent<FireworksModule>().Ignition(transform.position);
        }
        // ���������I�u�W�F�N�g�̃^�O���uResurrectionBox�v�Ȃ�
        if (other.gameObject.tag == "ResurrectionBox")
        {
            // ���������I�u�W�F�N�g�̔����t���O�𗧂Ă� 
            other.gameObject.GetComponent<FireworksModule>().Ignition(transform.position);
        }
    }
}