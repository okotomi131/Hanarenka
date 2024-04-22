using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ===================
 ����F����
 �T�v�F�I�u�W�F�N�g�̋������Ǘ�����X�N���v�g
 ===================
*/
public class MovementManager : MonoBehaviour
{
    //--- �񋓑̒�`(�^�C�v)
    public enum E_MovementType
    {
        ThreewayBehaviour,       // �O��������
        ThreepointBehaviour,     // �O�_�ԋ���
        ThreepointWaitBehaviour, // �O�_�ԑҋ@����
        CicrleBehaviour,         // �~����
        SmoothCircularBehaviour, // ���炩�ȉ~����
    }

    //--- �񋓑̒�`(��])
    public enum E_RotaDirection
    {
        Clockwise,        // �����v���
        CounterClockwise, // ���v���
    }

    //--- �񋓑̒�`(�ړ�)
    public enum E_MoveDirection
    {
        Horizontal, // ���ړ�
        Vertical,   // �c�ړ�
        Diagonal,   // �΂߈ړ�
    }


    //* ���ʊ֘A *//
    //- �C���X�y�N�^�[�ɕ\��
    [SerializeField, Header("�����̎��")]
    public E_MovementType _type = E_MovementType.ThreewayBehaviour;
    [SerializeField, Header("������~")]
    private bool StopMove = false;
    //- �C���X�y�N�^�[�����\��
    FireworksModule fireworks;
    public E_MovementType Type => _type;

    //* �O���������֘A *//
    //- �C���X�y�N�^�[�ɕ\��
    [SerializeField, HideInInspector]
    public E_MoveDirection _moveDirection = E_MoveDirection.Horizontal; // �ړ�����
    [SerializeField, HideInInspector]
    public float _moveDistance = 5.0f; // �ړ�����
    [SerializeField, HideInInspector]
    public float _travelTime   = 1.0f; // �ړ�����
    //- �C���X�y�N�^�[�����\��
    private Vector3 startPosition;   // �J�n�ʒu
    private Vector3 endPosition;     // �I���ʒu
    private float   timeElapsed;     // �o�ߎ���
    private bool    reverse = false; // �ړ��̕����]���p
    //- �O������̒l�擾�p
    public E_MoveDirection MoveDirection => _moveDirection;
    public float MoveDistance => _moveDistance;
    public float TravelTime => _travelTime;

    //* �O�_�ԋ��� *//
    //- �C���X�y�N�^�[�ɕ\��
    [SerializeField, HideInInspector]
    public Vector3 _startPoint;   // �n�_
    [SerializeField, HideInInspector]
    public Vector3 _halfwayPoint; // ���ԓ_
    [SerializeField, HideInInspector]
    public Vector3 _endPoint;     // �I�_
    [SerializeField, HideInInspector]
    public float _moveSpeed   = 1.0f;   // �ړ����x
    [SerializeField, HideInInspector]
    public float _endWaitTime = 1.0f;   // �I�_���B���̑ҋ@����
    [SerializeField, HideInInspector]
    public float _waitTime = 1.0f;      // �e�|�C���g���B���̑ҋ@����
    //- �C���X�y�N�^�[�����\��
    private Vector3[] points = new Vector3[3]; // �z��̌����i�[
    private int   currentPoint     = 0;     // ���݂̈ʒu
    private int   currentDirection = 1;     // ���݂̕���
    private float waitingTimer     = 0.0f;  // �ҋ@����
    private bool  isWaiting        = false; // �ҋ@���Ă��邩���Ă��Ȃ���
    //- �O������̒l�擾�p
    public Vector3 StartPoint => _startPoint;
    public Vector3 HalfwayPoint => _halfwayPoint;
    public Vector3 EndPoint => _endPoint;
    public float MoveSpeed => _moveSpeed;
    public float EndWaitTime => _endWaitTime;
    public float WaitTime => _waitTime;

