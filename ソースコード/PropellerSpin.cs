using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpin : MonoBehaviour
{   
    //- �i�[�p�̃g�����X�t�H�[��
    Transform myTransform;

    [SerializeField, Header("1�b�ŉ�]�����(�x)")]
    private float SecondSpinSpeed = 3.0f;

    //- 1�t���[���̈ړ���
    private float FrameSpinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //- �g�����X�t�H�[�����擾
        myTransform = this.transform;
        //- 1�t���[���̈ړ��ʂ��v�Z
        FrameSpinSpeed = SecondSpinSpeed / 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //- ���[���h��]���擾
        Vector3 worldAngle = myTransform.eulerAngles;
        //- ���[���h�n��Y����]
        worldAngle.y += FrameSpinSpeed;
        //- ���[���h��]�𔽉f
        myTransform.eulerAngles = worldAngle;
    }
}
