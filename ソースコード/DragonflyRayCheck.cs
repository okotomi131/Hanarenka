using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonflyRayCheck : MonoBehaviour
{
    [Header("���CBOX�̔�������(���CBOX�̈�ӂ̒���)"), SerializeField]
    private float RayBoxRadius;

    [Header("������������ɂȂ郌�C�̒���"), SerializeField]
    private float HitRayDistance;

    [Header("��ʊO���E���A��������I�u�W�F�N�g"), SerializeField]
    private GameObject DestroyObject;

    private FireworksModule module; //- �ԉ΃I�u�W�F�N�g�ɃA�^�b�`����Ă���X�N���v�g

    private Vector2[] RayStartPos;  //- ���C�̔������W���i�[����z��
    private Vector2[] RayDirection; //- ���C�̔����������i�[����z��
    private Vector2 SaveMoveDir;    //- ���X�̈ړ�������ۑ����Ă������߂̕ϐ�

    private int HitDirPlayerBlock = -1; �@    //- �N���s�e�[�v��������������
    private bool IsHitPlayerBlock = false;    //- �N���s�e�[�v�̐ڐG����t���O
    private bool CheckHitPlayerBlock = false; //- �N���s�e�[�v���痣�ꂽ�u�Ԃ𒲂ׂ邽�߂̃t���O
    private bool isDestroy = false;

    [SerializeField, HideInInspector] //- ���s�t���O�ꎞ��~�����ɖ߂�����
    public bool isPlayback = false; 

    void Start()
    {
        //- �ԉ΃X�N���v�g�̎擾
        module = GetComponent<FireworksModule>();

        //- ���C�̔������W�z��𐶐�
        RayStartPos = new Vector2[5];
        RayStartPos[0] = new Vector2(-1, 1);
        RayStartPos[1] = new Vector2(1, 1);
        RayStartPos[2] = new Vector2(1, -1);
        RayStartPos[3] = new Vector2(-1, -1);
        RayStartPos[4] = RayStartPos[0]; //- �Ō�ƍŏ��̍��W�͓����Ȃ̂ŁA�������W��p�ӂ��Ă���

        //- ���C�̔��������z��𐶐�
        RayDirection = new Vector2[4];
        RayDirection[0] = new Vector2(0, 1);
        RayDirection[1] = new Vector2(1, 0);
        RayDirection[2] = new Vector2(0, -1);
        RayDirection[3] = new Vector2(-1, 0);
    }

    void FixedUpdate()
    {
        //- �N���s�e�[�v�Ɨ����^�C�~���O�𒲂ׂ邽�߂̕ϐ�
        CheckHitPlayerBlock = false;

        //- ���g�̍��W���擾
        Vector3 MyPos = this.transform.position;

        // === �S�̊p����v�W�{�̃��C���΂����� ===
        for (int i = 0; i < 8; i++)
        {
            // === ���C�p�ϐ��p�ӕ��� === 
            //- ���C�̕���
            int DirNum = i / 2; //- ���C�����p�̔z��ԍ��ϐ�
            Vector3 NowDir = RayDirection[DirNum];
            //- ���C�̔������W�����CBOX�T�C�Y���������炷
            Vector2 PosRadius = new Vector2(RayBoxRadius / 2 * RayStartPos[(i + 1) / 2].x, RayBoxRadius / 2 * RayStartPos[(i + 1) / 2].y);
            //- ���C�����{���甭��������
            if (DirNum == 0 || DirNum == 2) PosRadius.y = 0;
            if (DirNum == 1 || DirNum == 3) PosRadius.x = 0;
            //- ���C�̔������W�����߂�
            Vector3 NowPos = new Vector3(MyPos.x + PosRadius.x, MyPos.y + PosRadius.y, 0);


            //- ���C�𐶐�
            Ray ray = new Ray(NowPos, NowDir);

            //- �����蔻��̃f�o�b�O�\��
            if (Input.GetKey(KeyCode.Alpha1) || true)
            {
                Debug.DrawRay(NowPos, NowDir * 2, Color.red);     //�ԐF�łT�b�ԉ���
                Debug.DrawRay(NowPos, NowDir * HitRayDistance, Color.blue); //�F�łT�b�ԉ���
            }

            //- �ђʂ��郌�C���΂��A���������I�u�W�F�N�g��S�Ē��ׂ�
            foreach (RaycastHit hit in Physics.RaycastAll(ray, HitRayDistance))
            {
                //- ���C�����������������q�b�g���苗����������΁A�R���e�j���[
                if (hit.distance >= HitRayDistance) continue;
                //- �^�O�ɂ���Ď��s���鏈����ς���
                switch (hit.collider.gameObject.tag)
                {
                    case "Stage":
                        HitCheckStage(hit,DirNum);
                        break;
                    case "PlayerBlock":
                        HitCheckPlayerBlock(hit, DirNum);
                        break;
                    default:
                        break;
                }//- �^�O��switch���I��
            }//- ��{�̃��C���I��
        }//- �S�Ẵ��C���I��

        // === �N���s�e�[�v�Ɨ��ꂽ���ǂ����`�F�b�N ===
        //- �N���s�e�[�v�ƐڐG���Ă��� &�@���C���N���s�e�[�v�ɓ�����Ȃ������@
        if (IsHitPlayerBlock && !CheckHitPlayerBlock)
        {
            //- �t���O�̕ύX
            IsHitPlayerBlock = false;
            //- �L�����Ă��ړ������𕜊�������
            module.movedir = SaveMoveDir;
            //- �����̃��Z�b�g
            HitDirPlayerBlock = -1;
        }
    }
    void HitCheckStage(RaycastHit hit, int dirnum)
    {
        // === ���C�̕����𒲂ׂāA�ړ��𔽓]������ ===

        //- ����� & �g���{�ԉ΂���Ɉړ����Ȃ�@�㉺�ɔ��]
        if (dirnum == 0 && module.movedir.y > 0) module.movedir.y *= -1;

        //- �E���� & �g���{�ԉ΂��E�Ɉړ����Ȃ�@���E�ɔ��]
        if (dirnum == 1 && module.movedir.x > 0) module.movedir.x *= -1;

        //- ������ & �g���{�ԉ΂����Ɉړ����Ȃ�@�㉺�ɔ��]
        if (dirnum == 2 && module.movedir.y < 0) module.movedir.y *= -1;

        //- ������ & �g���{�ԉ΂����Ɉړ����Ȃ�@���E�ɔ��]
        if (dirnum == 3 && module.movedir.x < 0) module.movedir.x *= -1;
    }
    void HitCheckPlayerBlock(RaycastHit hit, int dirnum)
    {
        //- �N���s�e�[�v�ƐڐG���Ă���Ƃ��̂ݎ��s
        if (IsHitPlayerBlock)
        {
            //- �N���s�e�[�v����������A�ƃ��C���������������������Ȃ珈��
            //- �N���s�e�[�v�Ƃ̐ڐG�`�F�b�N�t���O��ύX
            if (HitDirPlayerBlock == dirnum) CheckHitPlayerBlock = true;
        }
        //- �N���s�e�[�v�ɐڐG���Ă���Εʂ̊֐����Ă�
        if (IsHitPlayerBlock && HitDirPlayerBlock != -1)
        {
            CheckDobuleHit(dirnum);
            return;
        }

        // === ���C�̕����𒲂ׂāA�ړ����������肷�� ===

        //- �ړ��������L�����Ă���
        SaveMoveDir = module.movedir;

        //- �N���s�e�[�v�Ƃ̐ڐG�t���O��ύX
        IsHitPlayerBlock = true;

        //- ����� & �g���{�ԉ΂���Ɉړ����Ȃ���s
        if (dirnum == 0 && module.movedir.y > 0)
        {
            //- �c�����̈ړ�������
            module.movedir.y = 0;
            //- �������̈ړ����x��L�΂�
            if (module.movedir.x >  0) module.movedir.x = 1;
            if (module.movedir.x <= 0) module.movedir.x = -1;
            //- �ڐG�����̕ۑ�
            HitDirPlayerBlock = 0;
        }       
        //- �E���� & �g���{�ԉ΂��E�Ɉړ����Ȃ���s
        if (dirnum == 1 && module.movedir.x > 0)
        {
            //- �������̈ړ�������
            module.movedir.x = 0;
            //- �c�����̈ړ����x��L�΂�
            if (module.movedir.y >  0) module.movedir.y = 1;
            if (module.movedir.y <= 0) module.movedir.y = -1;
            //- �ڐG�����̕ۑ�
            HitDirPlayerBlock = 1;
        }
        //- ������ & �g���{�ԉ΂����Ɉړ����Ȃ���s
        if (dirnum == 2 && module.movedir.y < 0)
        {
            //- �c�����̈ړ�������
            module.movedir.y = 0;
            //- �������̈ړ����x��L�΂�
            if (module.movedir.x >  0) module.movedir.x = 1;
            if (module.movedir.x <= 0) module.movedir.x = -1;
            //- �ڐG�����̕ۑ�
            HitDirPlayerBlock = 2;
        }
        //- ������ & �g���{�ԉ΂����Ɉړ����Ȃ���s
        if (dirnum == 3 && module.movedir.x < 0)
        {
            //- �������̈ړ�������
            module.movedir.x = 0;
            //- �c�����̈ړ����x��L�΂�
            if (module.movedir.y >  0) module.movedir.y = 1;
            if (module.movedir.y <= 0) module.movedir.y = -1;
            //- �ڐG�����̕ۑ�
            HitDirPlayerBlock = 3;
        }
    }

    void CheckDobuleHit(int dirnum)
    {
        //- �g���{�ԉ΂���Ɉړ����Ȃ���s
        if (dirnum == 0 && module.movedir.x == 0 && module.movedir.y == 1)
        {
            module.movedir = new Vector2(0, 0);
        }
        //- �g���{�ԉ΂��E�Ɉړ����Ȃ���s
        if (dirnum == 1 && module.movedir.y == 0 && module.movedir.x == 1)
        {
            module.movedir = new Vector2(0, 0);
        }
        //- �g���{�ԉ΂����Ɉړ����Ȃ���s
        if (dirnum == 2 && module.movedir.x == 0 && module.movedir.y == -1)
        {
            module.movedir = new Vector2(0, 0);
        }
        //- �g���{�ԉ΂����Ɉړ����Ȃ���s
        if (dirnum == 3 && module.movedir.y == 0 && module.movedir.x == -1)
        {
            module.movedir = new Vector2(0, 0);
        }

        //- �܂����s��������ɖ߂��Ă��Ȃ��Ȃ珈��
        if (!isPlayback)
        {
            isPlayback = true;
            //- ���s����𕜊�������
            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopMiss(false);
        }
    }
}

