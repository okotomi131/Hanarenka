/*
 ===================
 基盤制作：大川
 ゲームの進行度によって雲の描画を変更するスクリプト
 ===================
 */
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ClowdAlpha : MonoBehaviour
{
    [SerializeField, Header("現在進行中の煙のアルファ値")]
    private float AlphaNum = 0.5f;
    [SerializeField, Header("現在の里のボスステージ")]
    private int CurrentStageNum = -1;
    [SerializeField,Header("前の里のボスステージ")]
    private int BeforStageNum = -1;

    private Image img;
    private SaveManager save;
    private bool read = false;
    private void Awake()
    {
        img = GetComponent<Image>();
        save = FindObjectOfType<SaveManager>();
    }
    void Update()
    {

        //- 今のステージをクリアしているか
        if(!read && save.GetStageClear(CurrentStageNum))
        {
            img.DOFade(0.0f, 0.0f);
            read = true;
        }
        //- 前のステージが-1でない、前のステージクリアしているか
        else if(!read && BeforStageNum > 0 && save.GetStageClear(BeforStageNum))
        {
            img.DOFade(AlphaNum, 0.0f);
            read = true;
        }
        //- 前のステージに-1にが設定されているか
        else if(!read && BeforStageNum == -1)
        {
            img.DOFade(AlphaNum, 0.0f);
            read = true;
        }
        else if(!read )
        {
            img.DOFade(1.0f, 0.0f);
            read = true;
        }
    }

}
