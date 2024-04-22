/*
 ===================
 ��Ր���F���
 �^�O�ŃI�u�W�F�N�g�̎c�������J�E���g����X�N���v�g
 ===================
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountEnemy : MonoBehaviour
{
    [SerializeField, Header("�J�E���g����^�O�̑I��")]
    private CountTag SelectedTag;

    private int CurrentCountNum;

    public enum CountTag
    {
        Fireworks,  //�ԉ�
        Player,     //�v���C���[

        Untagged    //���I���^�O
    }


    
    void Start()
    {
        GameObject[] SelectObject =
            GameObject.FindGameObjectsWithTag(SelectedTag.ToString());
        CurrentCountNum = SelectObject.Length;
    }

    /// <summary>
    /// SelectTag�őI�����ꂽ�^�O�𐔂��ATMPro�ɕ`�悷��
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
