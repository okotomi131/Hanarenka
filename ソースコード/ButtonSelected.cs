/*
 ===================
 基盤制作：中村
 追記：大川・牧野
 ===================
 */

using UnityEngine;
using UnityEngine.UI; // UIコンポーネントの使用

public class ButtonSelected : MonoBehaviour
{
    private Button UISelect;
    void Start()
    {
        UISelect = GetComponent<Button>();
        // ボタン選択音がならないようにする
        GetComponent<ButtonAnime>().bPermissionSelectSE = false;
        // 最初に選択状態にしたいボタンの設定
        UISelect.Select();
    }
}
