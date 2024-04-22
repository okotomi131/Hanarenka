using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject targetObject; // 注視したいオブジェクト
    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(targetObject.transform);
    }
}
