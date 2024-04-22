using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : MonoBehaviour
{
    //- 振動の強さ
    float VibrationPower = 0;

    //- 振動のフレームカウント
    int nVibrationFrameCount = 0;

    [Header("毎フレーム弱まる割合(パーセント)"), SerializeField]
    private float weakRatio = 3;

    [Header("最低限の振動の強さ"), SerializeField]
    private float minVibration = 0.6f;

    //- ゲームパッド接続されているかどうか
    bool bIsSuccessConnection = true;

    //- ゲームパッド
    Gamepad gamepad;

    void Start()
    {
        //- ゲームパッド接続
        gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.Log("ゲームパッド未接続");
            bIsSuccessConnection = false;
        }
    }

    void FixedUpdate()
    {
        //- 未接続なら実行しない
        if (!bIsSuccessConnection) return;

        //- 振動フレームが残っているかどうかで分岐
        if (nVibrationFrameCount > 0)
        {
            //- 振動を設定
            gamepad.SetMotorSpeeds(VibrationPower, VibrationPower);
        }
        else
        {
            //- 振動フレームが終了したら振動を終わる
            gamepad.SetMotorSpeeds(0.0f, 0.0f);

        }

        //- 振動フレームカウントを進める
        nVibrationFrameCount--;

        //- 振動を弱める
        VibrationPower -= VibrationPower * (weakRatio / 100);

        //- 最低限の振動より弱くならないようにする
        if (minVibration > VibrationPower) { VibrationPower = minVibration; }

    }

    public void SetVibration(int nFrame, float power)
    {
        //- 振動の強さ設定
        VibrationPower = power;
        //- 振動のフレーム数が現在のフレーム数より長かったら更新
        if (nVibrationFrameCount < nFrame) { nVibrationFrameCount = nFrame; }
    }
}
