using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PController : MonoBehaviour
{
    [Header("�ړ��̑���"), SerializeField]
    private float speed = 3;

    [Header("�d�͉����x"), SerializeField]
    private float gravity = 15;

    [Header("�������̑��������iInfinity�Ŗ������j"), SerializeField]
    private float fallSpeed = 10;

    [Header("�����̏���"), SerializeField]
    private float initFallSpeed = 2;

    [SerializeField, Header("�Ήԗp�̃I�u�W�F�N�g")]
    private GameObject particleObject;

    [SerializeField, Header("���ł���I�u�W�F�N�g")]
    private GameObject DeleteObject;

    //- �W�����v������(�W�����v�񐔂��񕜂����,���̕ϐ���0�ɖ߂�)
    private int nJumpCount = 0;

    AudioSource audioSource;

    private Transform _transform;
    private CharacterController characterController;
    private bool bIsPlaySound;
    private bool bIsWait = false; //- �ҋ@��Ԃ��ǂ����@

    private Vector2 inputMove;
    private float verticalVelocity;
    private float turnVelocity;
    private bool isGroundedPrev;
    bool isOnce; // ��������񂾂��s��
    private GameObject CameraObject;

    FireworksModule fireworks;
    SceneChange sceneChange;

    void Start()
    {
        _transform = transform;
        CameraObject = GameObject.Find("Main Camera");
        sceneChange = CameraObject.GetComponent<SceneChange>();
        //audioSource = GetComponent<AudioSource>();

        fireworks = GetComponent<FireworksModule>();

        isOnce = false;
    }
    
    /// <summary>
    /// �ړ�Action(PlayerInput������Ă΂��)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if(bIsWait) return;//- �ҋ@��ԂȂ烊�^�[��

        // ���͒l��ێ����Ă���
        inputMove = context.ReadValue<Vector2>();
        //- ���̍Đ�
        if ((inputMove.x != 0 || inputMove.y != 0) && !bIsPlaySound)
        {
            bIsPlaySound = true;
            //audioSource.Play();
        }
        else if (bIsPlaySound && (inputMove.x == 0 && inputMove.y == 0))
        {
            bIsPlaySound = false;
            //audioSource.Stop();
        }
    }

    public void OnDestruct(InputAction.CallbackContext context)
    {
        if (bIsWait) return;//- �ҋ@��ԂȂ烊�^�[��

        //����
        if (!isOnce)
        {
            //- ��������
            fireworks.Ignition(transform.position);

            //- SceneChange�X�N���v�g�̃v���C���[�����t���O��false�ɂ���
            sceneChange.bIsLife = false;
            isOnce = true;
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 Pos = _transform.position;

        var isGrounded = characterController.isGrounded;

        if (isGrounded && !isGroundedPrev)
        {
            // ���n����u�Ԃɗ����̏������w�肵�Ă���
            verticalVelocity = -initFallSpeed;
            //- ���n�����Ƃ��ɁA�W�����v���񕜂���(�W�����v�񐔂�0�ɂ���)
            nJumpCount = 0;
        }
        else if (!isGrounded)
        {
            // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
            verticalVelocity -= gravity * Time.deltaTime;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
            if (verticalVelocity < -fallSpeed)
                verticalVelocity = -fallSpeed;

            //// �󒆂ŕ��ɂԂ�����Y�����~�܂�(��������)
            //if (oldposY == this.transform.position.y)
            //{
            //    verticalVelocity = -1;
            //}
            //oldposY = this.transform.position.y;
        }

        isGroundedPrev = isGrounded;


        if(isOnce || fireworks.IsExploded)
        { inputMove = Vector2.zero; }

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        var moveVelocity = new Vector3(
            inputMove.x * speed,
            inputMove.y * speed
        );

        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        characterController.Move(moveDelta);

        if (inputMove != Vector2.zero)
        {
            // �X�e�B�b�N���E�ɓ|���ꂽ�Ƃ�
            if (inputMove.x >= 0.1f)
            {
               //�C�[�W���O���Ȃ��玟�̉�]�p�x���v�Z
               var angleY = Mathf.SmoothDampAngle(
                   transform.eulerAngles.y,
                   110.0f,      //�ڕW�p�x�̐ݒ�
                   ref turnVelocity,
                   0.1f     //��]���x
               );

                //��]�̍X�V
                transform.rotation = Quaternion.Euler(0,angleY,0);
            }

            //�X�e�B�b�N�����ɓ|���ꂽ�Ƃ�
            if (inputMove.x <= -0.1f)
            {
                //�C�[�W���O���Ȃ��玟�̉�]�p�x���v�Z
                var angleY = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    -160.0f,    //�ڕW�p�x�̐ݒ�
                    ref turnVelocity,
                    0.1f    //��]���x
                );

                //��]�̍X�V
                transform.rotation = Quaternion.Euler(0,angleY,0);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Stage")
        {
            float HitBlockMoveX = other.gameObject.transform.position.x - _transform.position.x;
            float HitBlockMoveY = other.gameObject.transform.position.y - _transform.position.y;
            var HitMove = new Vector3(HitBlockMoveX * 0.001f, HitBlockMoveY * 0.001f);
            characterController.Move(HitMove);
        }
    }

    //- �v���C���[���ҋ@��Ԃ��ǂ����̃t���O��؂�ւ���֐�
    public void SetWaitFlag(bool Flag)
    {
        //- �ҋ@��ԂȂ�ړ��ʂ�0��
        if (Flag) inputMove = new Vector2(0, 0);
        //- �ҋ@�t���O�؂�ւ�
        bIsWait = Flag;
    }

    /// <summary>
    /// �v���C���[���ҋ@��Ԃ��ԋp����
    /// </summary>
    /// <returns>bool:�ҋ@���</returns>
    public bool GetWaitFlag()
    { return bIsWait; }

    /// <summary>
    /// �����ς��擾����
    /// </summary>
    /// <returns> bool:���� </returns>
    public bool GetIsOnce() { return isOnce; }
}

