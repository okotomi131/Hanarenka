using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerQuickSpin : MonoBehaviour
{
    [SerializeField, Header("角度を全て周回した時にかかる時間(秒)")]
    private float GoAroundTime;

    [SerializeField, Header("現在角度と目標角度の差を補完する割合(パーセント)")]
    private float CompensatePercent;

    [SerializeField, Header("周回する角度(度) & 周回する数")]
    private float[] LookAngleNum;


    //- 現在が配列何番目の角度を目標としているか
    int nNowLookNum = 0;

    //- フレームカウント用、一周すると0でリセット
    int nFrameCount = 0;

    //- 一周にかかるフレーム数
    int nGoAroundFrame;

    //- 花火点火スクリプト
    FireworksModule fireworks;

    void Start()
    {
        //- 一周にかかるフレーム数を設定
        nGoAroundFrame = (int)GoAroundTime * 60;
        //- 花火点火スクリプトの取得
        fireworks = this.gameObject.GetComponent<FireworksModule>();
    }

    void FixedUpdate()
    {
        //- 爆破時、回転スクリプトを無効化
        if (fireworks.IsExploded)
        {
            enabled = false;
        }

        //- 現在が配列何番目の角度を目指しているか求める
        nNowLookNum = (nFrameCount / (nGoAroundFrame / LookAngleNum.Length));

        //- transformを取得
        Transform myTransform = this.transform;
        //- ワールド座標を基準に、回転を取得
        Vector3 worldAngle = myTransform.eulerAngles;
        //- ワールド座標を基準にした、y軸を軸にした回転角度の補完処理
        worldAngle.z += (LookAngleNum[nNowLookNum] - worldAngle.z) * (CompensatePercent / 100.0f);
        //- 回転の適用
        this.transform.eulerAngles = worldAngle;

        //- フレームカウントを進める
        nFrameCount++;
        if (nFrameCount >= nGoAroundFrame)
        {
            nFrameCount = 0;
        }
    }
}
