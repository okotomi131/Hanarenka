using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGamePopUp : MonoBehaviour
{
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button NextButton;
    [SerializeField]
    private Button EndButton;

    private Button EnterButton;
    private Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        EnterButton = GameObject.Find("ButtonEnter").GetComponent<Button>();
        BackButton = GameObject.Find("ButtonBack").GetComponent<Button>();

        gameObject.SetActive(false);
    }

    // ポップアップを表示する
    public void PopUpOpen()
    {
        gameObject.SetActive(true); // ポップアップのオブジェクトを有効化

        EnterButton.interactable = true;
        BackButton.interactable = true;

        // イージング設定
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);

        // ポップアップ内のボタン以外を無効化
        StartButton.interactable = false;
        NextButton.interactable = false;
        EndButton.interactable = false;

        // ボタンの初期選択
        gameObject.transform.Find("ButtonEnter").GetComponent<Button>().Select();
        return;
    }

    public void PopUpClose()
    {
        EnterButton.interactable = false;
        BackButton.interactable = false;

        // イージング設定
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.transform.DOScale(0.0f, 0.5f).SetEase(Ease.InCubic).OnComplete(() => { gameObject.SetActive(false); }); // 終了時オブジェクト無効化

        // ポップアップ内のボタン以外を有効化
        StartButton.interactable = true;
        NextButton.interactable = true;
        EndButton.interactable = true;

        // ニューゲームボタン選択
        StartButton.GetComponent<Button>().Select();
        return;
    }
}
