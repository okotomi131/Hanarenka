/*
 ===================
 ��Ր���F���
 �ǋL�F�����E�q��
 �{�^���I���E��I���E�������ɃA�j���[�V��������X�N���v�g
 ===================
 */


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif


//- �{�^���A�j���[�V�����N���X
public class ButtonAnime : MonoBehaviour,
    ISelectHandler,     //�I����
    IDeselectHandler,   //��I����
    ISubmitHandler      //������
{

    [SerializeField] private Image image;            //�{�^���������ɓ��삷��摜
    [SerializeField] private TextMeshProUGUI tmp;    //�{�^���������ɓ��삷��e�L�X�g
    [SerializeField] private Color OverTextColor;    //�{�^���������Ƀe�L�X�g��ϐF������p
    [SerializeField] private float SelectTime = 0.25f;       //�I���A�j������������܂ł̎���
    private Color BaseTextColor;                     //���F

    public bool bPermissionSelectSE = true;          // �I��SE�̍Đ���������Ă��邩
    private Button button;                           
    private Vector2 BaseSize;                        //�z�u�T�C�Y�擾�ϐ�
    private bool bPush = false;                      //���͔���


    void Awake()
    {
        //- ����摜�����邩
        if (image == null)
        { return; }

        //- ����p�ϐ�������
        button = GetComponent<Button>();
        image.fillAmount = 0;
        BaseTextColor = tmp.color;
    }


    /// <summary>
    /// �{�^���I�����̏���
    /// </summary>
    /// <param name="eventData"></param>

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        //-�I�����Đ�
        if (bPermissionSelectSE)
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Select);
        else
            bPermissionSelectSE = true;

        if (image == null)
        { return; }

        image.DOFillAmount(1.0f, SelectTime).SetEase(Ease.OutCubic).Play();  
        tmp.DOColor(OverTextColor, 0.25f).Play();
    }


    /// <summary>
    /// �{�^����I�����̏���
    /// </summary>
    /// <param name="eventData"></param>

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if (image == null)
        { return; }

        //- �A�ő΍�t���O�������Ă��Ȃ���Ώ���
        if (!bPush)
        {
            image.DOFillAmount(0.0f, SelectTime).SetEase(Ease.OutCubic).Play();
            tmp.DOColor(BaseTextColor, 0.25f).Play();
        }
    }


    /// <summary>
    /// �{�^���������̏��� 
    /// </summary>
    /// <param name="eventData"></param>

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        //- �I�����Đ�
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Click);
    }

    /// <summary>
    /// �{�^�����������ɃA�j���[�V�����������s��
    /// </summary>

    public void PushButtonAnime()
    {   image.DOColor(new Color(1.0f,0.5f,0.5f), 0.25f);    }


    /// <summary>
    /// �������̈Ӑ}���Ȃ�������h���t���O�֐�
    /// </summary>
    /// <param name="flag"></param>

    public void SetbSelect(bool flag)
    { bPush = flag; }

    /*�@���[�[�[�[�[�[�g���R�[�h�[�[�[�[�[�[���@*/
#if UNITY_EDITOR
    //- Inspector�g���N���X
    [CustomEditor(typeof(ButtonAnime))] //�K�{
    public class ButtonAnimeEditor : Editor //Editor�̌p��
    {
        public override void OnInspectorGUI()
        {
            ButtonAnime btnAnm = target as ButtonAnime;
            EditorGUI.BeginChangeCheck();
            btnAnm.image 
                = (Image)EditorGUILayout.ObjectField("���삷��摜",btnAnm.image,typeof(Image),true);
            btnAnm.tmp
                = (TextMeshProUGUI)EditorGUILayout.ObjectField("�e�L�X�g", btnAnm.tmp, typeof(TextMeshProUGUI), true);
            btnAnm.OverTextColor
                = EditorGUILayout.ColorField("�J���[", btnAnm.OverTextColor);
            btnAnm.SelectTime
                = EditorGUILayout.FloatField("�A�j�������܂ł̎���", btnAnm.SelectTime);
            
            //- �C���X�y�N�^�[�̍X�V
            if(GUI.changed)
            {   EditorUtility.SetDirty(target); }
        }
    }
#endif

}
