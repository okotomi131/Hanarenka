using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatingMove : MonoBehaviour
{
    [SerializeField, Header("1フレームにつき更新する角度(360度)")]
    private float updateAngle = 1.0f;
    [SerializeField, Header("プレイヤーの回転移動の半径")]
    private float radius = 1.0f;

    private bool MoveKill = false; //- 移動停止フラグ
    private float nowAngle = 0;    //- 現在の角度

    void Start()
    {       
        //- プレイヤーの座標取得
        Vector3 pos = this.transform.position;

        //- 最初に回転移動の中心に来るように、半径分だけ下にずらす
        pos.y -= radius;

        //- プレイヤーの座標適用
        this.transform.position = pos;
    }

    void FixedUpdate()
    {
        if (MoveKill) return; //- 移動停止フラグがtrueならリターン

        //- 1フレーム前の角度を所持しておく
        float oldAngle = nowAngle;
        //- 現在の角度を更新
        nowAngle += updateAngle;

        //- それぞれのYの値を求める
        float nowY = radius * Mathf.Sin(nowAngle * Mathf.Deg2Rad);
        float oldY = radius * Mathf.Sin(oldAngle * Mathf.Deg2Rad);
        
        //- プレイヤーの座標取得
        Vector3 pos = this.transform.position;

        //- 現在と1フレーム前の値の差だけ、プレイヤーを移動させる
        pos.y += nowY - oldY;

        //- プレイヤーの座標適用
        this.transform.position = pos;
    }

    public void StopMove()
    {
        MoveKill = true;
    }
}
