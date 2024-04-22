using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTime : MonoBehaviour
{
    ParticleSystem particleSystem;
    private GameObject CameraObject;
    SceneChange sceneChange;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        CameraObject = GameObject.Find("Main Camera");
        sceneChange = CameraObject.GetComponent<SceneChange>();
        sceneChange.SetParticleTime(particleSystem.time, particleSystem.main.duration);
    }

    // Update is called once per frame
    void Update()
    {
        sceneChange.SetParticleTime(particleSystem.time, particleSystem.main.duration);
        if(particleSystem.time == particleSystem.main.duration)
        {
            //Destroy(gameObject);
        }
    }
}
