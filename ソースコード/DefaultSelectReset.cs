using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSelectReset : MonoBehaviour
{
    [SerializeField, Header("里セレのリセット")]
    private bool satoSelectResetFlag;
    [SerializeField, Header("ステセレのリセット")]
    private bool stageSelectResetFlag;

    public void OnReset()
    {
        if (satoSelectResetFlag) {
            DefaultSelectStage.lastSelectSato = -1;
        }

        if (stageSelectResetFlag) {
            DefaultSelectStage.lastSelectStage = -1;
        }
    }

    public void OnNext()
    {
        if (DefaultSelectStage.lastSelectStage > 10) { return; }
        DefaultSelectStage.lastSelectStage++;
    }
}
