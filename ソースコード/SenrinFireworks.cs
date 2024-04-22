using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenrinFireworks : MonoBehaviour
{
    [SerializeField, Header("�G�t�F�N�g")]
    private GameObject obj;
    [SerializeField, Header("���s����")]
    private float duration;
    [SerializeField, Header("�����Ԋu")]
    private float intervalMin;
    [SerializeField]
    private float intervalMax;
    [SerializeField, Header("�����ʒu�̂΂����(���S����+-)")]
    private Vector3 dispersion;

    private GameObject senrin;
    private float time = 0;
    private float coolTime = 0;
    private float interval = 0;
    private Vector3 clonePos;

    // Start is called before the first frame update
    void Start()
    {
        senrin = obj;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        coolTime += Time.deltaTime;

        if (time >= duration) {
            gameObject.SetActive(false);
            return;
        }

        Debug.Log(Time.deltaTime);

        if (coolTime >= interval) {
            coolTime = 0;
            for (int i = 0; i < Time.deltaTime / interval; i++) {
                clonePos = transform.position;
                clonePos.x += Random.Range(-dispersion.x, dispersion.x);
                clonePos.y += Random.Range(-dispersion.y, dispersion.y);
                clonePos.z += Random.Range(-dispersion.z, dispersion.z);

                Instantiate(senrin, clonePos, transform.rotation);
                interval = Random.Range(intervalMin, intervalMax);
            }
        }
    }
}
