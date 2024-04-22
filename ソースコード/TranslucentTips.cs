using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TranslucentTips : MonoBehaviour
{
    Image tipsImage;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        tipsImage = GameObject.Find("BossTips").GetComponent<Image>();
        text = GameObject.Find("TipsText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            tipsImage.DOFade(0.1f, 0.5f);
            text.alpha = 0.1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            tipsImage.DOFade(1.0f, 0.5f);
            text.alpha = 1.0f;
        }
    }
}
