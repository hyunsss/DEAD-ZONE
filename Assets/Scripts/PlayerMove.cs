using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    CharacterController characterController;
    Vector3 inputVec;
    Rigidbody rigid;

    float gravityValue;
    public float jumpPower;
    bool isCanjump;

    public float walkSpeed;
    public float runSpeed;
    float currentSpeed;
    bool isRun;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();

        currentSpeed = walkSpeed;
        gravityValue = -9.81f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveState();
        RunState();
    }

    void OnMove(InputValue value) {
        Vector2 TempVec = value.Get<Vector2>();

        float x, y;

        Debug.Log(inputVec.x + ", " + inputVec.z);
        if(TempVec.y < 0) {
            x = TempVec.x;
            y = TempVec.y + 0.5f;
        } 
        else 
        {
            x = TempVec.x;
            y = TempVec.y;
        }

        inputVec.x = x;
        inputVec.z = y;
    }

    void OnJump(InputValue value) {
        Debug.Log("Jump Pressed");

        if(characterController.isGrounded == true) {
            Debug.Log("Can Jump");
            inputVec.y = jumpPower;
        } else {
            Debug.Log("Can not Jump");
        }
    }

    void OnRun(InputValue value) {
        isRun = value.isPressed;
    }

    void RunState() {
        if(isRun == true && inputVec.z > 0) { 
            Debug.Log("달리는 상태");
            currentSpeed = runSpeed;
        }
        else {currentSpeed = walkSpeed; Debug.Log("걷는 상태");}
    }

    void MoveState() {
        Vector3 nextVec = inputVec * currentSpeed * Time.deltaTime;
        if(characterController.isGrounded == false) inputVec.y += gravityValue * Time.deltaTime;
        characterController.Move(transform.TransformDirection(nextVec));
    }

}
