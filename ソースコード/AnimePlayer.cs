using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/*
 ===================
 ����F����
 �T�v�F�����̃v���C���[����
 ===================
*/
public class AnimePlayer : MonoBehaviour
{
    [SerializeField, Header("�X�P�[���ύX�I�u�W�F�N�g")] private GameObject scaleObj;
    [SerializeField, Header("�x������(�b)")] private float delayTime;
    [SerializeField, Header("�k��ł�������(�b)")] private float shrinkTime;

    public void SetAnime()
    {
        //=== �v���C���[�����X�ɏk�߂Ă����A�j���[�V���� ===
        scaleObj.transform.DOScale(Vector3.zero, shrinkTime) // �X�P�[����0�ɂȂ�܂ł̏k���A�j���[�V��������
        .SetDelay(delayTime) // �A�j���[�V��������������܂ł̒x������
        .OnComplete(() =>
        {
            //- �k���A�j���[�V�������I��������I�u�W�F�N�g���\���ɂ���
            scaleObj.SetActive(false);
        });
    }
}