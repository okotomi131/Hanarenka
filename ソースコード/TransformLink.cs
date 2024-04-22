using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLink : MonoBehaviour
{
    [Header("リンクするゲームオブジェクト"), SerializeField]
    private GameObject obj;
    private Transform trans;
    
    void Start()
    {
        //- リンク先のトランスフォーム取得
        trans = obj.transform;
    }

    void Update()
    {
        //- NULLチェック
        if (obj)
        //- トランスフォームをリンクする
        this.transform.position = trans.position;
    }
}
