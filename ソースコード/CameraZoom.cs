using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraZoom : MonoBehaviour
{
    GameObject maincamera;
    Transform cameratrans;
    [SerializeField,Header("里ボタン")]
    public GameObject selectpoint;
    RectTransform selecttrans;
    Camera camerasize;

    // Start is called before the first frame update
    void Start()
    {
        maincamera = GameObject.Find("Main Camera");
        cameratrans = maincamera.GetComponent<Transform>();
        selecttrans = selectpoint.GetComponent<RectTransform>();
        camerasize = maincamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void OnClick()
    {
        DOTween.Sequence().AppendInterval(0.7f).Append(
        cameratrans.DOMove(
            selecttrans.position, // 移動終了地点
            2.0f                    // 演出時間
        )).Join(
        camerasize.DOOrthoSize(2.0f, 2.0f));
    }
}
