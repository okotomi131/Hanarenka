using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidRotation : MonoBehaviour
{
    void Update()
    {
        //- ���[���h���W�Ō�����]�̎擾
        Vector3 rot = transform.eulerAngles;
        //- ��]��ł�����
        if (rot.x != 0) rot.x = 0;
        if (rot.y != 0) rot.y = 0;
        if (rot.z != 0) rot.z = 0;
        //- ��]�̓K�p
        transform.eulerAngles = rot;
    }
}
