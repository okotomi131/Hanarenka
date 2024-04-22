// =============================================
// StoryFlip.cs
//
// �쐬�F���
// �ǋL�F����
//
// �X�g�[���[�V�[���̉��o�p�X�N���v�g
// =============================================

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryFlip : MonoBehaviour
{
    [SerializeField]
    private GameObject[] flips;     // �X�g�[���[�̃C���X�g
    [SerializeField, Header("�t�F�[�h�b��")]
    private float FadeTime;
    [SerializeField] private float WaitTime = 0.0f;      //���ŋ����΂���܂őҋ@���鎞��

    private GameObject enterButton; // ���Z���N�g��ʂ֑J�ڂ���{�^���\����UI
    private Image fade;             // �t�F�[�h�p
    private BGMManager bgmManager;  // BGM�p     

    private int NowFlipNum = 0;     // ���݂̃C���X�g�̔ԍ�
    private bool isFlip = false;    // �C���X�g�������Ă邩�ǂ���
    private bool bPermissionClickSE = true; // �N���b�NSE�̍Đ���������Ă��邩
    private float CurrentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ----- �I�u�W�F�N�g�E�R���|�[�l���g�̎擾�A�����ݒ�
        // �V�[���J�ڃ{�^���p
        enterButton = GameObject.Find("EnterButton");
        //enterButton.SetActive(false);
        // �t�F�[�h
        fade = GameObject.Find("FadeImage").GetComponent<Image>();
        fade.DOFade(0.0f, 1.0f);        // �V�[���J�n���Ƀt�F�[�h�C������
        // ��
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();

        // ----- �X�g�[���[����̏����ݒ菈��
        gameObject.transform.DetachChildren(); // �ŏ��ɐe�q�֌W����������
        // 1���ڂ̃C���X�g
        flips[0].transform.parent = gameObject.transform;               // �e�q�֌W�̐ݒ�
        RectTransform trans0 = flips[0].GetComponent<RectTransform>();  // UI�p�g�����X�t�H�[���̎擾
        trans0.localPosition = Vector3.zero;                            // �C���X�g�̏����ʒu�ݒ�
        // �Q���ڈȍ~�̃C���X�g
        for (int i = 1; i < flips.Length; i++) {
            flips[i].transform.parent = gameObject.transform;
            RectTransform trans = flips[i].GetComponent<RectTransform>();
            trans.localPosition = flips[i - 1].GetComponent<RectTransform>().localPosition + new Vector3(trans.sizeDelta.x, 0.0f, 0.0f);
        }
    }

    private void Update()
    {
        CurrentTime += Time.deltaTime;

        // ----- �V�[���J�ڃ{�^���C���X�g�̗L���E�����؂�ւ�
        if (NowFlipNum >= flips.Length - 1 && !isFlip)
        { // �Ō�̃C���X�g�Ɉڂ�ς������ŁA�C���X�g���L���ɂȂ��Ă��Ȃ��Ƃ�
            //enterButton.SetActive(true);
        }
        else if (NowFlipNum < flips.Length - 1)
        { // �Ō�̃C���X�g�ȊO�̂Ƃ�
            //enterButton.SetActive(false);
        }
    }

    // ===================================================
    // �V�[���J�ڃL�[����������
    //
    // ���Z���N�g��ʂɑJ�ڂ���
    // ===================================================
    public void OnEnter(InputAction.CallbackContext context)
    {
        if (NowFlipNum >= flips.Length - 1 && !isFlip)
        { // �Ō�̃C���X�g�Ɉڂ�ς������
            //SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);
            DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager());
            fade.DOFade(1.0f, 1.5f).OnComplete(() => { SceneManager.LoadScene("1_Village"); });
        }
    }

    // ===================================================
    // ���̃C���X�g�L�[����������
    //
    // ���̃C���X�g�Ɉڂ�ς��
    // ===================================================
    public void OnNext(InputAction.CallbackContext context)
    {
        if (NowFlipNum < flips.Length - 1 && !isFlip)
        { // �C���X�g���Ō�ł͂Ȃ��A�ړ����ł͂Ȃ��Ƃ�
            isFlip = true;
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Slide);
            for (int i = 0; i < flips.Length; i++) {
                RectTransform trans = flips[i].GetComponent<RectTransform>();
                trans.DOMoveX(trans.position.x - 15.0f, 1.5f).SetEase(Ease.InOutCubic).OnComplete(() => { isFlip = false; });
            }
            NowFlipNum++;
        }
    }

    // ===================================================
    // �O�̃C���X�g�L�[����������
    //
    // �O�̃C���X�g�Ɉڂ�ς��
    // ===================================================
    public void OnPrev(InputAction.CallbackContext context)
    {
        if (NowFlipNum > 0 && !isFlip)
        { // �C���X�g���ŏ��ł͂Ȃ��A�ړ����ł͂Ȃ��Ƃ�
            isFlip = true;
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Slide);
            for (int i = 0; i < flips.Length; i++) {
                RectTransform trans = flips[i].GetComponent<RectTransform>();
                trans.DOMoveX(trans.position.x + 15.0f, 1.5f).SetEase(Ease.InOutCubic).OnComplete(() => { isFlip = false; });
            }
            NowFlipNum--;
        }
    }

    // ===================================================
    // BackSpace�L�[����������
    //
    // ���̃V�[���ֈڍs����
    // ===================================================
    public void OnClose(InputAction.CallbackContext context)
    {
        //- �ҋ@���Ԃ𒴂��Ă��Ȃ���Δ�΂��Ȃ�
        if(CurrentTime < WaitTime)
        { return; }

        if (NowFlipNum >= flips.Length - 1 && !isFlip)
        { // �Ō�̃C���X�g�Ɉڂ�ς������
            //SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);
            DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager());
            fade.DOFade(1.0f, 1.5f).OnComplete(() => { SceneManager.LoadScene("3_GameEnd"); });
        }
    }

    public float GetCurrentTime()
    {   return CurrentTime; }
}
