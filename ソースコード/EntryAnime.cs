/*
 ===================
 基盤制作：大川
 画面内にオブジェクトを登場/撤退させるスクリプト
 ===================
 */
using UnityEngine;
using DG.Tweening;

public class EntryAnime : MonoBehaviour
{
    private enum E_OUTDIRECTION
    {
        [Header("左")]
        LEFT,
        [Header("右")]
        RIGHT,
        [Header("上")]
        UP,
        [Header("下")]
        DOWN,
    }

    private const float LEFT = -300.0f;
    private const float RIGHT = 2500.0f;
    private const float TOP = 1200.0f;
    private const float DOWN = -200.0f;

    [SerializeField, Header("登場時の方向")]
    private E_OUTDIRECTION Sdirection = E_OUTDIRECTION.UP;
    [SerializeField,Header("登場時の移動開始までの時間")]
    private float DelayTime;
    [SerializeField, Header("登場時の移動時間")]
    private float MoveTime;
    [SerializeField, Header("退場時の方向")]
    private E_OUTDIRECTION direction = E_OUTDIRECTION.LEFT;
    [SerializeField, Header("退場時の移動時間")]
    private float EndMoveTime;

    private Vector3 pos;
    private bool OutMoveLoad = false;
    private bool InGame = false;
    
    private void Awake()
    {
        //- 配置位置を記憶
        pos = transform.position;
        //- 初期位置を指定方向に移動させる
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
    /// 登場時アニメーション
    /// </summary>
    public void StartMove()
    {
        if(BoardMove.MoveComplete)
        {   DelayTime = 0.0f;   }
        //- 画面内に登場する
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
    /// 呼び出されたら撤退挙動を行う
    /// </summary>
    public void OutMove()
    {
        if(!OutMoveLoad)
        {
            //- 指定方向に撤退
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
    /// 登場処理を行ったか返却する
    /// </summary>
    /// <returns>bool:登場フラグ</returns>
    public bool GetEntryFlag()
    { return InGame; }
}
