using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MovieManager : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�̒���(�b)")]
    private float FadeTime;

    [SerializeField, Header("�t�F�[�h��ł��グ��܂ł̎���(�b)")]
    private float DelayBeltTime;

    [SerializeField, Header("�ł��グ��A��������܂ł̎���(�b)")]
    private float DelayFireflowerTime;
    

    [SerializeField, Header("������A�t�F�[�h���n�܂�܂ł̎���(�b)")]
    private float DelayFadeTime;

    [SerializeField, Header("�X�e�[�W�`��I�u�W�F�N�g")]
    private GameObject StageDrawObj;

    [SerializeField, Header("�ʂ��ԉ΂̓����̕⊮")]
    private Ease easy;

    [SerializeField, Header("���[�h����X�e�[�W�ԍ�")]
    private int LoadStageNum;

    private SceneChange sceneChange; // �R���g���[���[�̐U���p
    private bool bPlayMovie = false; // ���o�����ǂ���
    private ObjectFade fade; // �t�F�[�h�p�̃X�v���C�g
    private GameObject movieObj; // ���o�V�[�����̃I�u�W�F�N�g
    private bool bPlayRequest; //- ���o�J�n�\�� 
    private CountEnemy countEnemy;

    void Start()
    {
        //- �J�����̎擾
        GameObject camera = GameObject.Find("Main Camera");
        //- ���C���J��������V�[���ύX�X�N���v�g�擾
        sceneChange = camera.GetComponent<SceneChange>();
        //- �t�F�[�h�p�X�N���v�g�̎擾
        fade = GameObject.Find("FadeImage").GetComponent<ObjectFade>();
        //- �ԉ΋ʂ��J�E���g���鏈��
        countEnemy = camera.GetComponent<CountEnemy>();
    }
    
    void Update()
    {
        // ���݂̓G�̐����X�V
        int EnemyNum = countEnemy.GetCurrentCountNum();
        if (bPlayMovie && EnemyNum == 0) sceneChange.ResetCurrentTime(); //- ���o���A�N���A���o���o�Ȃ��悤�ɂ���
        if (bPlayRequest) CheckCount(); //- ���o�J�n�\��������ΌĂяo����鏈��
    }

    //- ���o�t���O��ύX����֐�
    public void SetMovieFlag(bool bFlag)
    {
        bPlayMovie = bFlag;
    }

    //- ���o�������J�n����֐�
    public void StartVillageMovie()
    {
        bPlayRequest = true;
    }

    private void CheckCount()
    {
        // ���݂̓G�̐����X�V
        int EnemyNum = countEnemy.GetCurrentCountNum();

        if (EnemyNum != 0) return; //- �ԉ΋ʂ��c���Ă���΃��^�[��
        
        bPlayRequest = false;
        StartCoroutine(MovieSequence());
    }

    private IEnumerator MovieSequence()
    {
        bPlayMovie = true; //- ���o�t���O�ύX

        //- �t�F�[�h��o�ꂳ����
        fade.SetFade(ObjectFade.FadeState.In, FadeTime);
        yield return new WaitForSeconds(FadeTime);

        //- ���o�V�[����ǉ����[�h,�t�F�[�h��ޏꂳ����
        LoadMovieScene();
        fade.SetFade(ObjectFade.FadeState.Out, FadeTime);
        yield return new WaitForSeconds(FadeTime);

        //- ���o�V�[�����I�u�W�F�N�g������
        InitMovieObject();

        //- ��莞�Ԍ�A�ʂ��ԉ΂�ł��グ��
        if (LoadStageNum != 4)
        {
            yield return new WaitForSeconds(DelayBeltTime);
            SetActiveMovieObject(0, true);
            AnimeBossFireflower(0);
        }

        //- ��莞�Ԍ�A�ԉ΂𔭐�������
        yield return new WaitForSeconds(DelayFireflowerTime);
        //- ���ɉ����ė������ʉ���ς���
        if (LoadStageNum != 4) { SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion); }
        else { SEManager.Instance.EffectSetPlaySE(SEManager.E_SoundEffect.DragonFire, 0.1f); }
        SetActiveMovieObject(0, false); //- �ʂ��ԉ΂̑ޏ�
        SetActiveMovieObject(1, true);  //- �G�t�F�N�g�o��

        if (LoadStageNum != 4)
        {
            //- ��莞�Ԍ�A�t�F�[�h��o�ꂳ����
            yield return new WaitForSeconds(DelayFadeTime);
            fade.SetFade(ObjectFade.FadeState.In, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //- ���o�V�[�����A�����[�h
            UnloadMovieScene();
            fade.SetFade(ObjectFade.FadeState.Out, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopClear(false);
            scenechange.RequestStopClear(false);
            scenechange.RequestStopClear(false);
            scenechange.RequestStopMiss(false);
            bPlayMovie = false; //- ���o�t���O�ύX
        }
        else
        {
            //- ��莞�Ԍ�A�t�F�[�h��o�ꂳ����
            yield return new WaitForSeconds(DelayFadeTime);
            fade.SetColor(Color.white, 0.0f);
            fade.SetFade(ObjectFade.FadeState.In, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //- ���o�V�[�����A�����[�h
            UnloadMovieScene();
            fade.SetFade(ObjectFade.FadeState.Out, FadeTime);
            GameObject.Find("BGMManager").GetComponent<BGMManager>().DestroyPossible(); // BGMManager���폜�\�ɂ���
            SceneChange.bIsChange = true;
            GameObject.Find("Main Camera").GetComponent<SceneChange>().Change(); // �V�[���؂�ւ�
            yield return new WaitForSeconds(FadeTime);
        }
    }

    //- ���o�p�V�[���̃��[�h���s���֐�
    private void LoadMovieScene()
    {
        StageDrawObj.SetActive(false); //- �X�e�[�W�I�u�W�F�N�g�̕`�����߂�
        SceneManager.LoadScene("MovieVillage" + LoadStageNum.ToString(), LoadSceneMode.Additive); //- ���o�p�V�[����ǉ����[�h   
    }

    //- ���o�V�[�����[�h��A�I�u�W�F�N�g����肷�邽�߂̏������֐�
    private void InitMovieObject()
    {
        movieObj = GameObject.Find("MovieObject"); //- �I�u�W�F�N�g�̌���
    }

    //- �ʂ��ԉΗp�̉��o����
    private void AnimeBossFireflower(int childNum)
    {
        //- �ʂ��ԉ΂̒u������
        GameObject obj = movieObj.transform.GetChild(childNum).gameObject;
        //- �ʂ��ԉ΂̃X�P�[���t�F�[�h
        obj.transform.DOScale(new Vector3(1, 1, 1), DelayFireflowerTime).SetEase(easy);
    }

    //- ����̃I�u�W�F�N�g�̃t���O��ύX����֐�
    private void SetActiveMovieObject(int childNum, bool bFlag)
    {
        movieObj.transform.GetChild(childNum).gameObject.SetActive(bFlag); //- �t���O�ύX
    }

    //- ���o�p�V�[���̃A�����[�h���s���֐�
    private void UnloadMovieScene()
    {
        StageDrawObj.SetActive(true); //- �X�e�[�W�I�u�W�F�N�g�̕`����ĊJ
        SceneManager.UnloadScene("MovieVillage" + LoadStageNum.ToString()); //- ���o�p�V�[���̃A�����[�h
    }
}