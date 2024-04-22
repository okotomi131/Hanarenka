using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonationWarpCollision : MonoBehaviour
{
    public Vector3 RayStartPos = new Vector3(0, 0, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Fireworks") return;
        float markdis = 2.0f;
        Vector3 markpos = RayStartPos;
        Debug.DrawRay(markpos, new Vector3(+markdis, +markdis, 0), Color.blue, 3.0f);
        Debug.DrawRay(markpos, new Vector3(+markdis, -markdis, 0), Color.blue, 3.0f);
        Debug.DrawRay(markpos, new Vector3(-markdis, +markdis, 0), Color.blue, 3.0f);
        Debug.DrawRay(markpos, new Vector3(-markdis, -markdis, 0), Color.blue, 3.0f);
        CheckHitRayStage(other.gameObject);
    }

    private void CheckHitRayStage(GameObject obj)
    {
        // 自身から花火に向かう方向を計算
        Vector3 direction = obj.transform.position - RayStartPos;
        // 自身と花火の長さを計算
        float DisLength = direction.magnitude;
        // 自身から花火に向かうレイを作成
        Ray ray = new Ray(RayStartPos, direction);
        // 当たったオブジェクトを格納するための変数
        var HitList = new List<RaycastHit>();

        // レイが当たったオブジェクトをすべて順番に確認していく
        foreach (RaycastHit hit in Physics.RaycastAll(ray, DisLength))
        {
            //- 最初のオブジェクトなら無条件で格納
            if (HitList.Count == 0)
            {
                HitList.Add(hit);
                continue;
            }

            //- 格納フラグ
            bool bAdd = false;
            //- 格納変数と当たったオブジェクトの比較
            for (int i = 0; i < HitList.Count; i++)
            {
                //- 格納フラグチェック
                if (bAdd) break;
                //- 距離が格納箇所データの距離より長ければリターン
                if (HitList[i].distance < hit.distance) continue;
                //- 仮のデータを一番最後に格納
                HitList.Add(new RaycastHit());
                //- 最後から格納場所までデータをずらす
                for (int j = HitList.Count - 1; j > i; j--)
                {
                    //- データを一つ移動
                    HitList[j] = HitList[j - 1];
                }
                //- 格納場所に格納
                HitList[i] = hit;
                bAdd = true;
            }

            //- 格納フラグが立っていなければ、一番距離が長いオブジェクトなので
            //- 配列の一番最後に格納する
            if (!bAdd) HitList.Add(hit);
        }
        bool WarpCheck = false;
        //- 距離が短いものから調べる
        for (int i = 0; i < HitList.Count; i++)
        {
            RaycastHit hit = HitList[i];

            //- 当たり判定のデバッグ表示
             float markdis = 0.1f;
             Debug.DrawRay(transform.position, direction, Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(+markdis, +markdis, 0), Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(+markdis, -markdis, 0), Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(-markdis, +markdis, 0), Color.green, 3.0f);
             Debug.DrawRay(hit.point, new Vector3(-markdis, -markdis, 0), Color.green, 3.0f);
            if (hit.collider.gameObject.tag != "Warphole") continue; //- ステージオブジェクト以外なら次へ
            if (hit.distance > DisLength) continue;               //- 花火玉よりステージオブジェクトが奥にあれば次へ

            WarpCheck = true;
        }
        if (!WarpCheck) return;
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

        // 当たり判定用のオブジェクトがあったら処理
        if (obj.transform.GetChild(0).name == "Collision")
        {
            //- 移動スクリプトがあれば処理
            if (obj.GetComponent<MovementManager>())
                obj.GetComponent<MovementManager>().SetStopFrag(true);
        }

        //- ステージオブジェクトに当たっていない
        return;
    }
}
