using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerQuickSpin : MonoBehaviour
{
    [SerializeField, Header("�p�x��S�Ď��񂵂����ɂ����鎞��(�b)")]
    private float GoAroundTime;

    [SerializeField, Header("���݊p�x�ƖڕW�p�x�̍���⊮���銄��(�p�[�Z���g)")]
    private float CompensatePercent;

    [SerializeField, Header("���񂷂�p�x(�x) & ���񂷂鐔")]
    private float[] LookAngleNum;


    //- ���݂��z�񉽔Ԗڂ̊p�x��ڕW�Ƃ��Ă��邩
    int nNowLookNum = 0;

    //- �t���[���J�E���g�p�A��������0�Ń��Z�b�g
    int nFrameCount = 0;

    //- ����ɂ�����t���[����
    int nGoAroundFrame;

    //- �ԉΓ_�΃X�N���v�g
    FireworksModule fireworks;

    void Start()
    {
        //- ����ɂ�����t���[������ݒ�
        nGoAroundFrame = (int)GoAroundTime * 60;
        //- �ԉΓ_�΃X�N���v�g�̎擾
        fireworks = this.gameObject.GetComponent<FireworksModule>();
    }

    void FixedUpdate()
    {
        //- ���j���A��]�X�N���v�g�𖳌���
        if (fireworks.IsExploded)
        {
            enabled = false;
        }

        //- ���݂��z�񉽔Ԗڂ̊p�x��ڎw���Ă��邩���߂�
        nNowLookNum = (nFrameCount / (nGoAroundFrame / LookAngleNum.Length));

        //- transform���擾
        Transform myTransform = this.transform;
        //- ���[���h���W����ɁA��]���擾
        Vector3 worldAngle = myTransform.eulerAngles;
        //- ���[���h���W����ɂ����Ay�������ɂ�����]�p�x�̕⊮����
        worldAngle.z += (LookAngleNum[nNowLookNum] - worldAngle.z) * (CompensatePercent / 100.0f);
        //- ��]�̓K�p
        this.transform.eulerAngles = worldAngle;

        //- �t���[���J�E���g��i�߂�
        nFrameCount++;
        if (nFrameCount >= nGoAroundFrame)
        {
            nFrameCount = 0;
        }
    }
}
