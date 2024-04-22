using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // ���������I�u�W�F�N�g�̃^�O���u�ԉ΁v ���� �u�ł����ԉ΁v�Ȃ�
        if (collision.gameObject.tag == "Fireworks" ||
            collision.gameObject.tag == "ShotFireworks")
        {
            Debug.Log("BulletCollision:Hit");
            // ���������I�u�W�F�N�g��FireworksModule�X�N���v�g��L���ɂ���
            collision.gameObject.GetComponent<FireworksModule>().enabled = true;
            // ���������I�u�W�F�N�g��FireworksModule�X�N���v�g����isExploded��true�ɕς���
            collision.gameObject.GetComponent<FireworksModule>().Ignition(transform.position);
        }
    }
}
