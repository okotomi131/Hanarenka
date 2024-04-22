using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    [SerializeField, Header("�ړ����x")]
    private float speed = 0.1f;

    [SerializeField, Header("�W�����v��")]
    private float jampPower = 350f;

    [SerializeField, Header("�ړ��L�[")]
    private KeyCode right = KeyCode.D;
    [SerializeField]
    private KeyCode left = KeyCode.A;

    [SerializeField, Header("�W�����v�L�[")]
    private KeyCode jamp = KeyCode.Space;

    [SerializeField, Header("�ԉ΃L�[")]
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
        // �W�����v
        if (Input.GetKeyDown(jamp)) {
            if (isGround) {
                playerRB.AddForce(0.0f, jampPower, 0.0f);
            }
        }
        // �ԉΑł��グ
        Vector2 ShotDir = new (0.0f,0.0f);
        if (Input.GetKeyDown(fireWork))
        {
            shotBullet.Shot(ShotDir);
            Debug.Log("�ł��܂���(���Ŕ��ː��l��ݒ肵�Ă܂�)");
        }
    }

    void FixedUpdate() {
        // ���ړ�
        if (Input.GetKey(left)) {
            playerTrans.Translate(-speed, 0.0f, 0.0f);
        }
        // �E�ړ�
        if (Input.GetKey(right)) {
            playerTrans.Translate(speed, 0.0f, 0.0f);
        }

        // ���Z�b�g(�f�o�b�O�@�\)
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
