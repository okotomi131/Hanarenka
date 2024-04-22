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
        { Destroy(gameObject); }// �I�u�W�F�N�g��j������
        else
        { DontDestroyOnLoad(gameObject); }// �V�[���J�ڂ��Ă��A�I�u�W�F�N�g��j�����Ȃ�
    }

    /// <summary>
    /// BGM���폜����
    /// </summary>
    public void DestroyBGMManager()
    { Destroy(gameObject); }

    public void DestroyPossible()
    {
        // DontDestroyOnLoad�ɔ������I�u�W�F�N�g���폜�\�ɂ���
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}