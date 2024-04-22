using UnityEngine;

public class PickUpBullet : MonoBehaviour
{
    [SerializeField, Header("�����擾")]
    private bool isAuto = true;

    ShotBullet shotBullet;

    // Start is called before the first frame update
    void Start()
    {
        shotBullet = GetComponent<ShotBullet>();
    }

    private void Update() {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // �����擾�@�\���I���A�܂��͎擾�L�[��������Ă���ꍇ
        //if (isAuto)
        //{
            if (other.gameObject.tag == "Fireworks")
            {
                Destroy(other.gameObject);
                shotBullet.AddBullet(1);
            }
        //}

        if (!isAuto && Input.GetKey(KeyCode.F))
        {
            if (other.gameObject.tag == "Fireworks")
            {
                Destroy(other.gameObject);
                shotBullet.AddBullet(1);
            }
        }
    }
}
