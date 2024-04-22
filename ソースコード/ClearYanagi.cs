using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearYanagi : MonoBehaviour
{
    [SerializeField, Header("火花用のオブジェクト")]
    private GameObject particle;
    [SerializeField, Header("エフェクト生成遅延(秒)")]
    private float startDelayTime;
    [SerializeField, Header("エフェクトの長さ(秒)")]
    private float effectTime;

    private float countTime = 0;

    //- アクティブ時に実行
    void OnEnable()
    {
        if(!SceneChange.bIsChange)
        { return; }

        //- エフェクトの生成数を計算
        float maxEffect = effectTime / 0.1f;

        DOVirtual.DelayedCall(startDelayTime, //- 生成遅延の時間後に実行
            () => StartCoroutine(MakeYanagiEffect(0.1f,(int)maxEffect)) //- コルーチンをスタート
            );

        //- エフェクトが終了後、自身を非アクティブ化
        DOVirtual.DelayedCall(startDelayTime + effectTime, () => this.gameObject.SetActive(false));
    }

    //- 柳に遅延をかけて生成
    private IEnumerator MakeYanagiEffect(float delayTime, int maxEffect)
    {
        //- エフェクトを生成
        for (int i = 0; i < maxEffect; i++)
        {
            //- delayTime秒待機する
            yield return new WaitForSeconds(delayTime);
            //- エフェクト生成のために、座標を取得
            Vector3 pos = transform.position;
            pos.x = transform.position.x + 0.5f;
            //- 生成位置をずらす
            pos.y += 1.0f;//1.6f;
            //- 指定した位置に生成
            GameObject fire = Instantiate(particle, pos, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }
    }
}
