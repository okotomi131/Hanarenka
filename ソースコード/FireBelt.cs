/*
 ===================
 ��Ր���F���
 �z�u�ʒu�������̈ʒu�܂ňړ�����X�N���v�g
 ===================
 */
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FireBelt : MonoBehaviour
{
    private enum E_MOVELOCATION
    {
        [InspectorName("�z�u�ʒu���w��ʒu")]
        FromCurrentToDestination,
        [InspectorName("�z�u�ʒu���ڕW�I�u�W�F�N�g�n�_")]
        FromCurrentToTarget,
        [InspectorName("�^�����z�u�ʒu")]
        FromDownToCurrent,
        [InspectorName("�ړ��Ȃ�")]
        NoMove
    };

    [Header("�����ݒ�")]
    [SerializeField, Header("�ړ����")]
    private E_MOVELOCATION movelocation = E_MOVELOCATION.FromCurrentToDestination;

    [Header("�z�u�ʒu����ړI�n�Ɍ������ۂ̐ݒ�")]
    [SerializeField,Header("���ݒn����ǂ��܂ňړ����邩")]
    private float AddPosY;

    [Header("�z�u�ʒu����ڕW�I�u�W�F�N�g�n�_�Ɍ������ۂ̐ݒ�")]
    [SerializeField, Header("�����������ʒu�ɂ���I�u�W�F�N�g")]
    private GameObject PentObj = null;
    [SerializeField, Header("�ړI�n���琔�l�������ړI�n�����炷")]
    private float DiffY = 7.0f;

    [Header("���ʐݒ�")]
    [SerializeField, Header("�ړ�����")]
    private float MoveTime;
    [SerializeField, Header("�O�Վ��k����")]
    private float DeleteTIme;
    [SerializeField, Header("�t�F�[�h����")]
    private float FadeTIme;
    [SerializeField, Header("�����œ���")]
    private bool Auto = false;

    private Image img;
    private Slider sli;
    private bool MoveComplete = false;
    private bool DeleteFlag = false;

    private void Update()
    {
        if(!MoveComplete && Auto)
        {   MoveLocation(); }
    }

    /// <summary>
    /// �w��ʒu�Ɉړ�����
    /// </summary>
    public void MoveLocation()
    {
        img = GetComponent<Image>();
        sli = GetComponent<Slider>();
        //- Y���W��ۑ����Ă���
        float pos = img.transform.localPosition.y;
        float TargetPos;
        MoveComplete = true;
        //- �ړ���ޕʏ���
        switch (movelocation)
        {
            //- �z�u�ʒu����w��ʒu�܂�
            case E_MOVELOCATION.FromCurrentToDestination:
                //- �w��ʒu��ۑ�
                TargetPos = pos + AddPosY;
                Animetion(TargetPos);
                break;

            //- �z�u�ʒu����ڕW�I�u�W�F�N�g�܂�
            case E_MOVELOCATION.FromCurrentToTarget:
                //- null�`�F�b�N
                if(PentObj == null)
                {   Debug.Log("�����������I�u�W�F�N�g���ݒ肳��Ă��܂���:FireBelt");    }
                //- ���ݒn�𐶐��������ʒu�̃I�u�W�F�N�g�ʒu�ɂ���
                img.transform.localPosition = new Vector3(
                    PentObj.transform.localPosition.x, 
                    img.transform.localPosition.y,
                    img.transform.localPosition.z);
                //- �ڕW�ʒu��ݒ肷��
                TargetPos = PentObj.transform.localPosition.y;
                //- �ړ�����
                Animetion(TargetPos);
                break;
                
            //- �^������z�u�ʒu�܂�
            case E_MOVELOCATION.FromDownToCurrent:
                //- ���ݒn��ۑ�
                TargetPos = pos;
                img.transform.localPosition
                        = new Vector3(img.transform.localPosition.x, -50, img.transform.localPosition.z);
                Animetion(TargetPos);
                break;
            case E_MOVELOCATION.NoMove:
                break;
        }
        
        
    }

    private void Animetion(float TargetPos)
    {
        //- �ړ�
        transform
            .DOLocalMoveY(TargetPos - DiffY, MoveTime)
            .SetEase(Ease.OutCubic)
            .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable)
            .OnPlay(() => { SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Belt); }) // �ł��グ���Đ�
            .OnComplete(() => { DeleteFlag = true; });
        //- ���X�ɉ摜��������
        img.DOFillAmount(0, DeleteTIme)
            .SetEase(Ease.InOutQuad)
            .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable);
        //- �t�F�[�h
        img.DOFade(0, FadeTIme)
            .SetEase(Ease.OutSine)
            .SetLink(this.gameObject, LinkBehaviour.PauseOnDisablePlayOnEnable);
    }

    public bool GetMoveComplete()
    {   return MoveComplete;    }

    public bool GetDeleteFlag()
    {   return DeleteFlag;   }
}
