using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private bool IsOnce = false;

    private void OnParticleCollision(GameObject other)
    {
        // 当たったオブジェクトのタグが「Fireworks」なら
        if (other.gameObject.tag == "Fireworks")
        {
            // 当たり判定用のオブジェクトがあったら
            if (other.gameObject.transform.GetChild(0).name == "Collision")
            {
                //- 当たり判定を有効化する
                // 当たったオブジェクトのColliderを有効にする
                other.gameObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                // 当たり判定の拡大用コンポーネントを有効にする
                other.gameObject.transform.GetChild(0).GetComponent<DetonationCollision>().enabled = true;
                //- 移動スクリプトがあれば処理
                if (other.gameObject.GetComponent<MovementManager>())
                    other.gameObject.GetComponent<MovementManager>().SetStopFrag(true);
            }
            // 当たったオブジェクトの爆発フラグを立てる
            other.gameObject.GetComponent<FireworksModule>().Ignition(transform.position);
        }
        // 当たったオブジェクトのタグが「ResurrectionBox」なら
        if (other.gameObject.tag == "ResurrectionBox")
        {
            // 当たったオブジェクトの爆発フラグを立てる 
            other.gameObject.GetComponent<FireworksModule>().Ignition(transform.position);
        }
    }
}