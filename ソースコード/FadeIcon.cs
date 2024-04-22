using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*
 ===================
 ����F����
 �T�v�FMovie�V�[���̗��A�C�R���̋����X�N���v�g
 ===================
 */

public class FadeIcon : MonoBehaviour
{
    [SerializeField, Header("�x������(�b)")] private float DelayTime = 2.0f;
    [SerializeField, Header("�t�F�[�h����(�b)")] private float FadeTime = 2.0f;
    [SerializeField, Header("�t�F�[�h��̃A���t�@�l")] private float FadeAlpha = 0.0f;
    [SerializeField, Header("�t�F�[�h�O�̃A�C�R��")] private Image preFadeIcon;
    [SerializeField, Header("�t�F�[�h��̃A�C�R��")] private Image postFadeIcon;

    private void Start()
    {
        //- �t�F�[�h�O�̃A�C�R����\��
        preFadeIcon.gameObject.SetActive(true);
        //- �t�F�[�h��̃A�C�R�����\���ɂ���
        postFadeIcon.gameObject.SetActive(false);
        //- �t�F�[�h�O�̃A�C�R����������Ԃɂ���
        preFadeIcon.canvasRenderer.SetAlpha(1.0f);

        //=== �t�F�[�h�O�̃A�C�R�������X�Ƀt�F�[�h�A�E�g������ ===
        preFadeIcon.DOFade(FadeAlpha, FadeTime) // �w�肵��Alpha�̒l�ɂȂ�܂ł̃t�F�[�h����
        .SetDelay(DelayTime) // �t�F�[�h���n�܂�܂ł̒x������
        .OnComplete(() =>
        {
            // === �t�F�[�h������������A�t�F�[�h��̃A�C�R����\�����ăt�F�[�h�C�������� ===
            //- �t�F�[�h�������������_�ŁA�t�F�[�h�O�̃A�C�R���̓t�F�[�h�A�E�g���I����Ă��邽�߁A��\����
            preFadeIcon.gameObject.SetActive(false);
            //- �t�F�[�h��̃A�C�R���̓t�F�[�h�C��������O�Ȃ̂ŁA�\��
            postFadeIcon.gameObject.SetActive(true);
            //- �t�F�[�h��̃A�C�R���̃A���t�@�l��0�ɐݒ�
            postFadeIcon.canvasRenderer.SetAlpha(0.0f);
            //- �t�F�[�h��̃A�C�R�����w�肳�ꂽ���ԂŃt�F�[�h�C��������
            postFadeIcon.CrossFadeAlpha(1.0f, FadeTime, false);
        });
    }
}
