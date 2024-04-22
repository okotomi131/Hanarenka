/*
 ===================
 基盤制作：大川
 ゲーム終了スクリプト
 ===================
 */
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("終了");
    }   
}
