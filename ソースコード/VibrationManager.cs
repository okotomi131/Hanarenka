using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    //- �U���̋���
    float VibrationPower = 0;

    //- �U���̃t���[���J�E���g
    int nVibrationFrameCount = 0;

    [Header("���t���[����܂銄��(�p�[�Z���g)"), SerializeField]
    private float weakRatio = 3;

    [Header("�Œ���̐U���̋���"), SerializeField]
    private float minVibration = 0.6f;

    //- �Q�[���p�b�h�ڑ�����Ă��邩�ǂ���
    bool bIsSuccessConnection = true;

    //- �Q�[���p�b�h
    Gamepad gamepad;

    void Start()
    {
        //- �Q�[���p�b�h�ڑ�
        gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.Log("�Q�[���p�b�h���ڑ�");
            bIsSuccessConnection = false;
        }
    }

    void FixedUpdate()
    {
        //- ���ڑ��Ȃ���s���Ȃ�
        if (!bIsSuccessConnection) return;

        //- �U���t���[�����c���Ă��邩�ǂ����ŕ���
        if (nVibrationFrameCount > 0)
        {
            //- �U����ݒ�
            gamepad.SetMotorSpeeds(VibrationPower, VibrationPower);
        }
        else
        {
            //- �U���t���[�����I��������U�����I���
            gamepad.SetMotorSpeeds(0.0f, 0.0f);

        }

        //- �U���t���[���J�E���g��i�߂�
        nVibrationFrameCount--;

        //- �U������߂�
        VibrationPower -= VibrationPower * (weakRatio / 100);

        //- �Œ���̐U�����キ�Ȃ�Ȃ��悤�ɂ���
        if (minVibration > VibrationPower) { VibrationPower = minVibration; }

    }

    public void SetVibration(int nFrame, float power)
    {
        //- �U���̋����ݒ�
        VibrationPower = power;
        //- �U���̃t���[���������݂̃t���[������蒷��������X�V
        if (nVibrationFrameCount < nFrame) { nVibrationFrameCount = nFrame; }
    }
}
