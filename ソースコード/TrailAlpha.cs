using Unity.Mathematics;
using UnityEngine;

public class TrailAlpha : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] particles;
    [SerializeField]
    private ParticleSystem[] trails;

    private int colorNum;

    // Start is called before the first frame update
    void Start()
    {
        int pos = Shader.PropertyToID("_ParticleSystemPos");

        for (int i = 0; i < particles.Length; i++) {
            particles[i].GetComponent<ParticleSystemRenderer>().material.SetVector(pos, transform.position );
        }

        for (int i = 0; i < trails.Length; i++) {
            trails[i].GetComponent<ParticleSystemRenderer>().material.SetVector(pos, transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
