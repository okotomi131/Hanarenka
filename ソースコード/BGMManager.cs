using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    void Start()
    {
        int numMusicPlayers = FindObjectsOfType<BGMManager>().Length;
        if (numMusicPlayers > 1)
        { Destroy(gameObject); }// オブジェクトを破棄する
        else
        { DontDestroyOnLoad(gameObject); }// シーン遷移しても、オブジェクトを破棄しない
    }

    /// <summary>
    /// BGMを削除する
    /// </summary>
    public void DestroyBGMManager()
    { Destroy(gameObject); }

    public void DestroyPossible()
    {
        // DontDestroyOnLoadに避難させたオブジェクトを削除可能にする
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}