    //* �~�����֘A *//
    //- �C���X�y�N�^�[�ɕ\��
    [SerializeField, HideInInspector]
    public E_RotaDirection _rotaDirection = E_RotaDirection.Clockwise; // ��]����
    [SerializeField, HideInInspector]
    public Vector3 _center = Vector3.zero;    // ���S�_
    [SerializeField, HideInInspector]
    public Vector3 _axis   = Vector3.forward; // ��]��
    [SerializeField, HideInInspector]
    public float _radius     = 1.0f; // ���a�̑傫��
    [SerializeField, HideInInspector]
    public float _startTime  = 1.0f; // �J�n���ɂ��炷����(�b)
    [SerializeField, HideInInspector]
    public float _periodTime = 2.0f; // ������̂ɂ����鎞��(�b)
    [SerializeField, HideInInspector]
    public bool _updateRotation = false; // �������X�V���邩�ǂ���
    //- �C���X�y�N�^�[�����\��
    private float currentTime;  // ���݂̎���
    private float currentAngle; // ���݂̉�]�p�x
    private float angle = 360f; // ������̊p�x
    //- �O������̒l�擾�p
    public E_RotaDirection RotaDirection => _rotaDirection;
    public Vector3 Center => _center;
    public Vector3 Axis => _axis;
    public float Radius => _radius;
    public float StartTime => _startTime;
    public float PeriodTime => _periodTime;
    public bool UpdateRotation => _updateRotation;


    void Start()
    {
        //* ���ʍ��� *//
        fireworks = this.gameObject.GetComponent<FireworksModule>();

        //* �O������������ *//
        startPosition = transform.position;
        endPosition   = startPosition + Vector3.right * MoveDistance;

        //* �O�_�ԋ������� *//
        points[0] = StartPoint;    // �z���0�ԖڂɎn�_����
        points[1] = HalfwayPoint;  // �z���1�Ԗڂɒ��ԓ_����
        points[2] = EndPoint;      // �z���2�ԖڂɏI�_����

        //* �~�������� *//
        currentTime += StartTime; // �J�n���Ɏ��Ԃ����炷
    }

    void Update()
    {
        //- �I�����鋓���̃^�C�v�ɉ����ď����𕪊�
        switch (Type)
        {
            case E_MovementType.ThreewayBehaviour:
                ThreewayMove();
                break;
            case E_MovementType.ThreepointBehaviour:
                ThreePointMove();
                break;
            case E_MovementType.ThreepointWaitBehaviour:
                ThreePointWaitMove();
                break;
            case E_MovementType.CicrleBehaviour:
                CicrleMove();
                break;
            case E_MovementType.SmoothCircularBehaviour:
                SmoothCircularMove();
                break;
        }
    }

    /// <summary>
    /// �O��������
    /// </summary>
    private void ThreewayMove()
    {
        //- null�`�F�b�N
        if (StopMove && fireworks && fireworks.IsExploded) return;

        //- �o�ߎ��Ԃ��v�Z����
        timeElapsed += Time.deltaTime;

        //- �ړ��̊������v�Z����i0����1�܂ł̒l�j
        float t = Mathf.Clamp01(timeElapsed / TravelTime);

        //- �ړ������ɍ��킹�Ĉʒu��ύX����
        if (!reverse)
        {
            //- �ړ������ɉ����Ĉʒu���Ԃ���
            switch (MoveDirection)
            {
                case E_MoveDirection.Horizontal: // ����
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);
                    break;
                case E_MoveDirection.Vertical:   // ����
                    transform.position = Vector3.Lerp(
                        startPosition, startPosition + Vector3.up * MoveDistance, t);
                    break;
                case E_MoveDirection.Diagonal:   // �Ίp��
                    transform.position = Vector3.Lerp(
                        startPosition, startPosition + new Vector3(MoveDistance, MoveDistance, 0), t);
                    break;
            }
        }
        else
        {
            //- �t�����Ɉړ�����ꍇ
            switch (MoveDirection)
            {
                case E_MoveDirection.Horizontal:  // ����
                    transform.position = Vector3.Lerp(endPosition, startPosition, t);
                    break;
                case E_MoveDirection.Vertical:    // ����
                    transform.position = Vector3.Lerp(
                        startPosition + Vector3.up * MoveDistance, startPosition, t);
                    break;
                case E_MoveDirection.Diagonal:    // �Ίp��
                    transform.position = Vector3.Lerp(
                        startPosition + new Vector3(MoveDistance, MoveDistance, 0), startPosition, t);
                    break;
            }
        }

