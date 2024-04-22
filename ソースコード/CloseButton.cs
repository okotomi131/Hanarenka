/*
 ===================
 基盤制作：大川
 クリア後紙芝居のボタン入力描画をするスクリプト
 ===================
 */
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private StoryFlip flip;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI tmp;
    private bool Active = false;

    private void Awake()
    {
        img.DOColor(new Vector4(0.3f, 0.3f, 0.3f, 1.0f),0.0f); 
        tmp.DOColor(new Vector4(0.3f, 0.3f, 0.3f, 1.0f), 0.0f);
    }

    private void Update()
    {
        if(flip.GetCurrentTime() >= 2.0f && !Active)
        {
            Active = true;
            img.DOColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 0.0f);
            tmp.DOColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 0.0f);
        }
    }
}
