using UnityEngine;
using UnityEngine.UI;  // 追加

public class SelectLoop : MonoBehaviour
{
    // 最初のフレーム更新の前に開始が呼び出されます
    void Start()
    {
        // ボタンなど選択可能なコンポーネントを取得する
        var selects = GetComponentsInChildren<Selectable>();
        for (var i = 0; i < selects.Length; i++)
        {
            var nav = selects[i].navigation;
            nav.mode = Navigation.Mode.Explicit;
            nav.selectOnUp = selects[i == 0 ? selects.Length - 1 : i - 1];
            nav.selectOnDown = selects[(i + 1) % selects.Length];
            selects[i].navigation = nav;
        }
    }

    // 更新はフレームごとに1回呼び出されます
    void Update() { }
}