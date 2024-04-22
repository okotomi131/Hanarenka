using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimation : MonoBehaviour
{
    Animator animator;
    float animeTime = -1.0f * 3;
    bool bBreath = false;

    void Start()
    {
        //- �A�j���[�^�[�̎擾
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //- �ʂ��ԉ΂̃u���X�A�j���t���O��true�Ȃ烊�^�[��
        if (bBreath) return;

        //- �A�j���[�V�������x���O�{�Ői�s
        animeTime += Time.deltaTime * 3.0f;

        //- �J�n�͏����x�点��
        if (animeTime >= 0.0f)
        {
            float Rad = Mathf.Cos(animeTime);
            Rad /= 5.0f;
            animator.SetFloat(Animator.StringToHash("speed"), Rad);
        }
    }

    public void BreathAnime()
    {
        bBreath = true;
        animator.SetFloat(Animator.StringToHash("speed"), 1.0f);
    }
}
