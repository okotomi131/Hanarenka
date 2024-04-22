using Unity.Mathematics;
using UnityEngine;

public class RandomFireColor : MonoBehaviour
{
    // 変化させる色
    [ColorUsage(false, true), SerializeField]
    private Color[] particleColor;
    [ColorUsage(true, true), SerializeField]
    private Color[] trailColor;

    // 色を変えるパーティクルシステム
    [SerializeField]
    private ParticleSystem[] particles;
    [SerializeField]
    private ParticleSystem[] trails;

    // それぞれのシェーダー内に宣言してある色のプロパティ名
    [SerializeField]
    private string particleShaderProperty;
    [SerializeField]
    private string trailShaderProperty;

    private int colorNum; // 色のランダム指定用
    MaterialPropertyBlock materialPropertyBlock; // 個別に色変更するために使用

    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        colorNum = UnityEngine.Random.Range(0, particleColor.Length);

        for (int i = 0; i < particles.Length; i++) {
            int colorProperty = Shader.PropertyToID(particleShaderProperty);
            materialPropertyBlock.SetColor(colorProperty, particleColor[colorNum]);
            particles[i].GetComponent<ParticleSystemRenderer>().SetPropertyBlock(materialPropertyBlock);
        }

        for (int i = 0; i < trails.Length; i++) {
            int colorProperty = Shader.PropertyToID(trailShaderProperty);
            materialPropertyBlock.SetColor(colorProperty, trailColor[colorNum]);
            trails[i].GetComponent<ParticleSystemRenderer>().SetPropertyBlock(materialPropertyBlock);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
