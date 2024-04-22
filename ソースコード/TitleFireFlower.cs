using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイトル用
// 位置x,y,z、時間をランダムで”出現するオブジェクトを生成”

public class TitleFireFlower : MonoBehaviour
{
    [SerializeField, Header("出現させるオブジェクト")]
    private GameObject obj;
    private float time;
    private float tRnd;

    private float PosX;
    private float PosY;
    private float PosZ;
    private float PosRnd;

    private bool LR;

    // Start is called before the first frame update
    void Start()
    {
        time = 1.0f;
        tRnd = 0;
        PosRnd = 0.0f;
        PosX = Random.Range(45.0f, 60.0f);
        PosY = Random.Range(0.1f, 10.9f);
        PosZ = this.transform.position.z;
        LR = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0.0f)
        {
            Instantiate(obj, new Vector3(PosX, PosY, PosZ), obj.transform.rotation);

            tRnd = Random.Range(0.25f, 3.25f);
            time = tRnd;

            if (LR)
            {
                PosX = Random.Range(100.0f, 115.0f);
                LR = false;
            }
            else
            {
                PosX = Random.Range(-85.0f, -70.0f);
                LR = true;
            }
            PosY = Random.Range(-40.0f, 30.0f);
            PosZ = Random.Range( 50.0f, 200.0f);
            //Debug.Log(PosX);
            //Debug.Log(PosY);
            //Debug.Log(PosZ);
        }
        
    }
}
