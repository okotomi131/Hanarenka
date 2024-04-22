/*
 ===================
 ��Ր���F���
 �Q�[���̐i�s�x�ɂ���ĉ_�̕`���ύX����X�N���v�g
 ===================
 */
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ClowdAlpha : MonoBehaviour
{
    [SerializeField, Header("���ݐi�s���̉��̃A���t�@�l")]
    private float AlphaNum = 0.5f;
    [SerializeField, Header("���݂̗��̃{�X�X�e�[�W")]
    private int CurrentStageNum = -1;
    [SerializeField,Header("�O�̗��̃{�X�X�e�[�W")]
    private int BeforStageNum = -1;

    private Image img;
    private SaveManager save;
    private bool read = false;
    private void Awake()
    {
        img = GetComponent<Image>();
        save = FindObjectOfType<SaveManager>();
    }
    void Update()
    {

        //- ���̃X�e�[�W���N���A���Ă��邩
        if(!read && save.GetStageClear(CurrentStageNum))
        {
            img.DOFade(0.0f, 0.0f);
            read = true;
        }
        //- �O�̃X�e�[�W��-1�łȂ��A�O�̃X�e�[�W�N���A���Ă��邩
        else if(!read && BeforStageNum > 0 && save.GetStageClear(BeforStageNum))
        {
            img.DOFade(AlphaNum, 0.0f);
            read = true;
        }
        //- �O�̃X�e�[�W��-1�ɂ��ݒ肳��Ă��邩
        else if(!read && BeforStageNum == -1)
        {
            img.DOFade(AlphaNum, 0.0f);
            read = true;
        }
        else if(!read )
        {
            img.DOFade(1.0f, 0.0f);
            read = true;
        }
    }

}
