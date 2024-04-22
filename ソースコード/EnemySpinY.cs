using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpinY : MonoBehaviour
{
    //- 格納用のトランスフォーム
    Transform myTransform;

    [SerializeField, Header("1秒で回転する量(度)")]
    private float SecondSpinSpeed = 3.0f;

    //- 1フレームの移動量
    private float FrameSpinSpeed;

    //- 花火スクリプト
    FireworksModule fireworks;

    // Start is called before the first frame update
    void Start()
    {
        //- トランスフォームを取得
        myTransform = this.transform;
        //- 1フレームの移動量を計算
        FrameSpinSpeed = SecondSpinSpeed / 60;
        //- 花火点火スクリプトの取得
        fireworks = this.gameObject.GetComponent<FireworksModule>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //- 爆破時、回転スクリプトを無効化
        if (fireworks.IsExploded)
        {
            this.gameObject.GetComponent<EnemySpinY>().enabled = false;
        }

        //- ワールド回転を取得
        Vector3 worldAngle = myTransform.eulerAngles;
        //- ワールド系でZ軸回転
        worldAngle.z += FrameSpinSpeed;
        //- ワールド回転を反映
        myTransform.eulerAngles = worldAngle;
    }
}
