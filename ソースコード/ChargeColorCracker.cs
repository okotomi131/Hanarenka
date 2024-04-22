using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChargeColorCracker : MonoBehaviour
{
    [SerializeField, Header("�ω�������Q�[���I�u�W�F�N�g")]
    private GameObject obj;
    [SerializeField, Header("�擾����}�e���A���ԍ�")]
    private int MatNum = 0;
    [SerializeField, Header("�ʏ펞�̃J���[")]
    private Color DefaultColor;
    [SerializeField, Header("�`���[�W���̃J���[")]
    private Color ChargeColor;
    [SerializeField, Header("�F�̃t�F�[�h����(�b)")]
    private float FadeTime = 0.5f;
    [SerializeField, Header("�`���[�W���̐F�̎���(�b)")]
    private float ChargeTime = 0.5f;

    void Start()
    {
        //- �}�e���A���̎擾
        Material material = obj.GetComponent<Renderer>().materials[MatNum];
        //- �ʏ펞�̐F�ɕύX
        material.DOColor(DefaultColor, 0.0f);
        //- �`���[�W�F�̃t�F�[�h����
        material.DOColor(ChargeColor, FadeTime);
        //- �ʏ�F�ɖ߂�
        //material.DOColor(DefaultColor, FadeTime).SetDelay(FadeTime + ChargeTime);
    }
}