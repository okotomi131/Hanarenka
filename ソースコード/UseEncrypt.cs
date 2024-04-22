using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEncrypt : MonoBehaviour
{
    [SerializeField, Header("暗号化フラグ")]
    private bool isEncrypt = true;

    private void OnValidate()
    {
        SaveManager save = GetComponent<SaveManager>();
        save.SetEncryptFlag(isEncrypt);
        //Debug.Log(isEncrypt);
    }
}
