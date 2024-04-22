/*
 ===================
 基盤制作：大川
 タイトルに戻るかを確認するポップアップを表示するスクリプト
 ===================
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VillagePop : MonoBehaviour
{
    [SerializeField] private List<Button> sato;
    [SerializeField] private DefaultSelectStage Dss;
    private Button EnterButton;
    private Button BackButton;
    private bool Open = false;
    private bool Close = false;
    void Start()
    {
        EnterButton = GameObject.Find("ButtonEnter").GetComponent<Button>();
        BackButton = GameObject.Find("ButtonBack").GetComponent<Button>();
        gameObject.SetActive(false);
    }

    // ポップアップを表示する
    public void PopUpOpen()
    {
        //- すでに開かれていたら処理しない
        if (Open) { return; }
        Open = true;


        gameObject.SetActive(true); // ポップアップのオブジェクトを有効化

        EnterButton.interactable = true;
        BackButton.interactable = true;

        // イージング設定
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.DOScale(1.0f, 0.35f).SetEase(Ease.OutBack).OnComplete(() => {Close = false;});


        //- 里を選択できないようにする
        for (int i = 0; i < sato.Count; i++)
        { sato[i].interactable = false; }

        // ボタンの初期選択
        gameObject.transform.Find("ButtonEnter").GetComponent<Button>().Select();
        return;
    }

    public void PopUpClose()
    {
        //- すでに閉じられていたら処理しない
        if (Close) { return; }
        Close = true;

        EnterButton.interactable = false;
        BackButton.interactable = false;

        // イージング設定
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.DOScale(0.0f, 0.35f).SetEase(Ease.InCubic).OnComplete(() => {
            Open = false;
            gameObject.SetActive(false); }); // 終了時オブジェクト無効化


        //- 里を選択できないようにする
        for (int i = 0; i < sato.Count; i++)
        { sato[i].interactable = true; }

        //- ポップ前に選択中だったボタンを選択状態にする
        sato[Dss.GetSelectNum()].Select();

        return;
    }
}
