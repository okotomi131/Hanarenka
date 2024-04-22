/*
 ===================
 ����F���
 �M�~�b�N�K�C�h�A�j���[�V�������Ǘ�����X�N���v�g
 ===================
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Video;
#if UNITY_EDITOR
//- �f�v���C����Editor�X�N���v�g������ƃG���[�BUNITY_EDITOR�Ŋ���
using UnityEditor;
#endif

//- �M�~�b�N�����Ŕ̃A�j���[�V����
public class BoardMove : MonoBehaviour
{
    private enum E_OUTDIRECTION
    {
        [Header("��")]
        LEFT,
        [Header("�E")]
        RIGHT,
        [Header("��")]
        UP,
        [Header("��")]
        DOWN,
    }

    private const float LEFT = -2500.0f;
    private const float RIGHT = 3500.0f;
    private const float TOP = 1200.0f;
    private const float DOWN = -1200.0f;

    [SerializeField] private Image img;
    [SerializeField] private VideoPlayer movie;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private float IntervalTime = 0.0f;
    [SerializeField] private Image padbutton;
    [SerializeField] private TextMeshProUGUI padtmp;
    private Dictionary<string, Dictionary<string, Vector3>> InitValues;
    public static bool MoveComplete = false;

    private bool LoadStart = false;
    private bool LoadOut = false;
    private bool ReInMove;     //�o��{�^�����̓t���O
    private bool ReOutMove;    //�P�ރ{�^�����̓t���O
    private bool NowMove = false; //���쒆�t���O
    private bool NotInput = false;//���͎�t�t���O
    private bool StartComplete = false;//�o�ꏈ��������������

    private void Awake()
    {
        //- �����l�o�^
        InitValues = new Dictionary<string, Dictionary<string, Vector3>>
        {{"����",new Dictionary<string, Vector3>{{"�ʒu",movie.transform.position},}}};
        InitValues.Add("����", new Dictionary<string, Vector3> { { "�ʒu", tmp.transform.position } });
        InitValues.Add("�w�i", new Dictionary<string, Vector3> { { "�ʒu", img.transform.position } });
        //- �����ʒu�X�V
        img.transform.localPosition = new Vector3(LEFT + img.transform.localPosition.x, img.transform.localPosition.y);
        movie.transform.localPosition = new Vector3(LEFT + img.transform.localPosition.x, movie.transform.localPosition.y);
        tmp.transform.localPosition = new Vector3(LEFT + img.transform.localPosition.x, tmp.transform.localPosition.y);
        //- �ҋ@���͓����~
        movie.Pause();
    }
    
    /// <summary>
    /// �o�ꋓ�����s��
    /// </summary>
    public void StartMove()
    {
        //- ����ǂݍ���
        if(!LoadStart)
        {
            //- �ǂݍ��ݍςɂ���
            LoadStart = true;
            //- �o�ꏈ���ɓ�������{�^�����͂����͏�Ԃ�
            ReInMove = false;
            //- �o�ꂷ��ۂɓ���Đ�
            movie.Play();
            //- �V�[�P���X�쐬
            var InAnime = DOTween.Sequence();
            //- �o�ꋓ��
            InAnime.AppendInterval(IntervalTime)
            .Append(img.transform.DOMove(InitValues["�w�i"]["�ʒu"], 0.5f))
            .Join(movie.transform.DOMove(InitValues["����"]["�ʒu"], 0.525f))
            .Join(tmp.transform.DOMove(InitValues["����"]["�ʒu"], 0.5f))
            .OnComplete(() => {
                //- �o�ꋓ������������P�މ\�ɂ���
                LoadOut = false;
                //-�@�\������������
                StartComplete = true;
                //- ����������������A�j���[�V�����폜
                InAnime.Kill();
            });
        }
    }

    /// <summary>
    /// �P�ދ������s��
    /// </summary>
    public void OutMove()
    {
        //- ����ǂݍ���
        if(!LoadOut)
        {
            //- �ǂݍ��ݍςɂ���
            LoadOut = true;
            //- �A�Ŗh�~�t���O
            NowMove = false;
            //- �P�ނ���O�ɓ���̍Đ����~�߂�
            movie.Stop();
            //- �V�[�P���X�쐬
            var OutAnime = DOTween.Sequence();

            //- �P�ޏ�������
                OutAnime.Append(movie.transform.DOMoveX(RIGHT, 0.3f))
                .Join(img.transform.DOMoveX(RIGHT, 0.3f))
                .Join(tmp.transform.DOMoveX(RIGHT, 0.3f))
                .OnComplete(() =>
                {
                    //- �����ʒu�X�V
                    img.transform.localPosition = new Vector3(LEFT, img.transform.localPosition.y);
                    movie.transform.localPosition = new Vector3(LEFT, movie.transform.localPosition.y);
                    tmp.transform.localPosition = new Vector3(LEFT, tmp.transform.localPosition.y);
                    //- �P�ދ�������������Tips�ĕ\���\�ɂ���
                    LoadStart = false;
                    //- ��A�̋������I��
                    MoveComplete = true;
                    //- �A�j���[�V�����폜
                    OutAnime.Kill();
                });
        }
    }

    /// <summary>
    ///  ���񓮍슮���t���O�����Z�b�g
    /// </summary>
    public static void ResetMoveComplete()
    {   MoveComplete = false;    }
    
    /// <summary>
    /// �ēo��t���O�ԋp
    /// </summary>
    public bool GetInDrawButtonPush()
    {   return ReInMove;     }

    /// <summary>
    /// �ēP�ރt���O�ԋp
    /// </summary>
    public bool GetOutDrawButtonPush()
    { return ReOutMove;      }  

    /// <summary>
    /// �o��t���O�ԋp
    /// </summary>
    public bool GetLoadStart()
    { return LoadStart; }
    
    /// <summary>
    /// �ޏ�t���O�ԋp
    /// </summary>
    public bool GetLoadOut()
    { return LoadOut; }

    public bool GetStartComplete()
    { return StartComplete; }

    /// <summary>
    /// �ēo�����
    /// </summary>
    /// <param name="context"></param>
    public void OnInTips(InputAction.CallbackContext context)
    {
        //- ���͎�t�t���O�������Ă����珈�����Ȃ�
        if (NotInput) { return; }
        //- ���쒆�ł���Ύ��s���Ȃ�
        if (NowMove)  { return; }
        NowMove = true;

        //- Tips�ĕ`��t���O���I���ɂ���
        if (context.started && !SceneChange.bIsChange)
        {
            //- �ēo�ꎞ�͑ҋ@���Ԃ𖳂���
            IntervalTime = 0.0f;
            ReInMove = true;
        }
    }

    /// <summary>
    /// Tips�\�����󂯕t���邩
    /// </summary>
    /// <param name="flag"></param>
    public void SetReceiptInput(bool flag)
    {   NotInput = flag;    }

    public void SetButtonColor(Color color)
    {
        //- ���łɓ����F�Ȃ珈�����Ȃ�
        if(padbutton.color == color)
        { return; }
        //- �w��F�ɕύX
        padbutton.color = color;
        padtmp.color = color;
    }

    /// <summary>
    /// �ēP�ޓ���
    /// </summary>
    /// <param name="context"></param>
    public void OnOutTips(InputAction.CallbackContext context)
    {
        //- �N���A���Ă��Ȃ��ۂɃ{�^�����͂��󂯕t����
        if (context.started && !SceneChange.bIsChange)
        {   ReOutMove = true;  }//���͒�
        if(context.canceled && !SceneChange.bIsChange)
        {   ReOutMove = false; }//���͒��~
    }

    
}

