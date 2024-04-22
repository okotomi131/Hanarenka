using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイトル用
// 即座に爆発させて爆発が終わればオブジェクトを消す

public class TitleExplosion : MonoBehaviour
{
    [SerializeField, Header("火花用のオブジェクト")]
    private GameObject ParticleObjectA;
    [SerializeField]
    private GameObject ParticleObjectB;

    //[Header("爆発させるオブジェクト"), SerializeField]
    void Start()
    {
        if ((Random.value) % 2 <= 0.5f)
        {
            Instantiate(
                    ParticleObjectA,                     // 生成(コピー)する対象
                    transform.position,           // 生成される位置
                    Quaternion.Euler(0.0f, 0.0f, 0.0f)  // 最初にどれだけ回転するか
                    );
        }
        else
        {
            Instantiate(
                    ParticleObjectB,                     // 生成(コピー)する対象
                    transform.position,           // 生成される位置
                    Quaternion.Euler(0.0f, 0.0f, 0.0f)  // 最初にどれだけ回転するか
                    );
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Destroy(this.gameObject);
    }
}
