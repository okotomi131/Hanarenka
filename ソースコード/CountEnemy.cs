/*
 ===================
 基盤制作：大川
 タグでオブジェクトの残存数をカウントするスクリプト
 ===================
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountEnemy : MonoBehaviour
{
    [SerializeField, Header("カウントするタグの選択")]
    private CountTag SelectedTag;

    private int CurrentCountNum;

    public enum CountTag
    {
        Fireworks,  //花火
        Player,     //プレイヤー

        Untagged    //未選択タグ
    }


    
    void Start()
    {
        GameObject[] SelectObject =
            GameObject.FindGameObjectsWithTag(SelectedTag.ToString());
        CurrentCountNum = SelectObject.Length;
    }

    /// <summary>
    /// SelectTagで選択されたタグを数え、TMProに描画する
    /// </summary>
    void Update()
    {
        GameObject[] SelectObject =
                GameObject.FindGameObjectsWithTag(SelectedTag.ToString());
        CurrentCountNum = SelectObject.Length;
    }

    public int GetCurrentCountNum()
    {
        return CurrentCountNum;
    }
}
