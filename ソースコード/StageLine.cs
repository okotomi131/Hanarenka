using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ===================
 基盤制作：大川
 追記：髙橋
 里のアイコンを線で繋ぐスクリプト
 ===================
 */

public class StageLine : MonoBehaviour
{
    [SerializeField, Header("ステージ遷移用ボタンリスト")]
    private List<GameObject> StageList;
    [SerializeField, Header("ステージ間ライン")]
    private LineRenderer line;
    [SerializeField, Header("線の色")]
    private Color lineColor;
    [SerializeField, Header("線の太さ")]
    private float lineWidth = 0.2f;

    private void LineRender()
    {
        //- 端から端までの線の太さ
        line.startWidth = lineWidth;
        line.endWidth   = lineWidth;

        //- 端から端までの線の色
        line.startColor = lineColor;
        line.endColor   = lineColor;

        line.positionCount = StageList.Count;
        int LineCount = 0;
        foreach(GameObject sl in StageList)
        {
            line.SetPosition(LineCount, sl.transform.position);
            LineCount++;
        }
    }

    private void Awake()
    {
        LineRender();
    }
}
