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
    [SerializeField, Header("type��Stage�̎��̂ݗ��p")]
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
        // ----- �I������Ă���I�u�W�F�N�g�����邩�m�F
        selectObj = eventSystem.currentSelectedGameObject;
        if (selectObj == null) { return; }

        // ----- �I������Ă���I�u�W�F�N�g���ς�����ۂ̃t���O�ݒ�
        prevSelectName = nowSelectName;
        nowSelectName = selectObj.name;
        if (nowSelectName != prevSelectName) {
            isChange = true;
        }

        // ----- �ύX���Ȃ��ꍇ�������I������
        if (!isChange) { return; }

        // ----- �ύX����������
        // �t���O���Z�b�g
        isChange = false;
        // �X�e�[�W�I�����̍X�V
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
    /// ���݂̑I�����Ă�{�^���ԍ���ԋp
    /// </summary>
    /// <returns selectNum></returns>
    public int GetSelectNum()
    {   return selectNum;   }
}
