using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGamePopUp : MonoBehaviour
{
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button NextButton;
    [SerializeField]
    private Button EndButton;

    private Button EnterButton;
    private Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        EnterButton = GameObject.Find("ButtonEnter").GetComponent<Button>();
        BackButton = GameObject.Find("ButtonBack").GetComponent<Button>();

        gameObject.SetActive(false);
    }

    // �|�b�v�A�b�v��\������
    public void PopUpOpen()
    {
        gameObject.SetActive(true); // �|�b�v�A�b�v�̃I�u�W�F�N�g��L����

        EnterButton.interactable = true;
        BackButton.interactable = true;

        // �C�[�W���O�ݒ�
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);

        // �|�b�v�A�b�v���̃{�^���ȊO�𖳌���
        StartButton.interactable = false;
        NextButton.interactable = false;
        EndButton.interactable = false;

        // �{�^���̏����I��
        gameObject.transform.Find("ButtonEnter").GetComponent<Button>().Select();
        return;
    }

    public void PopUpClose()
    {
        EnterButton.interactable = false;
        BackButton.interactable = false;

        // �C�[�W���O�ݒ�
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.DOScale(0.0f, 0.5f).SetEase(Ease.InCubic).OnComplete(() => { gameObject.SetActive(false); }); // �I�����I�u�W�F�N�g������

        // �|�b�v�A�b�v���̃{�^���ȊO��L����
        StartButton.interactable = true;
        NextButton.interactable = true;
        EndButton.interactable = true;

        // �j���[�Q�[���{�^���I��
        StartButton.GetComponent<Button>().Select();
        return;
    }
}
