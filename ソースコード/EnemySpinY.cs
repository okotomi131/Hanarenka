using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinY : MonoBehaviour
{
    //- �i�[�p�̃g�����X�t�H�[��
    Transform myTransform;

    [SerializeField, Header("1�b�ŉ�]�����(�x)")]
    private float SecondSpinSpeed = 3.0f;

    //- 1�t���[���̈ړ���
    private float FrameSpinSpeed;

    //- �ԉ΃X�N���v�g
    FireworksModule fireworks;

    // Start is called before the first frame update
    void Start()
    {
        //- �g�����X�t�H�[�����擾
        myTransform = this.transform;
        //- 1�t���[���̈ړ��ʂ��v�Z
        FrameSpinSpeed = SecondSpinSpeed / 60;
        //- �ԉΓ_�΃X�N���v�g�̎擾
        fireworks = this.gameObject.GetComponent<FireworksModule>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //- ���j���A��]�X�N���v�g�𖳌���
        if (fireworks.IsExploded)
        {
            this.gameObject.GetComponent<EnemySpinY>().enabled = false;
        }

        //- ���[���h��]���擾
        Vector3 worldAngle = myTransform.eulerAngles;
        //- ���[���h�n��Z����]
        worldAngle.z += FrameSpinSpeed;
        //- ���[���h��]�𔽉f
        myTransform.eulerAngles = worldAngle;
    }
}
