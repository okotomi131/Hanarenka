using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleWarp : MonoBehaviour
{
    ParticleSystem m_particleSystem;
    List<ParticleSystem.Particle> m_enterList = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> m_exitList = new List<ParticleSystem.Particle>();
    GameObject WarpA;
    GameObject WarpB;

    [SerializeField, Header("クリア時にワープ判定停止")]
    private bool ClearStop = true;

    [SerializeField, Header("柳花火")]
    private bool IsYanagi = false;

    void Start()
    {
        // ===== オブジェクト、コンポーネント取得 =====
        m_particleSystem = this.GetComponent<ParticleSystem>();
        WarpA = GameObject.Find("WarpholeA");
        WarpB = GameObject.Find("WarpholeB");

        // ===== トリガーモジュールの設定 =====
        var trigger = m_particleSystem.trigger;
        if (WarpA)
        {
            trigger.enabled = true;
            trigger.inside = ParticleSystemOverlapAction.Ignore;
            trigger.outside = ParticleSystemOverlapAction.Ignore;
            trigger.enter = ParticleSystemOverlapAction.Callback;
            trigger.exit = ParticleSystemOverlapAction.Callback;
            trigger.radiusScale = 0.1f;
            trigger.AddCollider(WarpA.transform);
            trigger.AddCollider(WarpB.transform);
        }
    }

    void OnParticleTrigger()
    {
        // ===== クリア後にワープ処理を行わない =====
        if (SceneChange.bIsChange == true && ClearStop) return;

        // ===== パーティクルを状態で検索 =====
        // 条件に一致するパーティクルを ParticleSystem から取得する.
        int numEnter = m_particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, m_enterList);
        int numExit = m_particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, m_exitList);
        //- 拡縮取得
        Vector3 scale = transform.localScale;

        // ===== Particle : Exit =====
        for (int idx = 0; idx < numExit; idx++)
        {
            // ===== パーティクル情報の取得 =====
            ParticleSystem.Particle p = m_exitList[idx];
            Vector3 pos = p.position; //- 座標取得
            pos = m_particleSystem.transform.TransformPoint(pos); //- ローカルからワールド
            Vector3 rot = transform.eulerAngles; //- 回転取得
            Vector2 dis = new Vector2(0, 0);

            // ===== ワープした事がある粒子かどうか =====
            if (p.startColor.a == 230) continue;
            // ===== トレイルを映すための作業 ===== 
            p.startColor = new Color(1, 1, 1, 0.9f);
            pos.z -= 99999;

            // ===== ワープホールの情報取得 =====
            Vector3 posA = WarpA.transform.position;
            Vector3 posB = WarpB.transform.position;
            float disA = Vector3.Distance(pos, posA);
            float disB = Vector3.Distance(pos, posB);

            // ===== ワープの方向で分岐 =====
            if (disA < disB)
            {
                // ===== ワープAの回転を無効化 =====
                //- ワープホール中心から接触地点までの距離を取得
                Vector3 HitdisA = pos - WarpA.transform.position;
                //- ワープAの回転取得、回転を打ち消す
                Vector3 rotA = WarpA.transform.localEulerAngles;
                //- 回転数値の調整
                if (rotA.x < 180) rotA.x = rotA.x - 360;
                //- 回転の適用
                HitdisA = Quaternion.Euler(rotA.x, 180 - rotA.y, 0) * HitdisA;

                // ===== 出現位置を反対側の面にする =====
                //- 全体的な距離を求める
                Vector2 effectDis = posA - transform.position;
                if (gameObject.name == "Yanagi") effectDis = posA - pos;
                //- 回転量を求めて打ち消し、出現面を反転させる
                float rad = Mathf.Atan(effectDis.y / effectDis.x);
                HitdisA = Quaternion.Euler(0, 0, -rad * Mathf.Rad2Deg + 180) * HitdisA;
                //- 出現面を反転
                HitdisA.y *= -1;
                //- 回転量を復活させる
                HitdisA = Quaternion.Euler(0, 0, rad * Mathf.Rad2Deg) * HitdisA;

                // ===== ワープBの移動先へ =====
                //- ワープBの回転取得、回転を与える
                Vector3 rotB = WarpB.transform.eulerAngles;
                //- 回転数値の調整
                if (rotB.x < 180) rotB.x = rotB.x - 360;
                //- 回転の適用
                HitdisA = Quaternion.Euler(-rotB.x, 180 + rotB.y, 0) * HitdisA;

                // ===== 粒子を移動させる =====
                pos = posB + HitdisA;
            }
            else if (disA >= disB)
            {
                // ===== ワープAの回転を無効化 =====
                //- ワープホール中心から接触地点までの距離を取得
                Vector3 HitdisB = pos - WarpB.transform.position;
                //- ワープBの回転取得、回転を打ち消す
                Vector3 rotB = WarpB.transform.localEulerAngles;
                //- 回転数値の調整
                if (rotB.x < 180) rotB.x = rotB.x - 360;
                //- 回転の適用
                HitdisB = Quaternion.Euler(rotB.x, 180 - rotB.y, 0) * HitdisB;
                // ===== 出現位置を反対側の面にする =====
                //- 全体的な距離を求める
                Vector2 effectDis = posB - transform.position;
                if (gameObject.name == "Yanagi") effectDis = posB - pos;
                //- 回転量を求めて打ち消し、出現面を反転させる
                float rad = Mathf.Atan(effectDis.y / effectDis.x);
                HitdisB = Quaternion.Euler(0, 0, -rad * Mathf.Rad2Deg + 180) * HitdisB;
                //- 出現面を反転
                HitdisB.y *= -1;
                //- 回転量を復活させる
                HitdisB = Quaternion.Euler(0, 0, rad * Mathf.Rad2Deg) * HitdisB;

                // ===== ワープBの移動先へ =====
                //- ワープAの回転取得、回転を与える
                Vector3 rotA = WarpA.transform.eulerAngles;
                //- 回転数値の調整
                if (rotA.x < 180) rotA.x = rotA.x - 360;
                //- 回転の適用
                HitdisB = Quaternion.Euler(-rotA.x, 180 + rotA.y, 0) * HitdisB;

                // ===== 粒子を移動させる =====
                pos = posA + HitdisB;
            }

            // ===== 柳花火専用 =====
            if (IsYanagi == true) pos.z = 0;
            // ===== 計算後の適用処理 =====
            pos = m_particleSystem.transform.InverseTransformPoint(pos); //- ワールドからローカル
            p.position = pos;         //- 座標適用
            m_exitList[idx] = p; //- パーティクル適用
        }

        // ===== Particle : Enter =====
        for (int idx = 0; idx < numEnter; idx++)
        {
            // ===== パーティクル情報の取得 =====
            ParticleSystem.Particle p = m_enterList[idx];
            Vector3 pos = p.position; //- 座標取得
            pos = m_particleSystem.transform.TransformPoint(pos); //- ローカルからワールド
            // ===== ワープした事がある粒子かどうか =====
            if (p.startColor.a == 230) continue;
            // ===== トレイルを消すための作業 ===== 
            p.startColor = new Color(0, 0, 0, 0);

            pos.z += 99999;
            // ===== 計算後の適用処理 =====
            pos = m_particleSystem.transform.InverseTransformPoint(pos); //- ワールドからローカル
            p.position = pos;         //- 座標適用
            m_enterList[idx] = p; //- パーティクル適用
        }

        // ===== 設定変更後のパーティクルを適用 ======
        m_particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, m_enterList);
        m_particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, m_exitList);
    }
}