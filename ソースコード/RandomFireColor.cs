using Unity.Mathematics;
using UnityEngine;

public class RandomFireColor : MonoBehaviour
{
    // �ω�������F
    [ColorUsage(false, true), SerializeField]
    private Color[] particleColor;
    [ColorUsage(true, true), SerializeField]
    private Color[] trailColor;

    // �F��ς���p�[�e�B�N���V�X�e��
    [SerializeField]
    private ParticleSystem[] particles;
    [SerializeField]
    private ParticleSystem[] trails;

    // ���ꂼ��̃V�F�[�_�[���ɐ錾���Ă���F�̃v���p�e�B��
    [SerializeField]
    private string particleShaderProperty;
    [SerializeField]
    private string trailShaderProperty;

    private int colorNum; // �F�̃����_���w��p
    MaterialPropertyBlock materialPropertyBlock; // �ʂɐF�ύX���邽�߂Ɏg�p

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
