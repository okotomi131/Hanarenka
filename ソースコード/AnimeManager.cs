/*
 ===================
 ����F���
 UI�A�j���[�V�����̍쓮�^�C�~���O���Ǘ�����X�N���v�g
 ===================
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

//- UI�A�j���[�V�����쓮���Ǘ�����N���X
public class AnimeManager : MonoBehaviour
{

    [SerializeField] private EntryAnime DrawSelect;         //���I��UI
    [SerializeField] private EntryAnime DrawReset;          //��蒼��UI
    [SerializeField] private EntryAnime DrawTips;           //Tips�ĕ\��UI
    [SerializeField] private OpeningAnime DrawOpening;      //�J�����o
    [SerializeField] private CutIn DrawBossCutIn;           //�{�X�J�b�g�C��
    [SerializeField] private BoardMove DrawGimmickBoard;    //Tips�\��
    [SerializeField] private float MaxPushTime = 1.0f;      //���͎���
    [SerializeField] private bool Auto = false;             //�����N���A����

    private Dictionary<string, bool> ControlFlag;           //�A�j���[�V�����I�u�W�F�N�g�̃t���O�Ǘ�
    private bool InMoveCompleat = false;                    //�o�ꏈ�������t���O
    private bool OutMoveCompleat = false;                   //�P�ޏ��������t���O
    private bool FirstLoad = false;                         //����ǂݍ��݃t���O
    private bool TipsInLoad = false;                        //Tips�o��t���O
    private bool TipsOutLoad = false;                       //Tips�P�ރt���O
    private InputAction tipsAction;                         //Tips�o��̓��͏���
    private PController player;                             //�v���C���[����p�ϐ�
    private Image TipsButtonGage;                           //Tips�\���{�^���̒������Q�[�W
    private float TipsPossibleTime = 1.0f;                  //Tips�\��������͉\�ɂȂ�܂ł̎���
    private float StartTime = 0.0f;                         //�J�n����̎���

    private void Awake()
    {
        //- �A�j���Ǘ�����I�u�W�F�N�g�t���O������
        ControlFlag = new Dictionary<string, bool>
                    {
                        { "�Z���N�g", false },
                        { "���Z�b�g", false },
                        { "Tips�ĕ\��",false },
                        { "�J��", false },
                        { "Tips", false },
                        { "�{�X", false },
                    };

        //- �I�u�W�F�N�g�����݂�����t���O�ύX
        if (DrawSelect)       { ControlFlag["�Z���N�g"] = true;     }
        if (DrawReset)        { ControlFlag["���Z�b�g"] = true;     }
        if (DrawTips)         { ControlFlag["Tips�ĕ\��"] = true; }
        if (DrawOpening)      { ControlFlag["�J��"] = true;     }
        if (DrawGimmickBoard) { ControlFlag["Tips"] = true; }
        if (DrawBossCutIn)    { ControlFlag["�{�X"] = true;     }

        //- �v���C���[���擾
        player = GameObject.Find("Player").GetComponent<PController>();
    }

    private void Start()
    {
        //- Tips������E����`��
        if(ControlFlag["Tips"] && !BoardMove.MoveComplete)
        { DrawGimmickBoard.StartMove(); }//Tips��\��

        //- �J��������E����`��
        else if(ControlFlag["�J��"] && !OpeningAnime.MoveCompleat)
        { DrawOpening.StartMove(); }//�J����\��
    }

    void Update()
    {
        //- �v���C���[�̎����t���O���擾����
        bool PlayerBoom = player.GetIsOnce();
        //- Tips������ۂɂ͕\�����삪�I����Ă���ҋ@�J�E���g���J�n
        if (ControlFlag["Tips"] && DrawGimmickBoard.GetStartComplete())
        { StartTime += Time.deltaTime; }
        
        /*�@
         *�@��������������������������������
         *�@
         *�@��ڂ̃A�j���[�V�������s����
         *�@
         *�@��������������������������������
         */

        //- �{�X�X�e�[�W�E���o�����Ă��Ȃ��E���߂ď�������ETips���P�ނ��Ă���
        if (ControlFlag["�{�X"] && !CutIn.MoveCompleat && !FirstLoad && BoardMove.MoveComplete)
        {
            //- ���߂ēǂݍ���
            FirstLoad = true;
            //- �{�X���o���s��
            DrawBossCutIn.MoveCutIn();
            //- ���쒆��Tips������󂯕t���Ȃ�
            DrawGimmickBoard.SetReceiptInput(true);
        }
        //- �ʏ�X�e�[�W�E���o�����Ă��Ȃ��E���߂ď�������ETips���P�ނ��Ă���
        else if(ControlFlag["�J��"] && !OpeningAnime.MoveCompleat && !FirstLoad && BoardMove.MoveComplete)
        {
            //- ���߂ēǂݍ���
            FirstLoad = true;
            //- �J�����o���s��
            DrawOpening.StartMove();
            //- ���쒆��Tips������󂯕t���Ȃ�
            DrawGimmickBoard.SetReceiptInput(true);
        }


        /*�@
         *�@��������������������������������
         *�@
         *�@��ڂ̃A�j���[�V�������I�����A
         *�@�{�^���A�V�X�g��\������
         *�@
         *�@��������������������������������
         */
         
        //- �{�X�X�e�[�W�E���o�ςł���
        if (ControlFlag["�{�X"] && CutIn.MoveCompleat)
        {
            //- �A�V�X�g�\����Ɏg�p���Ȃ����߃t���O�ύX
            ControlFlag["�{�X"] = false;
            //- Tips������󂯕t����
            if (ControlFlag["Tips"])    { DrawGimmickBoard.SetReceiptInput(false); }
            //- �{�^���A�V�X�g��\������
            InGameDrawObjs();
        }
        //- �ʏ�X�e�[�W�E���o�ςł���
        if(ControlFlag["�J��"] && OpeningAnime.MoveCompleat)
        {
            //- �A�V�X�g��Ɏg�p���Ȃ����߃t���O�ύX
            ControlFlag["�J��"] = false;
            //- Tips������󂯕t����
            if (ControlFlag["Tips"])    { DrawGimmickBoard.SetReceiptInput(false); }
            //- �{�^���A�V�X�g��\��
            InGameDrawObjs();

        }


        /*�@
         *�@��������������������������������
         *�@
         *�@Tips��o��E�P�ނ�����@
         *�@
         *�@��������������������������������
         */

        //������������������      �o�ꏈ��       ������������������
        //- Tips������E�{�X���o���I����Ă���E�������Ă��Ȃ��E�ēo��{�^�����͂�����Ă���
        if (ControlFlag["Tips"] && CutIn.MoveCompleat && !PlayerBoom && DrawGimmickBoard.GetInDrawButtonPush())
        {
            //-�@�v���C���[�𓮍�s�\�ɂ���
            GameObject.Find("Player").GetComponent<PController>().SetWaitFlag(true);
            DrawGimmickBoard.StartMove();
            TipsOutLoad = false;
            StartTime = 0.0f;

        }
        //- Tips������E�J�����I����Ă���E�������Ă��Ȃ��E�ēo��{�^�����͂�����Ă���
        else if (ControlFlag["Tips"] && OpeningAnime.MoveCompleat && !PlayerBoom && DrawGimmickBoard.GetInDrawButtonPush())
        {
            //-�@�v���C���[�𓮍�s�\�ɂ���
            GameObject.Find("Player").GetComponent<PController>().SetWaitFlag(true);
            DrawGimmickBoard.StartMove();
            TipsOutLoad = false;
            StartTime = 0.0f;
        }
        //����������������������������������������������������������


        //������������������      �P�ޏ���     ������������������
        if(ControlFlag["Tips"])
        { 
            //- ���͎�t�\���ԂɂȂ����甒�ɂ���
            if(StartTime > TipsPossibleTime)
            { DrawGimmickBoard.SetButtonColor(Color.white);     }
            //- ���͕s���Ԃ̓O���[�ɂ���
            else
            {   DrawGimmickBoard.SetButtonColor(new Color(0.5f, 0.5f, 0.5f, 1.0f));   }
        }

        //- Tips������E����Tips���\������Ă���E�������Ă��Ȃ��E�ēo��{�^�����͂�����Ă���
        if (ControlFlag["Tips"] && DrawGimmickBoard.GetLoadStart() && !PlayerBoom && DrawGimmickBoard.GetOutDrawButtonPush())
        {
            //- �J�n2�b�͓P�ޏ������󂯕t���Ȃ�
            if(StartTime > TipsPossibleTime && !TipsOutLoad)
            {
                TipsOutLoad = true;
                DrawGimmickBoard.OutMove();
                //- ����̓P�ނłȂ�������v���C���[����Ǘ����s��
                if (CutIn.MoveCompleat || OpeningAnime.MoveCompleat)
                {
                    //-�@�v���C���[�𓮍�\�ɂ���
                    GameObject.Find("Player").GetComponent<PController>().SetWaitFlag(false);
                }
            }

        }


        //- �v���C���[�������������ATips���`�悳��Ă����狭���I�ɓP��
        if(PlayerBoom && ControlFlag["Tips"] && DrawGimmickBoard.GetLoadStart())
        { DrawGimmickBoard.OutMove(); }
        //����������������������������������������������������������


        /*�@
         *�@��������������������������������
         *�@
         *�@�N���A���Ƀ{�^���A�V�X�g��P�ނ���
         *�@
         *�@��������������������������������
         */
        if (SceneChange.bIsChange && !OutMoveCompleat)
        {
            if (ControlFlag["�Z���N�g"]) { DrawSelect.OutMove(); }
            if (ControlFlag["���Z�b�g"]) { DrawReset.OutMove(); }
            if (ControlFlag["Tips�ĕ\��"]) { DrawTips.OutMove(); }
            OutMoveCompleat = true;
        }
    }

    /// <summary>
    /// �{�^���A�V�X�g��\������
    /// </summary>
    private void InGameDrawObjs()
    {
        if (ControlFlag["�Z���N�g"]) { DrawSelect.StartMove(); }
        if (ControlFlag["���Z�b�g"]) { DrawReset.StartMove(); }
        if (ControlFlag["Tips�ĕ\��"]) { DrawTips.StartMove(); }
        //-�@�v���C���[�𓮍�\�ɂ���
        GameObject.Find("Player").GetComponent<PController>().SetWaitFlag(false);
        InMoveCompleat = true;
    }

    public void SkipAnime(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        if(context.started)
        { 
            Debug.Log("Skip");
            //-�@�v���C���[�𓮍�\�ɂ���
            GameObject.Find("Player").GetComponent<PController>().SetWaitFlag(false);
            if (DrawSelect)       { DrawSelect.OutMove();     }
            if (DrawReset)        { DrawReset.OutMove();    }
            if (DrawTips)         { DrawTips.OutMove(); }
            if (DrawGimmickBoard) { DrawGimmickBoard.OutMove(); }

            if (DrawSelect) { ControlFlag["�Z���N�g"] = false; }
            if (DrawReset) { ControlFlag["���Z�b�g"] = false; }
            if (DrawTips) { ControlFlag["Tips�ĕ\��"] = false; }
            if (DrawOpening) { ControlFlag["�J��"] = false; }
            if (DrawGimmickBoard) { ControlFlag["Tips"] = false; }
            if (DrawBossCutIn) { ControlFlag["�{�X"] = false; }

            SceneChange.bIsChange = Auto;
        }
#endif
    }
}

