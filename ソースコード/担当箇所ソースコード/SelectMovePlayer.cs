/*
 ===================
 ��Ր���F���
 �ǋL�@�@�F�q��
 �X�e�[�W�I�𒆂̃v���C���[�������s���X�N���v�g
 ===================
 */
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class SelectMovePlayer : MonoBehaviour,ISelectHandler
{
    [SerializeField, Header("�ړ�����")]
    private float MoveTIme = 1.0f;
    [SerializeField, Header("�v���C���[")]
    private GameObject player;
    private Vector3 StagePos;

    public void OnSelect(BaseEventData eventData)
    {
        Vector3 pos = eventData.selectedObject.transform.position;
        var Move = DOTween.Sequence();
        Move.Append(player.transform.DOMove(pos,MoveTIme))
            .OnComplete(() =>
            {   Move.Kill();});
    }

    public void InStageMove()
    {
        player.GetComponent<PlayerFloatingMove>().StopMove();
        DOTween.Sequence()
            .Append(player.transform.DOMoveY(-2.0f, 0.5f).SetRelative(true))
            .AppendInterval(0.25f)
            .Append(player.transform.DOMoveY(20.0f, 1.5f).SetRecyclable(true));
    }
}
