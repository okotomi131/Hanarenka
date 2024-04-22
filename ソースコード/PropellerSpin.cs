using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpin : MonoBehaviour
{   
    //- 格納用のトランスフォーム
    Transform myTransform;

    [SerializeField, Header("1秒で回転する量(度)")]
    private float SecondSpinSpeed = 3.0f;

    //- 1フレームの移動量
    private float FrameSpinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //- トランスフォームを取得
        myTransform = this.transform;
        //- 1フレームの移動量を計算
        FrameSpinSpeed = SecondSpinSpeed / 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //- ワールド回転を取得
        Vector3 worldAngle = myTransform.eulerAngles;
        //- ワールド系でY軸回転
        worldAngle.y += FrameSpinSpeed;
        //- ワールド回転を反映
        myTransform.eulerAngles = worldAngle;
    }
}
