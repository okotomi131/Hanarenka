/*
 ===================
 ����F���
 �J�n���̉��o���s���X�N���v�g
 ===================
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
#if UNITY_EDITOR
//- �f�v���C����Editor�X�N���v�g������ƃG���[�BUNITY_EDITOR�Ŋ���
using UnityEditor;
#endif

public class OpeningAnime: MonoBehaviour
{
    [SerializeField] private Image TextBack;
    [SerializeField] private TextMeshProUGUI tmp;
    private bool InFirst = false;

    private Vector3 InitTextBackPos;
    private Vector3 InittmpPos;

    public static bool MoveCompleat = false;
    private void Awake()
    {
        //- �����ʒu��ۑ�
        InitTextBackPos = TextBack.transform.localPosition;
        InittmpPos = tmp.transform.localPosition;
        //- �J�n���͊O�Ɉړ�
        TextBack.transform.localPosition = new Vector3(9999.0f, 9999.0f);
        tmp.transform.localPosition = new Vector3(9999.0f, 9999.0f);
        //- �ϐ�������
        TextBack.fillAmount = 0;
    }

    /// <summary>
    /// �J�n���o
    /// </summary>
    public void StartMove()
    {
        if(!InFirst)
        {
            InFirst = true;
            //- �e�L�X�g�A�j���p�ϐ��錾
            DOTweenTMPAnimator tmpAnimator = new DOTweenTMPAnimator(tmp);
            //- �e�L�X�g��90�x��]������
            for (int i = 0; i < tmpAnimator.textInfo.characterCount; ++i)
            {   tmpAnimator.DORotateChar(i, Vector3.up * 90, 0);    }


            var In = DOTween.Sequence();

            In.AppendInterval(0.5f) //�J�n�ҋ@����
               .OnPlay(() => {      //�ݒu�ʒu�Ɉړ�
                TextBack.transform.localPosition = InitTextBackPos;
                tmp.transform.localPosition = InittmpPos;})
              .Append(TextBack.DOFillAmount(1.0f, 0.25f))   //�w�i�o��
              .OnComplete(() => 
              {
                //- �e�L�X�g�\��
                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Opening); // �J�����Đ�
                for (int i = 0; i < tmpAnimator.textInfo.characterCount; i++)
                {DOTween.Sequence().Append(tmpAnimator.DORotateChar(i, Vector3.zero, 0.55f));}
                DOTween.To(() => tmp.characterSpacing, value => tmp.characterSpacing = value, 2.0f, 3.0f).SetEase(Ease.OutQuart);//�g��
                In.Kill();

                var Out = DOTween.Sequence();
                Out.AppendInterval(1.25f)                //�P���ҋ@����
                    .Append(TextBack.DOFade(0.0f, 0.2f)) //�t�F�[�h
                    .Join(tmp.DOFade(0.0f, 0.2f))        //�V
                    .OnComplete(()=> {
                        MoveCompleat = true;
                    });
              });
        }
    }
   
    /// <summary>
    ///  �`��t���O�����Z�b�g����
    /// </summary>
    public static void ResetMoveComplete()
    { MoveCompleat = false; }

    /*�@���[�[�[�[�[�[�g���R�[�h�[�[�[�[�[�[���@*/
#if UNITY_EDITOR
    //- Inspector�g���N���X
    [CustomEditor(typeof(OpeningAnime))] 
    public class TargetDescriptionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            OpeningAnime td = target as OpeningAnime;
            EditorGUI.BeginChangeCheck();
            td.TextBack
                = (Image)EditorGUILayout.ObjectField("���삷��摜", td.TextBack, typeof(Image), true);
            td.tmp
                = (TextMeshProUGUI)EditorGUILayout.ObjectField("�e�L�X�g", td.tmp, typeof(TextMeshProUGUI), true);

            //- �C���X�y�N�^�[�̍X�V
            if (GUI.changed)
            { EditorUtility.SetDirty(target); }
        }
    }
        
#endif
}
