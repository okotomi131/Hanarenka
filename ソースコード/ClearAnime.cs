/*
 ===================
 ��Ր���F���
 �N���A���o���s���X�N���v�g
 ===================
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ClearAnime : MonoBehaviour
{
    private enum E_Directions
    {
        CENTER,
        TOP,
        LOWER,
        RIGHT,
        LEFT,
    }

    private enum E_POSSTATE
    {
        Rect,
        local,
    }


    [HeaderAttribute("---���W�ݒ�---")]
    [SerializeField]
    private E_POSSTATE PosState = E_POSSTATE.Rect;
    [HeaderAttribute("---�t�F�[�h�ݒ�---")]
    [SerializeField, Header("�ړ����������s����:�`�F�b�N�Ŏ��s")]
    private bool UseFade = false;
    [SerializeField, Header("�J�n�̃A���t�@�l")]
    private float StartAlpha = 1.0f;
    [SerializeField, Header("�I���̃A���t�@�l")]
    private float EndAlpha = 0.0f;
    [SerializeField, Header("�t�F�[�h�����܂ł̎���:float")]
    private float FadeTime = 0.0f;
    [SerializeField, Header("�f�B���C:float")]
    private float FadeDelay = 0.0f;


    [HeaderAttribute("---�ړ��ݒ�--")]
    [SerializeField, Header("�ړ����������s����:�`�F�b�N�Ŏ��s")]
    private bool UseMove = false;
    [SerializeField, Header("�ǂ����猻�݈ʒu�Ɍ������Ă��邩")]
    private E_Directions StartPos;
    [SerializeField, Header("�ړ������܂ł̎���:float")]
    private float MoveTime = 0.0f;
    [SerializeField, Header("�f�B���C:float")]
    private float Delay = 0.0f;
    private Vector3 InitPos;

    [HeaderAttribute("---�|�b�v�ݒ�---")]
    [SerializeField, Header("�|�b�v���������s����F�`�F�b�N�Ŏ��s")]
    private bool UsePop = false;
    [SerializeField, Header("�|�b�v�̍ő�{���Ffloat")]
    private Vector2 PopSize;
    [SerializeField, Header("�|�b�v�̍ő�{���܂ł̎��ԁFfloat")]
    private float PopMaxSizeTime = 1.0f;
    //[SerializeField, Header("�|�b�v�㌳�T�C�Y�ɖ߂�܂ł̎��ԁFfloat")]
    //private float PopInitSizeTime = 1.0f;

    static int animeObjNum = 0;
    static bool isAnime = false;
    private bool isOnce = false;

    IgnoreMouseInputModule inputModule;

    private void Start()
    {
        inputModule = GameObject.Find("EventSystem").GetComponent<IgnoreMouseInputModule>();
        inputModule.enabled = false;
        isAnime = true;
        if (UseFade)
        { DoFade(); }
        if (UseMove)
        {   PosMove();   }
        if(UsePop)
        { DoPop(); }
    }

    private void Awake()
    {
        animeObjNum = 0;
    }

    private void Update()
    {
        if ( isAnime && animeObjNum <= 0) {
            //isOnce = true;
            inputModule.enabled = true;
        }
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void PosMove()
    {
        animeObjNum++;
        switch (PosState)
        {
            case E_POSSTATE.Rect:
                RectTransform trans = GetComponent<RectTransform>();
                //- �����ʒu��ۑ�
                InitPos = trans.anchoredPosition;
                //- ��Ԃɍ��킹�ăX�^�[�g�ʒu��ύX
                switch (StartPos)
                {
                    case E_Directions.CENTER:
                        trans.anchoredPosition = new Vector2(0.0f, 0.0f);
                        break;
                    case E_Directions.TOP:
                        trans.anchoredPosition = new Vector2(InitPos.x, Screen.height);
                        break;
                    case E_Directions.LOWER:
                        trans.anchoredPosition = new Vector2(InitPos.x, -Screen.height);
                        break;
                    case E_Directions.RIGHT:
                        trans.anchoredPosition = new Vector2(Screen.width, InitPos.y);
                        break;
                    case E_Directions.LEFT:
                        trans.anchoredPosition = new Vector2(-Screen.width, InitPos.y);
                        break;
                }
                
                //- �����ʒu�ɂނ����Ĉړ�
                transform.DOLocalMove(InitPos, MoveTime)
                    .SetEase(Ease.OutSine)
                    .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
                    .SetDelay(Delay)
                    .OnComplete(() => { animeObjNum--; });
                break;

            case E_POSSTATE.local:
                Transform localTrans = GetComponent<Transform>();
                //- �����ʒu��ۑ�
                InitPos = localTrans.position;
                //- ��Ԃɍ��킹�ăX�^�[�g�ʒu��ύX
                switch (StartPos)
                {
                    case E_Directions.CENTER:
                        localTrans.position = new Vector3(0.0f, 0.0f);
                        break;
                    case E_Directions.TOP:
                        localTrans.position = new Vector3(InitPos.x, 30.0f,-5.0f);
                        break;
                    case E_Directions.LOWER:
                        localTrans.position = new Vector3(InitPos.x, -30.0f, -5.0f);
                        break;
                    case E_Directions.RIGHT:
                        localTrans.position = new Vector3(Screen.width, InitPos.y);
                        break;
                    case E_Directions.LEFT:
                         localTrans.position = new Vector3(-Screen.width, InitPos.y);
                         localTrans.position = new Vector3(-Screen.width, InitPos.y);
                        break;
                }



                //- �����ʒu�ɂނ����Ĉړ�
                transform.DOMove(InitPos, MoveTime)
                    .SetEase(Ease.OutSine)
                    .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
                    .SetDelay(Delay)
                    .OnComplete(() => { animeObjNum--; });
                break;
        }
    }



/// <summary>
/// �t�F�[�h����
/// </summary>
private void DoFade()
    {
        animeObjNum++;

        Image image = GetComponent<Image>();
        //- �w�肵���A���t�@�l�ŊJ�n
        image.color = new Color(image.color.r, image.color.g, image.color.b, StartAlpha);
        //- �t�F�[�h���s��
        image.DOFade(EndAlpha, FadeTime).SetDelay(FadeDelay)
            .SetLink(image.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
            .OnComplete(() => {
                animeObjNum--;
            });
    }


    private void DoPop()
    {
        animeObjNum++;
        Debug.Log(animeObjNum);

        Image image = GetComponent<Image>();
        Vector2 Initsize = this.gameObject.transform.localScale;
        transform.DOScale(new Vector3(Initsize.x * PopSize.x, Initsize.y * PopSize.y, 0.0f), PopMaxSizeTime)
            .SetEase(Ease.OutSine)
            .SetLink(image.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
            .OnComplete(() => 
            {
                transform.DOScale(new Vector3(Initsize.x, Initsize.y, 0.0f), PopMaxSizeTime)
                .SetEase(Ease.OutSine)
                .SetLink(image.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
                .OnComplete(() => {
                    animeObjNum--;
                    Debug.Log(animeObjNum);
                });
            });
    }
}
