/*
 ===================
 制作：大川
 UIアニメーションの作動タイミングを管理するスクリプト
 ===================
 */

using UnityEngine;

//- クリア状態を読み
public class ReadClearFlag : MonoBehaviour
{
    private enum E_ACITEVESETTING
    {
        [InspectorName("クリアでアクティブ")]    ToActive,
        [InspectorName("クリアで非アクティブ")]  ToNoActive,
    }

    [SerializeField, Header("ステージ数")]  　    private int stagenum = -1;
    [SerializeField, Header("自分を入れる")]      private GameObject obj;
    [SerializeField, Header("アクティブ設定")]    private E_ACITEVESETTING Active = E_ACITEVESETTING.ToActive;

    private SaveManager save;  
    private bool read = false;  //読み込みフラグ
    private bool first = false; //初回処理フラグ

    void Awake()
    {   
        //- アクティブ設定ごとに処理を変更する
        switch (Active)
        {
            case E_ACITEVESETTING.ToActive:     //クリア時にアクティブ
                //- 初めて読み込むか
                if (!first)
                {
                    save = FindObjectOfType<SaveManager>();
                    //- クリアしていないか
                    if (!save.GetStageClear(stagenum))
                    {
                        //- クリアしていなければ非アクティブにする
                        obj.SetActive(false);
                    }
                    else if (save.GetStageClear(stagenum))
                    {
                        //- クリア済であればアクティブにする
                        obj.SetActive(true);
                        //- アクティブ化したのでこれ以上読み込み処理を行わないようにする
                        read = true;
                    }
                    //- 初回読み込みを終了
                    first = true;
                }

                //- クリアしている、まだ読み込み処理をしていない
                if (!read && save.GetStageClear(stagenum))
                {
                    //- ボタンをアクティブにする
                    obj.SetActive(true);
                    //- 読み込みフラグ変更
                    read = true;
                }
                break;
            case E_ACITEVESETTING.ToNoActive:   //クリア時に非アクティブ
                //- 初めて読み込むか
                if (!first)
                {
                    save = FindObjectOfType<SaveManager>();
                    //- クリアしていないか
                    if (!save.GetStageClear(stagenum))
                    {
                        //- クリアしていなければアクティブにする
                        obj.SetActive(true);
                    }
                    else if (save.GetStageClear(stagenum))
                    {
                        //- クリア済であれば非アクティブにする
                        obj.SetActive(false);
                        //- 非アクティブ化したのでこれ以上読み込み処理を行わないようにする
                        read = true;
                    }
                    //- 初回読み込みを終了
                    first = true;
                }

                //- クリアしている、まだ読み込み処理をしていない
                if (!read && save.GetStageClear(stagenum))
                {
                    //- ボタンを非アクティブにする
                    obj.SetActive(false);
                    //- 読み込みフラグ変更
                    read = true;
                }
                break;
        }
    }
}
