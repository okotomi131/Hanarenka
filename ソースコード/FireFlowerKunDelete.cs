/*
 ===================
 ��Ր���F���
 �N���A���o���̉ԉ΋ʂ��폜����I�u�W�F�N�g
 ===================
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerKunDelete : MonoBehaviour
{
    private FireBelt Belt;
    private void Awake()
    {
        Belt = transform.parent.GetComponent<FireBelt>();
    }
    void Update()
    {
        if(Belt.GetDeleteFlag())
        {
            //- �������g���폜����
            Destroy(gameObject);
        }
    }
}
