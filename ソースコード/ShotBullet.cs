using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour {

    [SerializeField, Header("�e�p�̃I�u�W�F�N�g")]
    private GameObject bulletObject;

    int bulletNum;

    // Start is called before the first frame update
    void Start() {
        bulletNum = 0;
    }

    //- �e�𔭎˂���֐�
    //- �����Fbool �E�Ɍ����ǂ���
    //        Vector2 ���˕����x�N�g���Ƃ��̋���
    public void Shot(Vector2 ShotDirVector) {
        if (bulletNum <= 0) { return; }

        bulletNum -= 1;

        //- �E���ւ̔��˃x�N�g���𐶐�
        Vector3 pos = new(1.0f, 0.5f, 0.0f);
        //- ���˕����x�N�g���̊i�[�p
        Vector3 ForceDirection = new Vector3(ShotDirVector.x, ShotDirVector.y, 0.0f);
        //- �e���������ɑł����ꍇ�A���ˈʒu�����E���]����
        if (ShotDirVector.x < 0)
        { pos.x *= -1; }
        
        GameObject bullet = Instantiate(
                bulletObject,     // ����(�R�s�[)����Ώ�
                transform.position + pos,     // ���������ʒu
                Quaternion.Euler(0.0f, 0.0f, 0.0f)    // �ŏ��ɂǂꂾ����]���邩
        );
        bullet.GetComponent<FireworksModule>().enabled = true;
        bullet.GetComponent<Rigidbody>().AddForce(ForceDirection);
    }

    public void AddBullet(int num) {
        bulletNum += num;
    }
}
