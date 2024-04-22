using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    [SerializeField, Header("移動速度")]
    private float speed = 0.1f;

    [SerializeField, Header("ジャンプ力")]
    private float jampPower = 350f;

    [SerializeField, Header("移動キー")]
    private KeyCode right = KeyCode.D;
    [SerializeField]
    private KeyCode left = KeyCode.A;

    [SerializeField, Header("ジャンプキー")]
    private KeyCode jamp = KeyCode.Space;

    [SerializeField, Header("花火キー")]
    private KeyCode fireWork = KeyCode.R;

    Transform playerTrans;
    Rigidbody playerRB;

    ShotBullet shotBullet;

    bool isGround;

    // Start is called before the first frame update
    void Start() {
        playerTrans = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody>();
        shotBullet = GetComponent<ShotBullet>();

        isGround = true;
    }

    // Update is called once per frame
    void Update() {
        // ジャンプ
        if (Input.GetKeyDown(jamp)) {
            if (isGround) {
                playerRB.AddForce(0.0f, jampPower, 0.0f);
            }
        }
        // 花火打ち上げ
        Vector2 ShotDir = new (0.0f,0.0f);
        if (Input.GetKeyDown(fireWork))
        {
            shotBullet.Shot(ShotDir);
            Debug.Log("打ちました(仮で発射数値を設定してます)");
        }
    }

    void FixedUpdate() {
        // 左移動
        if (Input.GetKey(left)) {
            playerTrans.Translate(-speed, 0.0f, 0.0f);
        }
        // 右移動
        if (Input.GetKey(right)) {
            playerTrans.Translate(speed, 0.0f, 0.0f);
        }

        // リセット(デバッグ機能)
        if (Input.GetKey(KeyCode.F1)) {
            SceneManager.LoadScene("SampleScene");
        }

        isGround = false;
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Stage") {
            isGround = true;
        }
    }
}
