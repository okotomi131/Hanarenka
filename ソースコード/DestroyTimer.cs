using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField, Header("íœ‚Ü‚Å‚ÌŽžŠÔ(•b)")]
    private float destroyTime = 1.0f;

    [SerializeField, Header("Œë·(•b)")]
    private float timeRange = 0.0f;

    float currentTime;

    float range;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0.0f;
        range = Random.Range(-timeRange, timeRange);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= destroyTime + range) {
            Destroy(gameObject);
        }
    }
}
