using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class OptionGage : MonoBehaviour
{
    //[SerializeField, Header("�G���J�E���g���Ă���I�u�W�F�N�g")]
    //private GameObject CountObject;

    [SerializeField, Header("�I�v�V��������ɂ����鎞��(�b)")]
    private float OptionTime = 1.0f;
    [SerializeField, Header("�V�[���ړ����̃t�F�[�h����(�b)")]
    private float FadeTime = 1.0f;
    [SerializeField, Header("�Z���N�g�V�[��")]
    private SceneObject SelectScene;

    //- �{�^����������Ă��邩�ǂ���
    bool bIsPushRetry = false;
    bool bIsPushInGame = false;

    //- �{�^���������ꂽ����
    float bPushTimeRetry = 0;
    float bPushTimeInGame = 0;

    //- �C���[�W�̃R���|�[�l���g
    Image imageRetry;
    Image imageInGame;

    //- �V�[���ړ����n�܂������ǂ����̃t���O
    bool bIsStartRetry = false;
    bool bIsStartInGame = false;

    // CountEnemy�X�N���v�g������ϐ�
    CountEnemy countEnemy;

    //- �v���C���[�̏����擾����ϐ�
    PController player;

    //- �Q�[���p�b�h
    Gamepad gamepad;

    void Start()
    {
        //- �R���|�[�l���g�̎擾
        imageRetry = GameObject.Find("ResetGage").GetComponent<Image>();
        imageInGame = GameObject.Find("InGameGage").GetComponent<Image>();
        //- �G�J�E���gUI�̎擾
        countEnemy = GameObject.Find("Main Camera").GetComponent<CountEnemy>();

        //- �v���C���[�̏���
        player = GameObject.Find("Player").GetComponent<PController>();
        //- �Q�[���p�b�h�̎擾
        gamepad = Gamepad.current;
    }
    
    void FixedUpdate()
    {

        // ���݂̓G�̐����X�V
        int EnemyNum = countEnemy.GetCurrentCountNum();
        //- �V�[���ړ����n�܂������ǂ����̃t���O���擾
        bool bIsMoveScene = false;

        //- �V�[���ړ��̃t���O�X�V 
        if (bIsStartInGame || bIsStartRetry) bIsMoveScene = true;

        //�����������@���g���C�{�^���@����������
        //- �u�G���c���Ă���v�u�V�[���ړ��{�^����������Ă�v�u�V�[���ړ����n�܂��Ă��Ȃ��v
        //- ��L�����ׂĖ������Ώ�������
        if (bIsPushRetry && EnemyNum > 0 && !bIsMoveScene)
        {
            bPushTimeRetry += Time.deltaTime;                    //- �v�b�V�����Ԃ̍X�V
            imageRetry.fillAmount = bPushTimeRetry / OptionTime; //- �Q�[�W�̍X�V
        }
        else if (!bIsMoveScene)
        {
            bPushTimeRetry = 0;        //- �v�b�V�����Ԃ̃��Z�b�g
            imageRetry.fillAmount = 0; //- �Q�[�W�̃��Z�b�g
        }
        //- ��莞�Ԓ��������ꂽ�珈������
        if (bPushTimeRetry >= OptionTime)
        {
            if (bIsStartRetry == true) return; //- ���Z�b�g�J�n�t���O�������Ă���΃��^�[��
            imageInGame.fillAmount = 0; //- ���΂̃Q�[�W�̃��Z�b�g
            bIsStartRetry = true; //�V�[���J�n�t���O�����Ă�

            GameObject.Find("FadeImage").GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime); // �t�F�[�h�J�n
            if (gamepad != null) DOVirtual.DelayedCall(FadeTime, () => gamepad.SetMotorSpeeds(0.0f, 0.0f)); //- �U�����~�߂�
            DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name)); // �V�[���̃��[�h(�x������)
        }

        //- �V�[���ړ��̃t���O�X�V 
        if (bIsStartInGame || bIsStartRetry) bIsMoveScene = true;

        //�����������@�C���Q�[���{�^���@����������
        //- �u�G���c���Ă���v�u�V�[���ړ��{�^����������Ă�v�u�V�[���ړ����n�܂��Ă��Ȃ��v
        //- ��L�����ׂĖ������Ώ�������
        if (bIsPushInGame && EnemyNum > 0 && !bIsMoveScene)
        {
            bPushTimeInGame += Time.deltaTime;                    //- �v�b�V�����Ԃ̍X�V
            imageInGame.fillAmount = bPushTimeInGame / OptionTime; //- �Q�[�W�̍X�V
        }
        else if (!bIsMoveScene)
        {
            bPushTimeInGame = 0;        //- �v�b�V�����Ԃ̃��Z�b�g
            imageInGame.fillAmount = 0; //- �Q�[�W�̃��Z�b�g
        }
        //- ��莞�Ԓ��������ꂽ�珈������
        if (bPushTimeInGame >= OptionTime)
        {
            if (bIsStartInGame == true) return; //- ���Z�b�g�J�n�t���O�������Ă���΃��^�[��
            imageRetry.fillAmount = 0; //- ���΂̃Q�[�W�̃��Z�b�g
            bIsStartInGame = true; //�V�[���J�n�t���O�����Ă�

            GameObject.Find("FadeImage").GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime); // �t�F�[�h�J�n
            BGMManager bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
            if (gamepad != null) DOVirtual.DelayedCall(FadeTime, () => gamepad.SetMotorSpeeds(0.0f, 0.0f)); //- �U�����~�߂�
            DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager());
            DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(SelectScene)); // �V�[���̃��[�h(�x������)
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        //- �{�^����������Ă���ԁA�v���C���[������\���ɕϐ���ݒ�
        if (context.started && !player.GetWaitFlag()) { bIsPushRetry = true; }
        if (context.canceled && !player.GetWaitFlag()) { bIsPushRetry = false; }
    }

    public void OnInGame(InputAction.CallbackContext context)
    {
        //- �{�^����������Ă���ԁA�A�v���C���[������\���ɕϐ���ݒ�
        if (context.started && !player.GetWaitFlag()) { bIsPushInGame = true; }
        if (context.canceled && !player.GetWaitFlag()) { bIsPushInGame = false; }
    }
}
