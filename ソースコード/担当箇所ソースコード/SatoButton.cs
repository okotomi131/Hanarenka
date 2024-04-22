/*
 ===================
 ��Ր���F���
 �ǋL�ҁF����
 �{�^����I�������ۂɓ��삷��X�N���v�g
 ===================
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//- �{�^���I�����ɓ��삷��N���X
public class SatoButton : MonoBehaviour
{
    [SerializeField, Header("�V�[���J�ڐ�")] private SceneObject NextScene;          //�V�[���J�ڐ�
    [SerializeField, Header("�t�F�[�h�J�n�x������")] private float DelayTime;         //�t�F�[�h���Ăяo�����܂ł̒x������
    [SerializeField, Header("�t�F�[�h�I�u�W�F�N�g")] private GameObject fadeObject;   //�t�F�[�h�p�I�u�W�F�N�g
    [SerializeField, Header("�t�F�[�h�����܂ł̎���")] private float FadeTime;        //�t�F�[�h��������

    private BGMManager  bgmManager;
    private Button      button;
    private ButtonAnime buttonAnime;
    private AnimePlayer animePlayer;
    private bool Load        = false;  // ���d���[�h�}�~
    public static bool Input = false;  // ���͔���
    void Awake()
    {
        buttonAnime = GetComponent<ButtonAnime>();
        button      = GetComponent<Button>();
        bgmManager  = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        Input       = false;
    }
    
    /// <summary>
    /// �V�[���J�ڂ��s������
    /// ���I��
    /// </summary>
    public void MoveSatoScene()
    {
        //- ���̓��͂��s���Ă����珈�����Ȃ�
        if (BackScene.Input)
        { return; }
        //- �Ăяo���ςȂ�s��Ȃ�
        if (Load)
        { return; }
        //- �Ăяo���ςɂ���
        Load = true;

        //- ���͂���������t���O�ύX
        Input = true;
        //- �Ăяo���ꂽ��㉺���E�I���𖳌���
        Navigation NoneNavigation = button.navigation;
        NoneNavigation.selectOnUp = null;
        NoneNavigation.selectOnDown = null;
        NoneNavigation.selectOnLeft = null;
        NoneNavigation.selectOnRight = null;
        button.navigation = NoneNavigation;

        //- �{�^���̓��͂��󂯕t���Ȃ�
        button.interactable = false;

        //- ���o�̕`��t���O�����Z�b�g
        if (CutIn.MoveCompleat) { CutIn.ResetMoveComplete(); }
        if (BoardMove.MoveComplete) { BoardMove.ResetMoveComplete(); }
        if (OpeningAnime.MoveCompleat) { OpeningAnime.ResetMoveComplete(); }

        //- �N���b�N���Đ�
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);

        //- �v���C���[�A�j���[�V�������Đ�����
        animePlayer = GetComponent<AnimePlayer>();
        animePlayer.SetAnime();

        //- �x����̏���
        DOVirtual.DelayedCall(DelayTime, () => fadeObject.GetComponent<ObjectFade>().SetFade(ObjectFade.FadeState.In, FadeTime));
        DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager()).SetDelay(DelayTime);
        DOVirtual.DelayedCall(FadeTime, () => SceneManager.LoadScene(NextScene)).SetDelay(DelayTime);
    }
}