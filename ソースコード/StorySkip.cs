
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StorySkip : MonoBehaviour
{
    [SerializeField] private Image Skip;
    [SerializeField] private float SetPushTime = 0.0f;
    [SerializeField, Header("�t�F�[�h�b��")]
    private float FadeTime;
    private Image fade;             // �t�F�[�h�p
    private BGMManager bgmManager;  // BGM�p     
    private float PushButtonTime = 0.0f;
    private bool PushFlag = false;
    private bool FirstLoad = false;

    private void Awake()
    {
        // �t�F�[�h
        fade = GameObject.Find("FadeImage").GetComponent<Image>();
        // ��
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
    }
    private void Update()
    {
        //- ���͒�
        if(PushFlag)
        {
            PushButtonTime += Time.deltaTime;
            Skip.fillAmount = PushButtonTime / SetPushTime;
        }
        //- �����
        else
        {
            PushButtonTime = 0.0f;
            Skip.fillAmount = 0.0f;
        }
        //- �w�莞�Ԉȏ���͂���Ă����狭���I�ɃV�[���J�ڂ��s��
        if(PushButtonTime > SetPushTime)
        {
            //- ���ǂݍ��񂾂�ȍ~�������Ȃ�
            if(!FirstLoad)
            {
                FirstLoad = true;
                DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager());
                fade.DOFade(1.0f, 1.5f).OnComplete(() => { SceneManager.LoadScene("1_Village"); });
            }
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        //- �N���A���Ă��Ȃ��ۂɃ{�^�����͂��󂯕t����
        if (context.started)
        { PushFlag = true;  }//���͒�
        if (context.canceled)
        { PushFlag = false; }//���͒��~
    }
}
