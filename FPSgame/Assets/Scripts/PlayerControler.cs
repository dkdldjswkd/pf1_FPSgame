using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControler : MonoBehaviour
{
    //스피드 변수
    [SerializeField]
    private float WalkSpeed;
    [SerializeField]
    private float RunSpeed;
    private float ApplySpeed;
    [SerializeField]
    private float JumpForce;
    [SerializeField]
    private float CrouchSpeed;

    //상태 변수
    private bool IsRun = false;
    private bool IsGround = true;
    private bool IsCrouch = false;

    // 앉았을때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    // 민감도 변수
    [SerializeField]
    private float LookSensitivity;
    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit = 90;
    private float currentCameraRotationX = 0;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;


    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        ApplySpeed = WalkSpeed;

        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;

    }

    void Update()
    {
        GroundCheck();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharactorRotation();
    }

    // 앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기 동작
    private void Crouch()
    {
        IsCrouch = !IsCrouch;

        if (IsCrouch)
        {
            ApplySpeed = CrouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            ApplySpeed = WalkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    //부드러운 앉기 동작
    IEnumerator CrouchCoroutine() //cpu하나가 왔다갔다하며 병렬처리를 함, 이해 힘듬
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.1f); // 보간의 함수 a에서 b까지 c의 속도로 증가한다
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    //캐릭터 움직임
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * ApplySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    //달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    //달리기 동작
    private void Running()
    {
        // 앉아있다면 앉기 취소
        if (IsCrouch)
            Crouch();

        IsRun = true;
        ApplySpeed = RunSpeed;
    }
    private void RunningCancel()
    {
        IsRun = false;
        ApplySpeed = WalkSpeed;
    }

    //캐릭터 y축 회전 (자식 객체인 카메라는 y축회전)
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * LookSensitivity;
        currentCameraRotationX += _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);
    }

    //카메라 x축 회전
    private void CharactorRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * LookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    //지면 체크
    private void GroundCheck()
    {
        IsGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            Jump();
        }
    }

    //점프 실행
    private void Jump()
    {
        if (IsCrouch) // 만약 앉아있다면 일어나라
            Crouch();

        myRigid.velocity = transform.up * JumpForce;
    }


}
