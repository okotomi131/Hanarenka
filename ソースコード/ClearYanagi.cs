using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearYanagi : MonoBehaviour
{
    [SerializeField, Header("�Ήԗp�̃I�u�W�F�N�g")]
    private GameObject particle;
    [SerializeField, Header("�G�t�F�N�g�����x��(�b)")]
    private float startDelayTime;
    [SerializeField, Header("�G�t�F�N�g�̒���(�b)")]
    private float effectTime;

    private float countTime = 0;

    //- �A�N�e�B�u���Ɏ��s
    void OnEnable()
    {
        if(!SceneChange.bIsChange)
        { return; }

        //- �G�t�F�N�g�̐��������v�Z
        float maxEffect = effectTime / 0.1f;

        DOVirtual.DelayedCall(startDelayTime, //- �����x���̎��Ԍ�Ɏ��s
            () => StartCoroutine(MakeYanagiEffect(0.1f,(int)maxEffect)) //- �R���[�`�����X�^�[�g
            );

        //- �G�t�F�N�g���I����A���g���A�N�e�B�u��
        DOVirtual.DelayedCall(startDelayTime + effectTime, () => this.gameObject.SetActive(false));
    }

    //- ���ɒx���������Đ���
    private IEnumerator MakeYanagiEffect(float delayTime, int maxEffect)
    {
        //- �G�t�F�N�g�𐶐�
        for (int i = 0; i < maxEffect; i++)
        {
            //- delayTime�b�ҋ@����
            yield return new WaitForSeconds(delayTime);
            //- �G�t�F�N�g�����̂��߂ɁA���W���擾
            Vector3 pos = transform.position;
            pos.x = transform.position.x + 0.5f;
            //- �����ʒu�����炷
            pos.y += 1.0f;//1.6f;
            //- �w�肵���ʒu�ɐ���
            GameObject fire = Instantiate(particle, pos, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }
    }
}
