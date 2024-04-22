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
        //- アニメーターの取得
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //- ぬし花火のブレスアニメフラグがtrueならリターン
        if (bBreath) return;

        //- アニメーション速度を三倍で進行
        animeTime += Time.deltaTime * 3.0f;

        //- 開始は少し遅らせる
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
