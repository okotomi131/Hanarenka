using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnitionCollision : MonoBehaviour
{
    [Header("花火スクリプトオブジェクト"), SerializeField]
    private GameObject moduleObj;
    private FireworksModule module; //- 花火スクリプト
    [Header("被弾後の無敵時間(秒)"), SerializeField]
    private float InvisibleTime;
    [Header("削除するオブジェクト"), SerializeField]
    private GameObject destroyObj;
    
    private DragonflyRayCheck raycheck; //- レイ当たり判定スクリプト

    //- インスペクター側から非表示にする
    [SerializeField, HideInInspector]
    public bool IsDestroy = false; //- 破壊フラグ

    private float TimeCount = 0; //- タイムカウンタ

    void Start()
    {
        //- 花火スクリプトの取得
        module = moduleObj.GetComponent<FireworksModule>();
        //- レイ当たり判定スクリプトの取得
        raycheck = moduleObj.GetComponent<DragonflyRayCheck>();
    }
    void FixedUpdate()
    {
        //- 爆発してからの時間をカウント
        if (module.IsExploded) TimeCount += Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        //- 当たったオブジェクトのタグによって処理を変える
        if (other.gameObject.tag == "Fireworks") HitFireworks(other);
        if (other.gameObject.tag == "ExplodeCollision") HitExplodeCollision(other);
        if (other.gameObject.tag == "OutsideWall")
        {
            //- レイスクリプト側で失敗判定を復活させていないなら、失敗判定を復活させる
            if (!raycheck.isPlayback)
            {
                SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
                scenechange.RequestStopMiss(false);
            }
            Destroy(destroyObj);
        }

        //- フラグがたっていれば破壊
        if (IsDestroy)
        {
            //- レイスクリプト側で失敗判定を復活させていないなら、失敗判定を復活させる
            if (!raycheck.isPlayback)
            {
                SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
                scenechange.RequestStopMiss(false);
            }
            Destroy(destroyObj);
        }
    }
    void HitFireworks(Collider other)
    {
        //- オブジェクトに変換
        GameObject obj = other.gameObject;

        //- 当たったオブジェクトのFireworksModuleの取得
        FireworksModule module = obj.GetComponent<FireworksModule>();
        //- 当たったオブジェクトの花火タイプによって処理を分岐
        if (module.Type == FireworksModule.FireworksType.Boss)
            module.IgnitionBoss(obj);
        else if (module.Type != FireworksModule.FireworksType.ResurrectionPlayer)
            module.Ignition(transform.position);
        else if (module.Type == FireworksModule.FireworksType.ResurrectionPlayer)
            if (module.GetIsInv() == false)
            { module.Ignition(transform.position); }

        //- 当たったオブジェクトの種類によって自身を消滅
        if (module.Type == FireworksModule.FireworksType.Normal ||
            module.Type == FireworksModule.FireworksType.Double)
        {
            //- レイスクリプト側で失敗判定を復活させていないなら、失敗判定を復活させる
            if (!raycheck.isPlayback)
            {
                SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
                scenechange.RequestStopMiss(false);
            }
            Destroy(destroyObj);
        }
    }
    void HitExplodeCollision(Collider other)
    {
        if (other.gameObject.name == "TonboCollision") return;
        //- 無敵時間中ならリターン
        if (TimeCount <= InvisibleTime) return;
        //- 自身の破壊フラグを変更
        IsDestroy = true;
    }
}
