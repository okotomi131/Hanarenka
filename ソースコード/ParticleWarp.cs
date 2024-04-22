using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleWarp : MonoBehaviour
{
    ParticleSystem m_particleSystem;
    List<ParticleSystem.Particle> m_enterList = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> m_exitList = new List<ParticleSystem.Particle>();
    GameObject WarpA;
    GameObject WarpB;

    [SerializeField, Header("�N���A���Ƀ��[�v�����~")]
    private bool ClearStop = true;

    [SerializeField, Header("���ԉ�")]
    private bool IsYanagi = false;

    void Start()
    {
        // ===== �I�u�W�F�N�g�A�R���|�[�l���g�擾 =====
        m_particleSystem = this.GetComponent<ParticleSystem>();
        WarpA = GameObject.Find("WarpholeA");
        WarpB = GameObject.Find("WarpholeB");

        // ===== �g���K�[���W���[���̐ݒ� =====
        var trigger = m_particleSystem.trigger;
        if (WarpA)
        {
            trigger.enabled = true;
            trigger.inside = ParticleSystemOverlapAction.Ignore;
            trigger.outside = ParticleSystemOverlapAction.Ignore;
            trigger.enter = ParticleSystemOverlapAction.Callback;
            trigger.exit = ParticleSystemOverlapAction.Callback;
            trigger.radiusScale = 0.1f;
            trigger.AddCollider(WarpA.transform);
            trigger.AddCollider(WarpB.transform);
        }
    }

    void OnParticleTrigger()
    {
        // ===== �N���A��Ƀ��[�v�������s��Ȃ� =====
        if (SceneChange.bIsChange == true && ClearStop) return;

        // ===== �p�[�e�B�N������ԂŌ��� =====
        // �����Ɉ�v����p�[�e�B�N���� ParticleSystem ����擾����.
        int numEnter = m_particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, m_enterList);
        int numExit = m_particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, m_exitList);
        //- �g�k�擾
        Vector3 scale = transform.localScale;

        // ===== Particle : Exit =====
        for (int idx = 0; idx < numExit; idx++)
        {
            // ===== �p�[�e�B�N�����̎擾 =====
            ParticleSystem.Particle p = m_exitList[idx];
            Vector3 pos = p.position; //- ���W�擾
            pos = m_particleSystem.transform.TransformPoint(pos); //- ���[�J�����烏�[���h
            Vector3 rot = transform.eulerAngles; //- ��]�擾
            Vector2 dis = new Vector2(0, 0);

            // ===== ���[�v�����������闱�q���ǂ��� =====
            if (p.startColor.a == 230) continue;
            // ===== �g���C�����f�����߂̍�� ===== 
            p.startColor = new Color(1, 1, 1, 0.9f);
            pos.z -= 99999;

            // ===== ���[�v�z�[���̏��擾 =====
            Vector3 posA = WarpA.transform.position;
            Vector3 posB = WarpB.transform.position;
            float disA = Vector3.Distance(pos, posA);
            float disB = Vector3.Distance(pos, posB);

            // ===== ���[�v�̕����ŕ��� =====
            if (disA < disB)
            {
                // ===== ���[�vA�̉�]�𖳌��� =====
                //- ���[�v�z�[�����S����ڐG�n�_�܂ł̋������擾
                Vector3 HitdisA = pos - WarpA.transform.position;
                //- ���[�vA�̉�]�擾�A��]��ł�����
                Vector3 rotA = WarpA.transform.localEulerAngles;
                //- ��]���l�̒���
                if (rotA.x < 180) rotA.x = rotA.x - 360;
                //- ��]�̓K�p
                HitdisA = Quaternion.Euler(rotA.x, 180 - rotA.y, 0) * HitdisA;

                // ===== �o���ʒu�𔽑Α��̖ʂɂ��� =====
                //- �S�̓I�ȋ��������߂�
                Vector2 effectDis = posA - transform.position;
                if (gameObject.name == "Yanagi") effectDis = posA - pos;
                //- ��]�ʂ����߂đł������A�o���ʂ𔽓]������
                float rad = Mathf.Atan(effectDis.y / effectDis.x);
                HitdisA = Quaternion.Euler(0, 0, -rad * Mathf.Rad2Deg + 180) * HitdisA;
                //- �o���ʂ𔽓]
                HitdisA.y *= -1;
                //- ��]�ʂ𕜊�������
                HitdisA = Quaternion.Euler(0, 0, rad * Mathf.Rad2Deg) * HitdisA;

                // ===== ���[�vB�̈ړ���� =====
                //- ���[�vB�̉�]�擾�A��]��^����
                Vector3 rotB = WarpB.transform.eulerAngles;
                //- ��]���l�̒���
                if (rotB.x < 180) rotB.x = rotB.x - 360;
                //- ��]�̓K�p
                HitdisA = Quaternion.Euler(-rotB.x, 180 + rotB.y, 0) * HitdisA;

                // ===== ���q���ړ������� =====
                pos = posB + HitdisA;
            }
            else if (disA >= disB)
            {
                // ===== ���[�vA�̉�]�𖳌��� =====
                //- ���[�v�z�[�����S����ڐG�n�_�܂ł̋������擾
                Vector3 HitdisB = pos - WarpB.transform.position;
                //- ���[�vB�̉�]�擾�A��]��ł�����
                Vector3 rotB = WarpB.transform.localEulerAngles;
                //- ��]���l�̒���
                if (rotB.x < 180) rotB.x = rotB.x - 360;
                //- ��]�̓K�p
                HitdisB = Quaternion.Euler(rotB.x, 180 - rotB.y, 0) * HitdisB;
                // ===== �o���ʒu�𔽑Α��̖ʂɂ��� =====
                //- �S�̓I�ȋ��������߂�
                Vector2 effectDis = posB - transform.position;
                if (gameObject.name == "Yanagi") effectDis = posB - pos;
                //- ��]�ʂ����߂đł������A�o���ʂ𔽓]������
                float rad = Mathf.Atan(effectDis.y / effectDis.x);
                HitdisB = Quaternion.Euler(0, 0, -rad * Mathf.Rad2Deg + 180) * HitdisB;
                //- �o���ʂ𔽓]
                HitdisB.y *= -1;
                //- ��]�ʂ𕜊�������
                HitdisB = Quaternion.Euler(0, 0, rad * Mathf.Rad2Deg) * HitdisB;

                // ===== ���[�vB�̈ړ���� =====
                //- ���[�vA�̉�]�擾�A��]��^����
                Vector3 rotA = WarpA.transform.eulerAngles;
                //- ��]���l�̒���
                if (rotA.x < 180) rotA.x = rotA.x - 360;
                //- ��]�̓K�p
                HitdisB = Quaternion.Euler(-rotA.x, 180 + rotA.y, 0) * HitdisB;

                // ===== ���q���ړ������� =====
                pos = posA + HitdisB;
            }

            // ===== ���ԉΐ�p =====
            if (IsYanagi == true) pos.z = 0;
            // ===== �v�Z��̓K�p���� =====
            pos = m_particleSystem.transform.InverseTransformPoint(pos); //- ���[���h���烍�[�J��
            p.position = pos;         //- ���W�K�p
            m_exitList[idx] = p; //- �p�[�e�B�N���K�p
        }

        // ===== Particle : Enter =====
        for (int idx = 0; idx < numEnter; idx++)
        {
            // ===== �p�[�e�B�N�����̎擾 =====
            ParticleSystem.Particle p = m_enterList[idx];
            Vector3 pos = p.position; //- ���W�擾
            pos = m_particleSystem.transform.TransformPoint(pos); //- ���[�J�����烏�[���h
            // ===== ���[�v�����������闱�q���ǂ��� =====
            if (p.startColor.a == 230) continue;
            // ===== �g���C�����������߂̍�� ===== 
            p.startColor = new Color(0, 0, 0, 0);

            pos.z += 99999;
            // ===== �v�Z��̓K�p���� =====
            pos = m_particleSystem.transform.InverseTransformPoint(pos); //- ���[���h���烍�[�J��
            p.position = pos;         //- ���W�K�p
            m_enterList[idx] = p; //- �p�[�e�B�N���K�p
        }

        // ===== �ݒ�ύX��̃p�[�e�B�N����K�p ======
        m_particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, m_enterList);
        m_particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, m_exitList);
    }
}