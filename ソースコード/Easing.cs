//======================================================================================
// ～イージングの使い方～
// 
// 1. float型で補完後の値を受け取る変数を作る。
// 
// 2. Easingクラス内のEasingFanc()に以下の順番で引数を入れる。
//   ・イージングの種類 Easing.EaseType型
//   ・補完する初期値
//   ・補完する終了値
//   ・補完終了までの時間
//   ・補完開始から現在の経過時間
// 
// 3. 1で作った変数に2の戻り値を代入する。
// 
// (例) 0から5までを3秒かけて遷移するプログラム
// 
// float hoge;
// float time;
// float totalTime;
//
// void Start()
// {
//     hoge = 0f;
//     time = 0f;
//     totalTime = 3f;
// }
// 
// void Update()
// {
//     if (time <= totalTime)
//     {
//         time += Time.deltaTime;
//     }
// 
//     hoge = Easing.EasingFunc(Easing.EaseType.Liner, 0, 5, totalTime, time);
// }
//=======================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Easing
{
    public enum EaseType
    {
        Liner,

        InSine,
        OutSine,
        InOutSine,

        InQuad,
        OutQuad,
        InOutQuad,

        InCubic,
        OutCubic,
        InOutCubic,

        InQuart,
        OutQuart,
        InOutQuart,

        InQuint,
        OutQuint,
        InOutQuint,

        InExpo,
        OutExpo,
        InOutExpo,

        InCirc,
        OutCirc,
        InOutCirc,

        InBack,
        OutBack,
        InOutBack,

        InElastic,
        OutElastic,
        InOutElastic,

        InBounce,
        OutBounce,
        InOutBounce
    };

    // back用
    const float c1 = 1.70158f;
    const float c2 = c1 * 1.525f;
    const float c3 = c1 + 1;
    const float c4 = (2 * Mathf.PI) / 3;
    const float c5 = (2 * Mathf.PI) / 4.5f;

    const float n1 = 7.5625f;
    const float d1 = 2.75f;

    private static float EaseOutBounce(float t, float v, float start)
    {
        float ret = start;

        if (t < 1 / d1) {
            ret = v * (n1 * t * t) + start;
        }
        else if (t < 2 / d1) {
            ret = v * (n1 * (t -= 1.5f / d1) * t + 0.75f) + start;
        }
        else if (t < 2.5 / d1) {
            ret = v * (n1 * (t -= 2.25f / d1) * t + 0.9375f) + start;
        }
        else {
            ret = v * (n1 * (t -= 2.625f / d1) * t + 0.984375f) + start;
        }

        return ret;
    }

    // 入力値に沿って補完する関数
    // @ Ret : 補完した数値
    // @ Arg1: イージングの種類
    // @ Arg2: 開始時の数値
    // @ Arg3: 終了時の数値
    // @ Arg4: 変化にかかる時間
    // @ Arg5: 現在の経過時間
    public static float EasingFunc(EaseType easeType, float start, float end, float totalTime, float currentTime)
    {
        float ret = start;

        float t = currentTime / totalTime;
        float v = end - start;

        switch (easeType)
        {
            case EaseType.Liner:
                ret = v * t + start; 
                break;

            case EaseType.InSine:
                ret = v * (1 - Mathf.Cos(( t * Mathf.PI) / 2)) + start;
                break;

            case EaseType.OutSine:
                ret = v * (Mathf.Sin((t * Mathf.PI) / 2)) + start;
                break;

            case EaseType.InOutSine:
                ret = v * (-(Mathf.Cos(Mathf.PI * t) - 1) / 2) + start;
                break;

            case EaseType.InQuad:
                ret = v * (t * t) + start;
                break;

            case EaseType.OutQuad:
                ret = v * (1 - (1 - t) * (1 - t)) + start;
                break;

            case EaseType.InOutQuad:
                ret = v * (t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2) + start;
                break;

            case EaseType.InCubic:
                ret = v * (t * t * t) + start;
                break;

            case EaseType.OutCubic:
                ret = v * (1 - Mathf.Pow(1 - t, 3)) + start;
                break;

            case EaseType.InOutCubic:
                ret = v * (t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2) + start;
                break;

            case EaseType.InQuart:
                ret = v * (t * t * t * t) + start;
                break;

            case EaseType.OutQuart:
                ret = v * (1 - Mathf.Pow(1 - t, 4)) + start;
                break;

            case EaseType.InOutQuart:
                ret = v * (t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2) + start;
                break;

            case EaseType.InQuint:
                ret = v * (t * t * t * t * t) + start;
                break;

            case EaseType.OutQuint:
                ret = v * (1 - Mathf.Pow(1 - t, 5)) + start;
                break;

            case EaseType.InOutQuint:
                ret = v * (t < 0.5 ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2) + start;
                break;

            case EaseType.InExpo:
                ret = v * (t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10)) + start;
                break;

            case EaseType.OutExpo:
                ret = v * (t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t)) + start;
                break;

            case EaseType.InOutExpo:
                ret = v * (t == 0 ? 0 : t == 1 ? 1 : t < 0.5 ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2) + start;
                break;

            case EaseType.InCirc:
                ret = v * (1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2))) + start;
                break;

            case EaseType.OutCirc:
                ret = v * (Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2))) + start;
                break;

            case EaseType.InOutCirc:
                ret = v * (t < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2) + start;
                break;

            case EaseType.InBack: 
                ret = v * (c3 * t * t * t - c1 * t * t) + start;
                break;

            case EaseType.OutBack:
                ret = v * (1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2)) + start;
                break;

            case EaseType.InOutBack:
                ret = v * (t < 0.5 ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2 : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2) + start;
                break;

            case EaseType.InElastic:
                ret = v * (t == 0 ? 0 : t == 1 ? 1 : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c4)) + start;
                break;

            case EaseType.OutElastic:
                ret = v * (t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1) + start;
                break;

            case EaseType.InOutElastic:
                ret = v * (t == 0 ? 0 : t == 1 ? 1 : t < 0.5 ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1) + start;
                break;

            case EaseType.InBounce:
                ret = EaseOutBounce(1 - t, v, start);
                break;

            case EaseType.OutBounce:
                ret = EaseOutBounce(t, v, start);
                break;

            case EaseType.InOutBounce:
                ret = v * (t < 0.5 ? (1 - EaseOutBounce(1 - 2 * t, 1, 0)) / 2 : (1 + EaseOutBounce(2 * t - 1, 1, 0)) / 2) + start;
                break;

            default:
                break;
        }

        return ret;
    }
}
