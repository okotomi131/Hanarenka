using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 基盤制作 : 髙橋
 * 追記 : 牧野、中村
 * 概要 : オブジェクトをワープさせるスクリプト
 */
public class WarpGate : MonoBehaviour
{
    [SerializeField, Header("ワープゲート")] private GameObject WarpHole;
    [SerializeField, Header("ワープする位置")] private Transform warpPoint;
    [SerializeField, Header("ワープゲートの範囲")] private float radius = 1.0f;

    private void OnTriggerStay(Collider other)
    {
        //- ワープホールに接触したら指定した位置にワープする(プレイヤー)
        if (other.gameObject.name == "Player")
        {
            //- プレイヤーとワープゲートの距離を計算
            Vector3 playerPos = other.transform.position;
            Vector3 gatePos   = transform.position;
            float distance = Vector3.Distance(playerPos, gatePos);
            //- ワープから出てくる方向と位置調整のために入射角を取得
            Vector3 warpdistance = WarpHole.transform.position - other.transform.position;

            //- プレイヤーとワープゲートの距離が範囲内なら
            if (distance < radius)
            {
                //- コントローラーのコンポーネントを取得
                var cc = other.gameObject.GetComponent<CharacterController>();
                cc.enabled = false; // コントローラーの判定を無効にする
                //- プレイヤーを指定した位置にワープさせる
                other.transform.position = warpPoint.position + warpdistance * 1.2f;
                Vector3 pos = other.transform.position;
                pos.z = 0.0f;
                other.transform.position = pos;
                cc.enabled = true; // コントローラーの判定を有効にする
            }
        }
        //- ワープホールに接触したら指定した位置にワープする(復活プレイヤー)
        if (other.gameObject.name == "ResurrectionFireFlower(Clone)")
        {
            //- 復活プレイヤーとワープゲートの距離を計算
            Vector3 resPlayerPos = other.transform.position;
            Vector3 gatePos      = transform.position;
            float distance       = Vector3.Distance(resPlayerPos, gatePos);
            //- ワープから出てくる方向と位置調整のために入射角を取得
            Vector3 warpdistance = WarpHole.transform.position - other.transform.position;

            //- プレイヤーとワープゲートの距離が範囲内なら
            if (distance < radius)
            {
                var cc = other.gameObject.GetComponent<CharacterController>();
                cc.enabled = false; // コントローラーの判定を無効にする
                //- プレイヤーを指定した位置にワープさせる
                other.transform.position = warpPoint.position + warpdistance * 1.2f;
                Vector3 pos = other.transform.position;
                pos.z = 0.0f;
                other.transform.position = pos;
                cc.enabled = true; // コントローラーの判定を有効にする
            }
        }
        //- ワープホールに接触したら指定した位置にワープする(トンボ花火)
        if (other.gameObject.name == "DragonflyModule")
        {
            //- トンボ花火とワープゲートの距離を計算
            Vector3 DFPos   = other.transform.position;
            Vector3 gatePos = transform.position;
            float distance  = Vector3.Distance(DFPos, gatePos);
            //- ワープから出てくる方向と位置調整のために入射角を取得
            Vector3 warpdistance = WarpHole.transform.position - other.transform.position;

            //- プレイヤーとワープゲートの距離が範囲内なら
            if (distance < radius)
            {
                //- プレイヤーを指定した位置にワープさせる
                other.transform.position = warpPoint.position + warpdistance;
                Vector3 pos = other.transform.position;
                pos.z = 0.0f;
                other.transform.position = pos;
            }
        }
    }
}