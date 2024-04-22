using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnitionCollision : MonoBehaviour
{
    [Header("�ԉ΃X�N���v�g�I�u�W�F�N�g"), SerializeField]
    private GameObject moduleObj;
    private FireworksModule module; //- �ԉ΃X�N���v�g
    [Header("��e��̖��G����(�b)"), SerializeField]
    private float InvisibleTime;
    [Header("�폜����I�u�W�F�N�g"), SerializeField]
    private GameObject destroyObj;
    
    private DragonflyRayCheck raycheck; //- ���C�����蔻��X�N���v�g

    //- �C���X�y�N�^�[�������\���ɂ���
    [SerializeField, HideInInspector]
    public bool IsDestroy = false; //- �j��t���O

    private float TimeCount = 0; //- �^�C���J�E���^

    void Start()
    {
        //- �ԉ΃X�N���v�g�̎擾
        module = moduleObj.GetComponent<FireworksModule>();
        //- ���C�����蔻��X�N���v�g�̎擾
        raycheck = moduleObj.GetComponent<DragonflyRayCheck>();
    }
    void FixedUpdate()
    {
        //- �������Ă���̎��Ԃ��J�E���g
        if (module.IsExploded) TimeCount += Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        //- ���������I�u�W�F�N�g�̃^�O�ɂ���ď�����ς���
        if (other.gameObject.tag == "Fireworks") HitFireworks(other);
        if (other.gameObject.tag == "ExplodeCollision") HitExplodeCollision(other);
        if (other.gameObject.tag == "OutsideWall")
        {
            //- ���C�X�N���v�g���Ŏ��s����𕜊������Ă��Ȃ��Ȃ�A���s����𕜊�������
            if (!raycheck.isPlayback)
            {
                SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
                scenechange.RequestStopMiss(false);
            }
            Destroy(destroyObj);
        }

        //- �t���O�������Ă���Δj��
        if (IsDestroy)
        {
            //- ���C�X�N���v�g���Ŏ��s����𕜊������Ă��Ȃ��Ȃ�A���s����𕜊�������
            if (!raycheck.isPlayback)
            {
                SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
                scenechange.RequestStopMiss(false);
            }
            Destroy(destroyObj);
        }
    }
    void HitFireworks(Collider other)
    {
        //- �I�u�W�F�N�g�ɕϊ�
        GameObject obj = other.gameObject;

        //- ���������I�u�W�F�N�g��FireworksModule�̎擾
        FireworksModule module = obj.GetComponent<FireworksModule>();
        //- ���������I�u�W�F�N�g�̉ԉ΃^�C�v�ɂ���ď����𕪊�
        if (module.Type == FireworksModule.FireworksType.Boss)
            module.IgnitionBoss(obj);
        else if (module.Type != FireworksModule.FireworksType.ResurrectionPlayer)
            module.Ignition(transform.position);
        else if (module.Type == FireworksModule.FireworksType.ResurrectionPlayer)
            if (module.GetIsInv() == false)
            { module.Ignition(transform.position); }

        //- ���������I�u�W�F�N�g�̎�ނɂ���Ď��g������
        if (module.Type == FireworksModule.FireworksType.Normal ||
            module.Type == FireworksModule.FireworksType.Double)
        {
            //- ���C�X�N���v�g���Ŏ��s����𕜊������Ă��Ȃ��Ȃ�A���s����𕜊�������
            if (!raycheck.isPlayback)
            {
                SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
                scenechange.RequestStopMiss(false);
            }
            Destroy(destroyObj);
        }
    }
    void HitExplodeCollision(Collider other)
    {
        if (other.gameObject.name == "TonboCollision") return;
        //- ���G���Ԓ��Ȃ烊�^�[��
        if (TimeCount <= InvisibleTime) return;
        //- ���g�̔j��t���O��ύX
        IsDestroy = true;
    }
}
