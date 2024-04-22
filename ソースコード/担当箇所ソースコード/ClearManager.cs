/*
 ===================
 制作：大川
 クリア情報を渡すスクリプト
 ===================
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//- クリア情報を受け渡すクラス
public class ClearManager : MonoBehaviour
{
    [SerializeField, Header("ステージ数")]   private int stagenum = -1;

    private SaveManager save;
    private bool write = false; //書き込みフラグ
    /// <summary>
    /// セーブファイルに書き込みを行えるかチェック
    /// </summary>
    public void WriteClear()
    {
        //- クリアフラグが立っているか
        if(SceneChange.bIsChange && !write)
        {
            save = new SaveManager();
            write = true;
            save.SetStageClear(stagenum);
            Debug.Log("書き込み" + "," + stagenum + "," + save.GetStageClear(stagenum));
        }
    }
}
