using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JoyFace : MonoBehaviour
{
    [SerializeField, Header("�\������(�b)")]
    private float Time = 1.5f;

    [SerializeField, Header("�����x����ԒႢ���̃A���t�@�l(1.0�`0.0)")]
    private float PeakAlpha = 0.7f;

    void Start()
    {
        //- �}�e���A���̎擾
        Material material = GetComponent<Renderer>().material;
        //- �}�e���A���𓧖��ɂ���
        material.color = new Color32(255, 255, 255, 0);

        //=== �A�j���[�V���� ===
        //- �����Â傫���Ȃ�
        transform.DOScale(new Vector3(4, 4, 1), Time).SetEase(Ease.OutQuint);
        //- �ɂ��ݏo�Ă���
        material.DOFade(PeakAlpha, Time / 2);
        //- ������
        material.DOFade(0.0F, Time / 2).SetDelay(Time / 2);
    }
}
