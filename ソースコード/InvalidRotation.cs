using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidRotation : MonoBehaviour
{
    void Update()
    {
        //- ワールド座標で見た回転の取得
        Vector3 rot = transform.eulerAngles;
        //- 回転を打ち消す
        if (rot.x != 0) rot.x = 0;
        if (rot.y != 0) rot.y = 0;
        if (rot.z != 0) rot.z = 0;
        //- 回転の適用
        transform.eulerAngles = rot;
    }
}
