
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StorySkip : MonoBehaviour
{
    [SerializeField] private Image Skip;
    [SerializeField] private float SetPushTime = 0.0f;
    [SerializeField, Header("フェード秒数")]
    private float FadeTime;
    private Image fade;             // フェード用
    private BGMManager bgmManager;  // BGM用     
    private float PushButtonTime = 0.0f;
    private bool PushFlag = false;
    private bool FirstLoad = false;

    private void Awake()
    {
        // フェード
        fade = GameObject.Find("FadeImage").GetComponent<Image>();
        // 音
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
    }
    private void Update()
    {
        //- 入力中
        if(PushFlag)
        {
            PushButtonTime += Time.deltaTime;
            Skip.fillAmount = PushButtonTime / SetPushTime;
        }
        //- 非入力
        else
        {
            PushButtonTime = 0.0f;
            Skip.fillAmount = 0.0f;
        }
        //- 指定時間以上入力されていたら強制的にシーン遷移を行う
        if(PushButtonTime > SetPushTime)
        {
            //- 一回読み込んだら以降処理しない
            if(!FirstLoad)
            {
                FirstLoad = true;
                DOVirtual.DelayedCall(FadeTime, () => bgmManager.DestroyBGMManager());
                fade.DOFade(1.0f, 1.5f).OnComplete(() => { SceneManager.LoadScene("1_Village"); });
            }
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        //- クリアしていない際にボタン入力を受け付ける
        if (context.started)
        { PushFlag = true;  }//入力中
        if (context.canceled)
        { PushFlag = false; }//入力中止
    }
}
