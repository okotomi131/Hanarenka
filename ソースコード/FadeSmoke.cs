using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeSmoke : MonoBehaviour
{
    [SerializeField, Header("�x������(�b)")]
    private float DelayTime = 2.0f;
    [SerializeField, Header("�t�F�[�h����(�b)")]
    private float FadeTime = 2.0f;
    [SerializeField, Header("�t�F�[�h��̃A���t�@�l")]
    private float FadeAlpha = 0.0f;
    [SerializeField, Header("�t�F�[�h���̈ړ���")]
    private Vector3 Move;

    // Start is called before the first frame update
    void Start()
    {
        //- �C���[�W�̎擾
        Image image = GetComponent<Image>();
        //- �t�F�[�h
        image.DOFade(FadeAlpha, FadeTime).SetDelay(DelayTime);
        //- �ړ���̍��W
        Vector3 MovePos = transform.position + Move;
        //- ����
        transform.DOMove(MovePos, FadeTime).SetDelay(DelayTime);
    }
}
