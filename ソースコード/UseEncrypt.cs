using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEncrypt : MonoBehaviour
{
    [SerializeField, Header("�Í����t���O")]
    private bool isEncrypt = true;

    private void OnValidate()
    {
        SaveManager save = GetComponent<SaveManager>();
        save.SetEncryptFlag(isEncrypt);
        //Debug.Log(isEncrypt);
    }
}
