/*
 ===================
 基盤制作：大川
 オブジェクトを整列させるスクリプト
 ===================
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransformSetting : MonoBehaviour
{
    [SerializeField, Header("統一に配置したいオブジェクト")]
    private List<GameObject> objs;

    [SerializeField, Header("開始地点オブジェクト")]
    private GameObject StartObj = null;


    [SerializeField, Header("ずらす間隔")]
    private Vector3 shift;

    [SerializeField, Header("ずらし幅：上限値")]
    private float Upper;
    [SerializeField, Header("ずらし幅：下限値")]
    private float Lower;


    void Awake()
    {
        //- 開始地点のオブジェクトが登録されていない時
        if(StartObj == null)
        {
            //- 登録されていなかったらListの0番目を登録する
            StartObj = objs[0];
        }

        //- オブジェクトの位置を揃える
        objs[0].transform.position = StartObj.transform.position;

        //- 開始地点オブジェクトを参考に、等間隔にずらす
        for (int i = 0; i < objs.Count - 1; i++)
        {
            //- 次のオブジェクトを今のオブジェクトから設定数値分ずらす
            objs[i + 1].transform.position =
                new Vector3(
                    objs[i].transform.position.x + shift.x,
                    objs[i + 1].transform.position.y + shift.y,
                    objs[0].transform.position.z + shift.z);
        }
    }
}
