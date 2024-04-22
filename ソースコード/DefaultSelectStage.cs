using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultSelectStage : MonoBehaviour
{
    private enum SelectType
    {
        Sato,
        Stage
    }

    [SerializeField]
    private SelectType type;
    [SerializeField]
    private Button[] stageButton;
    [SerializeField]
    private SaveManager saveManager;
    [SerializeField, Header("typeがStageの時のみ利用")]
    private int stageNum;
    [SerializeField]
    private GameObject player;

    private EventSystem eventSystem;
    private int selectNum = 0;
    private bool isChange;
    private string nowSelectName;
    private string prevSelectName;
    private GameObject selectObj;

    public static int lastSelectSato = -1;
    public static int lastSelectStage = -1;

    void Start()
    {
        eventSystem = EventSystem.current;

        int satoValue = 1;
        int stageValue = 0;
        if (type == SelectType.Sato) satoValue *= 10;
        if (type == SelectType.Stage) stageValue = (stageNum - 1) * 10;

        if (type == SelectType.Sato && lastSelectSato >= 0) {
            selectNum = lastSelectSato;
        }
        else if (type == SelectType.Stage && lastSelectStage >= 0) {
            selectNum = lastSelectStage;
        }
        else {
            for (int i = 0; i < stageButton.Length - 1; i++) {
                if (saveManager.GetStageClear((i + 1) * satoValue + stageValue)) selectNum++;
            }

            if (type == SelectType.Sato && saveManager.GetStageClear(40)) {
                selectNum = 0;
            }

            if (type == SelectType.Stage && saveManager.GetStageClear(10 * stageNum)) {
                selectNum = 0;
            }
        }

        stageButton[selectNum].Select();
        player.transform.position = stageButton[selectNum].transform.position;

        Debug.Log(eventSystem.currentSelectedGameObject.name);
    }

    private void Update()
    {
        // ----- 選択されているオブジェクトがあるか確認
        selectObj = eventSystem.currentSelectedGameObject;
        if (selectObj == null) { return; }

        // ----- 選択されているオブジェクトが変わった際のフラグ設定
        prevSelectName = nowSelectName;
        nowSelectName = selectObj.name;
        if (nowSelectName != prevSelectName) {
            isChange = true;
        }

        // ----- 変更がない場合処理を終了する
        if (!isChange) { return; }

        // ----- 変更があった時
        // フラグリセット
        isChange = false;
        // ステージ選択情報の更新
        for (int i = 0;i < stageButton.Length;i++) {
            if (nowSelectName == stageButton[i].name) {
                switch(type) {
                case SelectType.Sato:
                    lastSelectSato = i;
                    break;
                case SelectType.Stage:
                    lastSelectStage = i;
                    break;
                }
                break;
            }
        }

        Debug.Log(selectObj.name);
    }

    /// <summary>
    /// 現在の選択してるボタン番号を返却
    /// </summary>
    /// <returns selectNum></returns>
    public int GetSelectNum()
    {   return selectNum;   }
}
