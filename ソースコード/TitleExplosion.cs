using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �^�C�g���p
// �����ɔ��������Ĕ������I���΃I�u�W�F�N�g������

public class TitleExplosion : MonoBehaviour
{
    [SerializeField, Header("�Ήԗp�̃I�u�W�F�N�g")]
    private GameObject ParticleObjectA;
    [SerializeField]
    private GameObject ParticleObjectB;

    //[Header("����������I�u�W�F�N�g"), SerializeField]
    void Start()
    {
        if ((Random.value) % 2 <= 0.5f)
        {
            Instantiate(
                    ParticleObjectA,                     // ����(�R�s�[)����Ώ�
                    transform.position,           // ���������ʒu
                    Quaternion.Euler(0.0f, 0.0f, 0.0f)  // �ŏ��ɂǂꂾ����]���邩
                    );
        }
        else
        {
            Instantiate(
                    ParticleObjectB,                     // ����(�R�s�[)����Ώ�
                    transform.position,           // ���������ʒu
                    Quaternion.Euler(0.0f, 0.0f, 0.0f)  // �ŏ��ɂǂꂾ����]���邩
                    );
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Destroy(this.gameObject);
    }
}
