using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingCracker : MonoBehaviour
{
    [SerializeField, Header("�p�[�e�B�N���I�u�W�F�N�g")]
    public GameObject _particleObject; //- �p�[�e�B�N���I�u�W�F�N�g 
    [SerializeField, Header("���Ŏ���")]
    public float _destroyTime; //- �p�[�e�B�N���I�u�W�F�N�g 

    void Start()
    {
        //- ���Ή��Đ�
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Ignition);       
        //- �N���b�J�[�̃G�t�F�N�g����
        GameObject fire = Instantiate(
            _particleObject,                                            // ����(�R�s�[)����Ώ�
            transform.position,                                         // ���������ʒu
            Quaternion.Euler(0.0f, 0.0f, transform.localEulerAngles.z)  // �ŏ��ɂǂꂾ����]���邩
            );
        //- ���g��j�󂷂�
        Destroy(this.gameObject, _destroyTime);
        //- �j�􉹂̍Đ�
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Brust);
        //- �U���̐ݒ�
        //VibrationManager vibration = GameObject.Find("VibrationManager").GetComponent<VibrationManager>();
        //vibration.SetVibration(30, 1.0f);
    }
}
