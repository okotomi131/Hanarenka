using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLink : MonoBehaviour
{
    [Header("�����N����Q�[���I�u�W�F�N�g"), SerializeField]
    private GameObject obj;
    private Transform trans;
    
    void Start()
    {
        //- �����N��̃g�����X�t�H�[���擾
        trans = obj.transform;
    }

    void Update()
    {
        //- NULL�`�F�b�N
        if (obj)
        //- �g�����X�t�H�[���������N����
        this.transform.position = trans.position;
    }
}
