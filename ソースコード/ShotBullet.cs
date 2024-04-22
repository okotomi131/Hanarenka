using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour {

    [SerializeField, Header("弾用のオブジェクト")]
    private GameObject bulletObject;

    int bulletNum;

    // Start is called before the first frame update
    void Start() {
        bulletNum = 0;
    }

    //- 弾を発射する関数
    //- 引数：bool 右に撃つかどうか
    //        Vector2 発射方向ベクトルとその強さ
    public void Shot(Vector2 ShotDirVector) {
        if (bulletNum <= 0) { return; }

        bulletNum -= 1;

        //- 右側への発射ベクトルを生成
        Vector3 pos = new(1.0f, 0.5f, 0.0f);
        //- 発射方向ベクトルの格納用
        Vector3 ForceDirection = new Vector3(ShotDirVector.x, ShotDirVector.y, 0.0f);
        //- 弾が左方向に打たれる場合、発射位置を左右反転する
        if (ShotDirVector.x < 0)
        { pos.x *= -1; }
        
        GameObject bullet = Instantiate(
                bulletObject,     // 生成(コピー)する対象
                transform.position + pos,     // 生成される位置
                Quaternion.Euler(0.0f, 0.0f, 0.0f)    // 最初にどれだけ回転するか
        );
        bullet.GetComponent<FireworksModule>().enabled = true;
        bullet.GetComponent<Rigidbody>().AddForce(ForceDirection);
    }

    public void AddBullet(int num) {
        bulletNum += num;
    }
}
