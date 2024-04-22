/*
 ===================
 ��Ր���F���
 �^�C�g���ɖ߂邩���m�F����|�b�v�A�b�v��\������X�N���v�g
 ===================
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VillagePop : MonoBehaviour
{
    [SerializeField] private List<Button> sato;
    [SerializeField] private DefaultSelectStage Dss;
    private Button EnterButton;
    private Button BackButton;
    private bool Open = false;
    private bool Close = false;
    void Start()
    {
        EnterButton = GameObject.Find("ButtonEnter").GetComponent<Button>();
        BackButton = GameObject.Find("ButtonBack").GetComponent<Button>();
        gameObject.SetActive(false);
    }

    // �|�b�v�A�b�v��\������
    public void PopUpOpen()
    {
        //- ���łɊJ����Ă����珈�����Ȃ�
        if (Open) { return; }
        Open = true;


        gameObject.SetActive(true); // �|�b�v�A�b�v�̃I�u�W�F�N�g��L����

        EnterButton.interactable = true;
        BackButton.interactable = true;

        // �C�[�W���O�ݒ�
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.DOScale(1.0f, 0.35f).SetEase(Ease.OutBack).OnComplete(() => {Close = false;});


        //- ����I���ł��Ȃ��悤�ɂ���
        for (int i = 0; i < sato.Count; i++)
        { sato[i].interactable = false; }

        // �{�^���̏����I��
        gameObject.transform.Find("ButtonEnter").GetComponent<Button>().Select();
        return;
    }

    public void PopUpClose()
    {
        //- ���łɕ����Ă����珈�����Ȃ�
        if (Close) { return; }
        Close = true;

        EnterButton.interactable = false;
        BackButton.interactable = false;

        // �C�[�W���O�ݒ�
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.DOScale(0.0f, 0.35f).SetEase(Ease.InCubic).OnComplete(() => {
            Open = false;
            gameObject.SetActive(false); }); // �I�����I�u�W�F�N�g������


        //- ����I���ł��Ȃ��悤�ɂ���
        for (int i = 0; i < sato.Count; i++)
        { sato[i].interactable = true; }

        //- �|�b�v�O�ɑI�𒆂������{�^����I����Ԃɂ���
        sato[Dss.GetSelectNum()].Select();

        return;
    }
}
