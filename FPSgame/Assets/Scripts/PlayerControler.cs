using System.Collections;
using System.Collections.Generic;
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

    //상태 변수
    private bool IsRun = false;
    private bool IsGround = true;

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

    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        TryJump();
        TryRun();
        Move();
        CameraRotation();
        CharactorRotation();
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * ApplySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

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

    private void Running()
    {
        IsRun = true;
        ApplySpeed = RunSpeed;
    }
    private void RunningCancel()
    {
        IsRun = false;
        ApplySpeed = WalkSpeed;
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * LookSensitivity;
        currentCameraRotationX += _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3 (-currentCameraRotationX, 0, 0);
    }

    private void CharactorRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * LookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        Debug.Log(myRigid.rotation);
        Debug.Log(myRigid.rotation.eulerAngles);
    }

    private void GroundCheck()
    {
        IsGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        myRigid.velocity = transform.up * JumpForce;
    }


}