        //- �ړ�������������o�ߎ��Ԃ����Z�b�g����
        if (t == 1.0f)
        {
            timeElapsed = 0.0f; // �o�ߎ��Ԃ̃��Z�b�g
            reverse = !reverse; // �ړ������𔽓]
        }
    }

    /// <summary>
    /// �O�_�ԋ���
    /// </summary>
    private void ThreePointMove()
    {
        //- null�`�F�b�N
        if (StopMove && fireworks && fireworks.IsExploded) return;

        //- �I�u�W�F�N�g���ҋ@�����ǂ���
        if (isWaiting)
        {
            //- �o�ߎ��ԂɊ�Â��đҋ@���Ԃ�����������
            waitingTimer -= Time.deltaTime;

            //- �ҋ@���Ԃ�0�ȉ��ɂȂ����ꍇ
            if (waitingTimer <= 0)
            {
                //- �ҋ@��Ԃ��I������
                isWaiting = false;
                //- ���݂̕����Ɋ�Â��Ď��̃|�C���g�Ɉړ�����
                currentPoint += currentDirection;
                //- ���݂̃|�C���g���|�C���g�͈̔͊O�ł��鎞
                if (currentPoint >= points.Length || currentPoint < 0)
                {
                    //- �ړ������𔽓]������
                    currentDirection *= -1;
                    //- ���]���������̎��̃|�C���g�Ɉړ�����
                    currentPoint += currentDirection;
                }
            }
        }
        else
        {
            //- ���̈ʒu�Ɉړ����邽�߂̕����x�N�g�����v�Z����
            Vector3 directionVector = (points[currentPoint] - transform.position).normalized;

            //- ���̈ʒu�Ɉړ����邽�߂̋������v�Z����
            float distanceToMove = MoveSpeed * Time.deltaTime;

            //- ���̈ʒu�Ɉړ�����
            transform.position += directionVector * distanceToMove;

            //- �������݈ʒu�Ǝ��̃|�C���g�Ƃ̋��������l�����ł���΁A���̃|�C���g�ɓ��B�����Ɣ��f����
            if (Vector3.Distance(transform.position, points[currentPoint]) < 0.01f)
            {
                //- �������݂̃|�C���g���I�_�ł����
                if (currentPoint == points.Length - 1)
                {
                    //- �ҋ@��Ԃ�L���ɂ���
                    isWaiting    = true;
                    //- �w�肵���ҋ@���ԕ��A�ҋ@����
                    waitingTimer = EndWaitTime;
                }
                else
                {
                    //- ���݂̕����Ɋ�Â��Ď��̃|�C���g�Ɉړ�����
                    currentPoint += currentDirection;
                    //- �������݂̃|�C���g���|�C���g�͈̔͊O�ł��鎞
                    if (currentPoint >= points.Length || currentPoint < 0)
                    {
                        //- �ړ������𔽓]������
                        currentDirection *= -1;
                        //- ���]���������̎��̃|�C���g�Ɉړ�����
                        currentPoint += currentDirection;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �O�_�ԋ���(�e�|�C���g�ҋ@)
    /// </summary>
    private void ThreePointWaitMove()
    {
        //- null�`�F�b�N
        if (StopMove && fireworks && fireworks.IsExploded) return;

        //- �I�u�W�F�N�g���ҋ@�����ǂ���
        if (isWaiting)
        {
            //- �o�ߎ��ԂɊ�Â��đҋ@���Ԃ�����������
            waitingTimer -= Time.deltaTime;

            //- �ҋ@���Ԃ�0�ȉ��ɂȂ����ꍇ
            if (waitingTimer <= 0.0f)
            {
                //- �ҋ@��Ԃ��I������
                isWaiting = false;
                //- ���݂̕����Ɋ�Â��Ď��̃|�C���g�Ɉړ�����
                currentPoint += currentDirection;
                //- ���݂̃|�C���g���|�C���g�͈̔͊O�ł��鎞
                if (currentPoint >= points.Length || currentPoint < 0)
                {
                    //- ���݂̕����𔽓]������
                    currentDirection *= -1;
                    //- ���]���������̎��̃|�C���g�Ɉړ�����
                    currentPoint += currentDirection;
                }
            }
        }
        else
        {
            //- ���̈ʒu�Ɉړ����邽�߂̕����x�N�g�����v�Z����
            Vector3 directionVector = (points[currentPoint] - transform.position).normalized;

            //- ���̈ʒu�Ɉړ����邽�߂̋������v�Z����
            float distanceToMove = MoveSpeed * Time.deltaTime;

            //- ���̈ʒu�Ɉړ�����
            transform.position += directionVector * distanceToMove;

            //- �������݈ʒu�Ǝ��̃|�C���g�Ƃ̋��������l�����ł���΁A���̃|�C���g�ɓ��B�����Ɣ��f����
            if (Vector3.Distance(transform.position, points[currentPoint]) < 0.01f)
            {
                //- �������݂̃|�C���g���n�_�܂��͒��ԓ_�܂��͏I�_�ł����
                if (currentPoint == points.Length - 3 || currentPoint == points.Length - 2 
                    || currentPoint == points.Length - 1)
                {
                    //- �ҋ@��Ԃ�L���ɂ���
                    isWaiting    = true;
                    //- �w�肵���ҋ@���ԕ��A�ҋ@����
                    waitingTimer = WaitTime;
                }
                else
                {
                    //- ���݂̕����Ɋ�Â��Ď��̃|�C���g�Ɉړ�����
                    currentPoint += currentDirection;
                    //- �������݂̃|�C���g���|�C���g�͈̔͊O�ł��鎞
                    if (currentPoint >= points.Length || currentPoint < 0)
                    {
                        //- �ړ������𔽓]������
                        currentDirection *= -1;
                        //- ���]���������̎��̃|�C���g�Ɉړ�����
                        currentPoint += currentDirection;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �~�^��
    /// </summary>
    private void CicrleMove()
    {
        //- null�`�F�b�N
        if (StopMove && fireworks && fireworks.IsExploded) return;

        //- Transform�I�u�W�F�N�g�̎Q�Ƃ��擾
        var trans = transform;

        //- ���݂̊p�x�Ǝ��Ɋ�Â��ăN�I�[�^�j�I���𐶐�
        var angleAxis = Quaternion.AngleAxis(currentAngle, Axis);

        //- ���a�ɑΉ�����x�N�g�����쐬���A��]���ɉ����ĉ�]������
        var radiusVec = angleAxis * (Vector3.up * Radius);

        //- ���S�_�ɔ��a�ɑΉ�����x�N�g�������Z���Ĉʒu���v�Z����
        var pos = Center + radiusVec;

        //- �ʒu���X�V����
        trans.position = pos;

        //- �������X�V����
        if (UpdateRotation)
        {  trans.rotation = Quaternion.LookRotation(Center - pos, Vector3.up);  }

        //- ���݂̉�]�p�x���X�V����
        currentTime += Time.deltaTime;

        //- ��]�����ɉ����ď����𕪊�
        switch (RotaDirection)
        {
            case E_RotaDirection.Clockwise:
                currentAngle = (currentTime % PeriodTime) / PeriodTime * angle;
                break;
            case E_RotaDirection.CounterClockwise:
                currentAngle = angle - ((currentTime % PeriodTime) / PeriodTime * angle);
                break;
        }
    }

    /// <summary>
    /// ���炩�ȉ~�^��
    /// </summary>
    private void SmoothCircularMove()
    {
        //- null�`�F�b�N
        if (StopMove && fireworks && fireworks.IsExploded) return;

        //- ��]�����ɉ����ď����𕪊�
        switch (RotaDirection)
        {
            case E_RotaDirection.Clockwise:
                currentAngle = (currentTime % PeriodTime) / PeriodTime * angle;
                break;
            case E_RotaDirection.CounterClockwise:
                currentAngle = angle - ((currentTime % PeriodTime) / PeriodTime * angle);
                break;
        }

        //- transform���A�ϐ�trans�Ɋi�[����
        var trans = transform;

        //- ��]�̃N�H�[�^�j�I���쐬
        var angleAxis = Quaternion.AngleAxis(currentAngle, Axis);

        //- ���a�ɑΉ�����x�N�g�����쐬���A��]���ɉ����ĉ�]������
        var radiusVec = angleAxis * (Vector3.down * Radius);

        //- ���S�_�ɔ��a�ɑΉ�����x�N�g�������Z���Ĉʒu���v�Z����
        var pos = Center + radiusVec;

        //- �ʒu���X�V����
        trans.position = pos;

        //- �������X�V����
        if (UpdateRotation)
        {  trans.rotation = Quaternion.LookRotation(Center - pos, Vector3.up);  }

        //- ���݂̉�]�p�x���X�V����
        currentTime += Time.deltaTime;
    }

    /// <summary>
    /// ��������~���Ă��邩��������֐�
    /// </summary>
    /// <param name="moveFlag"></param>
    public void SetStopFrag(bool moveFlag)
    {
        StopMove = moveFlag;
    }
}