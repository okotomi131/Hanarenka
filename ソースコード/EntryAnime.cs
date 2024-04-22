/*
 ===================
 ��Ր���F���
 ��ʓ��ɃI�u�W�F�N�g��o��/�P�ނ�����X�N���v�g
 ===================
 */
using UnityEngine;
using DG.Tweening;

public class EntryAnime : MonoBehaviour
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

    private const float LEFT = -300.0f;
    private const float RIGHT = 2500.0f;
    private const float TOP = 1200.0f;
    private const float DOWN = -200.0f;

    [SerializeField, Header("�o�ꎞ�̕���")]
    private E_OUTDIRECTION Sdirection = E_OUTDIRECTION.UP;
    [SerializeField,Header("�o�ꎞ�̈ړ��J�n�܂ł̎���")]
    private float DelayTime;
    [SerializeField, Header("�o�ꎞ�̈ړ�����")]
    private float MoveTime;
    [SerializeField, Header("�ޏꎞ�̕���")]
    private E_OUTDIRECTION direction = E_OUTDIRECTION.LEFT;
    [SerializeField, Header("�ޏꎞ�̈ړ�����")]
    private float EndMoveTime;

    private Vector3 pos;
    private bool OutMoveLoad = false;
    private bool InGame = false;
    
    private void Awake()
    {
        //- �z�u�ʒu���L��
        pos = transform.position;
        //- �����ʒu���w������Ɉړ�������
        switch (Sdirection)
        {
            case E_OUTDIRECTION.LEFT:
                transform.position = new Vector3(LEFT, pos.y, pos.z);
                break;
            case E_OUTDIRECTION.RIGHT:
                transform.position = new Vector3(RIGHT, pos.y, pos.z);
                break;
            case E_OUTDIRECTION.UP:
                transform.position = new Vector3(pos.x, TOP, pos.z);
                break;
            case E_OUTDIRECTION.DOWN:
                transform.position = new Vector3(pos.x, DOWN, pos.z);
                break;
        }

    }

    /// <summary>
    /// �o�ꎞ�A�j���[�V����
    /// </summary>
    public void StartMove()
    {
        if(BoardMove.MoveComplete)
        {   DelayTime = 0.0f;   }
        //- ��ʓ��ɓo�ꂷ��
        switch (Sdirection)
        {
            case E_OUTDIRECTION.LEFT:
                DOTween.Sequence()
                     .AppendInterval(DelayTime)
                     .Append(transform.DOMoveX(pos.x, MoveTime));
                InGame = true;
                break;
            case E_OUTDIRECTION.RIGHT:
                DOTween.Sequence()
                     .AppendInterval(DelayTime)
                     .Append(transform.DOMoveX(pos.x, MoveTime));
                InGame = true;
                break;
            case E_OUTDIRECTION.UP:
                DOTween.Sequence()
                     .AppendInterval(DelayTime)
                     .Append(transform.DOMoveY(pos.y, MoveTime));
                InGame = true;
                break;
            case E_OUTDIRECTION.DOWN:
                DOTween.Sequence()
                     .AppendInterval(DelayTime)
                     .Append(transform.DOMoveY(pos.y, MoveTime));
                InGame = true;
                break;
        }
    }

    /// <summary>
    /// �Ăяo���ꂽ��P�ދ������s��
    /// </summary>
    public void OutMove()
    {
        if(!OutMoveLoad)
        {
            //- �w������ɓP��
            switch (direction)
            {
            case E_OUTDIRECTION.LEFT:
                    transform.DOMoveX(LEFT, EndMoveTime);
                break;
            case E_OUTDIRECTION.RIGHT:
                    transform.DOMoveX(RIGHT, EndMoveTime);
                break;
            }
            OutMoveLoad = true;
        }
    }

    /// <summary>
    /// �o�ꏈ�����s�������ԋp����
    /// </summary>
    /// <returns>bool:�o��t���O</returns>
    public bool GetEntryFlag()
    { return InGame; }
}
