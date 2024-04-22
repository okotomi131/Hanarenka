using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSample : MonoBehaviour
{

    private GameObject player;   //�v���C���[���i�[�p
    private Vector3 offset;      //���΋����擾�p

    // Use this for initialization
    void Start()
    {

        //unitychan�̏����擾
        this.player = GameObject.Find("Body");

        // MainCamera(�������g)��player�Ƃ̑��΋��������߂�
        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float X = (player.transform.position.x - this.transform.position.x) * 0.01f;
        float Y = (player.transform.position.y - this.transform.position.y + 5.0f) * 0.01f;
        float NowX = this.transform.position.x + X;
        float NowY = this.transform.position.y + Y;
        transform.position = new Vector3(NowX, NowY, -20.0f);
    }
}