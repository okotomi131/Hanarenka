using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �A�^�b�`�����I�u�W�F�N�g��"������Ƃ�I�u�W�F�N�g"�Ɏw�肵���I�u�W�F�N�g���m�ɋ��܂ꂽ��
// "����������I�u�W�F�N�g"�𔚔�������

public class CrushObject : MonoBehaviour
{
    [Header("������Ƃ�I�u�W�F�N�g"), SerializeField]
    private GameObject obj1;
    [SerializeField]
    private GameObject obj2;

    [Header("����������I�u�W�F�N�g"), SerializeField]
    private FireworksModule fireworks;
    [Header("���̑�"), SerializeField]
    private PController pcontroller;
    [SerializeField]
    private SceneChange sceneChange;
    

    // �Փ˂��Ă���I�u�W�F�N�g���X�g
    private List<GameObject> hitObjects = new List<GameObject>();

    private bool checkobj1;
    private bool checkobj2;

    void Start()
    {
        checkobj1 = false;
        checkobj2 = false;
    }

    void Update()
    {
        if (hitObjects.Count >= 2)
        {
            // TODO:2�ȏ㓯���ɏՓ˂��Ă����ꍇ�̏���
            //- ��������
            fireworks.Ignition(transform.position);

            //- SceneChange�X�N���v�g�̃v���C���[�����t���O��false�ɂ���
            sceneChange.bIsLife = false;
        }

        // ����Փ˂����I�u�W�F�N�g���X�g���N���A����
        hitObjects.Clear();
        checkobj1 = false;
        checkobj2 = false;
    }

    void OnTriggerStay(Collider other)
    {
        // �Փ˂��Ă���I�u�W�F�N�g�����X�g�ɓo�^����
        if (other.name == obj1.name && checkobj1 == false)
        {
            hitObjects.Add(other.gameObject);
            checkobj1 = true;
        }

        if (other.name == obj2.name && checkobj2 == false)
        {
            hitObjects.Add(other.gameObject);
            checkobj2 = true;
        }

    }
}
