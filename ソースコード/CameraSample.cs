using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSample : MonoBehaviour
{

    private GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用

    // Use this for initialization
    void Start()
    {

        //unitychanの情報を取得
        this.player = GameObject.Find("Body");

        // MainCamera(自分自身)とplayerとの相対距離を求める
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