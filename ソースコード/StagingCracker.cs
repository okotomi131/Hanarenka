using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingCracker : MonoBehaviour
{
    [SerializeField, Header("パーティクルオブジェクト")]
    public GameObject _particleObject; //- パーティクルオブジェクト 
    [SerializeField, Header("消滅時間")]
    public float _destroyTime; //- パーティクルオブジェクト 

    void Start()
    {
        //- 着火音再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Ignition);       
        //- クラッカーのエフェクト生成
        GameObject fire = Instantiate(
            _particleObject,                                            // 生成(コピー)する対象
            transform.position,                                         // 生成される位置
            Quaternion.Euler(0.0f, 0.0f, transform.localEulerAngles.z)  // 最初にどれだけ回転するか
            );
        //- 自身を破壊する
        Destroy(this.gameObject, _destroyTime);
        //- 破裂音の再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Brust);
        //- 振動の設定
        //VibrationManager vibration = GameObject.Find("VibrationManager").GetComponent<VibrationManager>();
        //vibration.SetVibration(30, 1.0f);
    }
}
