using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // 当たったオブジェクトのタグが「花火」 又は 「打った花火」なら
        if (collision.gameObject.tag == "Fireworks" ||
            collision.gameObject.tag == "ShotFireworks")
        {
            Debug.Log("BulletCollision:Hit");
            // 当たったオブジェクトのFireworksModuleスクリプトを有効にする
            collision.gameObject.GetComponent<FireworksModule>().enabled = true;
            // 当たったオブジェクトのFireworksModuleスクリプト内のisExplodedをtrueに変える
            collision.gameObject.GetComponent<FireworksModule>().Ignition(transform.position);
        }
    }
}
