using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPlayer : MonoBehaviour
{
    [SerializeField, Header("最初から待機状態かどうか")]
    private bool bIsWaitInit = true;
    [SerializeField, Header("一定時間後、操作可能にするか")]
    private bool bIsDelayWait = true;
    [SerializeField, Header("操作可能になるまでの時間(秒)")]
    private float MaxWaitTime = 1.5f;

    private bool bIsWaitNow;            //- プレイヤーが待機状態かどうか
    private float WaitTimeCount = 0.0f; //- タイムカウンタ
    private PController pCnt;           //- プレイヤー操作スクリプト
    
    void Start()
    {
        //- コンポーネントの取得
        pCnt = gameObject.GetComponent<PController>();
        //- コンポーネントのアクティブ設定
        pCnt.SetWaitFlag(bIsWaitInit);
        bIsWaitNow = bIsWaitInit;
    }

    void FixedUpdate()
    {
        if (!bIsDelayWait) return; //- アクティブフラグ遅延設定を処理しないリターン
        if (!bIsWaitNow)   return; //- プレイヤーが待機状態でないならリターン

        //- 時間更新
        WaitTimeCount += Time.deltaTime;
        //- 待機時間を過ぎていなければリターン
        if (WaitTimeCount < MaxWaitTime) return;

        //- プレイヤーを操作可能に変更
        pCnt.SetWaitFlag(false);
        bIsWaitNow = false;
    }

    //- プレイヤーが待機状態かどうかのアクティブ切り替えを行う関数
    public void SetWaitPlayer(bool Flag)
    {
        pCnt.SetWaitFlag(Flag);
    }
}