using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ObjectFade : MonoBehaviour {
    public enum FadeState {
        None,
        Out,
        In
    }

    enum FadeType
    {
        Color,
        Grad
    }

    [SerializeField, Header("フェードの種類")]
    private FadeType fadeType;

    // フェードの状態
    FadeState fadeState;

    // アルファ値の変更用
    Image image;

    Material material;

    int progress = Shader.PropertyToID("_Progress");

    // フェードにかかる時間
    float duration;

    // フェード周りの情報
    Tweener tweener;

    // フェード中かどうか
    public bool isFade { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // フェード状態の初期化
        fadeState = FadeState.None;

        //----- コンポーネントの取得
        image = GetComponent<Image>();
        if (fadeType == FadeType.Grad) {
            material = GetComponent<Renderer>().material;
        }


        duration = 0;

        tweener = null;

        isFade = false;

        SetFade(FadeState.Out, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (fadeType) {
        case FadeType.Color:
            ColorFade();
            break;
        case FadeType.Grad:
            GradFade();
            break;
        }

        tweener.OnComplete(() => {
            isFade = false;
            //Debug.Log(isFade.ToString());
        });
    }

    public void SetFade(FadeState fadeState ,float duration)
    {
        this.fadeState = fadeState;
        this.duration = duration;
        isFade = true;
    }

    private void ColorFade()
    {
        switch (fadeState) {
        case FadeState.None:
            break;

        case FadeState.Out:
            tweener = TweenColorFade.FadeOut(image, duration);
            fadeState = FadeState.None;
            break;

        case FadeState.In:
            tweener = TweenColorFade.FadeIn(image, duration);
            fadeState = FadeState.None;
            break;

        default:
            break;
        }
    }

    private void GradFade()
    {
        switch (fadeState) {
        case FadeState.None:
            break;

        case FadeState.Out:

            tweener = TweenGradFade.FadeOut(material, duration);
            fadeState = FadeState.None;
            break;

        case FadeState.In:

            tweener = TweenGradFade.FadeIn(material, duration);
            fadeState = FadeState.None;
            break;

        default:
            break;
        }

        material.SetFloat(progress, TweenGradFade.currentTime);
    }

    public void SetColor(Color color, float alpha)
    {
        color.a = alpha;
        image.color = color;
    }
}
