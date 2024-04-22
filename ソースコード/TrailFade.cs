using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrailFade : MonoBehaviour
{
    [SerializeField, Header("’x‰„")]
    float delay = 0;
    [SerializeField, Header("‚©‚©‚éŽžŠÔ")]
    float duration = 0;

    float time = 0;
    float value = 1;

    private readonly int fadeProperty = Shader.PropertyToID("_FadeTime");

    ParticleSystemRenderer particle;
    MaterialPropertyBlock materialPropertyBlock;

    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        particle = GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();

        materialPropertyBlock.SetFloat(fadeProperty, value);
        particle.SetPropertyBlock(materialPropertyBlock);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time < delay) { return; }
        if (value <= 0) { return; }

        if (time <= duration + delay) {
            value = (0 - 1) / duration * (time - delay) + 1;
            //Debug.Log((time - delay) + ":" + value);
            materialPropertyBlock.SetFloat(fadeProperty, value);
            particle.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
