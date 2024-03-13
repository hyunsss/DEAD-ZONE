using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;

    Vector3 moveDir;
    Vector3 inputVec;
    Vector3 gravityDir;
    float gravityValue;
    public float jumpPower;

    public float walkSpeed;
    public float runSpeed;
    float currentSpeed;
    bool isRun;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        animator.SetLayerWeight(1, 0f);
        currentSpeed = walkSpeed;
        gravityValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CalcGravity();
        MoveState();
        RunState();
        PlayAnim();
    }

    void CalcGravity() {
        //중력 가속도 가산
        gravityDir.y += Physics.gravity.y * gravityValue * Time.deltaTime;
    }

    void PlayAnim() {
        animator.SetFloat("Speed", moveDir.magnitude);
        
        if(moveDir.y > 0 && isRun == true) SetPlayerWalkAnimation(0, 2);
        else {SetPlayerWalkAnimation(moveDir.x, moveDir.y);}

        if(characterController.isGrounded == true && animator.GetBool("isJump") == true) {
            animator.SetBool("isJump", false);
        }

    }

    void OnMove(InputValue value) {
        moveDir = value.Get<Vector2>();

        float x, y;

        if(moveDir.y < 0) {
            x = moveDir.x;
            y = moveDir.y + 0.5f;
        } 
        else 
        {
            x = moveDir.x;
            y = moveDir.y;
        }

        inputVec.x = x;
        inputVec.z = y;
    }


    void OnJump(InputValue value) {

        if(characterController.isGrounded == true) {
            gravityDir.y = jumpPower;
            animator.SetBool("isJump", true);
        } 
    }

    void OnRun(InputValue value) {
        isRun = value.isPressed;
    }

    void RunState() {
        if(isRun == true && inputVec.z > 0) { 
            currentSpeed = runSpeed;
            
        }
        else {currentSpeed = walkSpeed;}
    }

    void MoveState() {
        Vector3 nextVec = inputVec * currentSpeed * Time.deltaTime;
        characterController.Move(transform.TransformDirection(nextVec));
        inputVec.y = gravityDir.y;
    }

    void SetPlayerWalkAnimation(float x, float y) {
        float TimeSpeed = 6f;
        float newX = Mathf.Lerp(animator.GetFloat("WalkX"), x, Time.deltaTime * TimeSpeed);
        float newY = Mathf.Lerp(animator.GetFloat("WalkY"), y, Time.deltaTime * TimeSpeed);

        animator.SetFloat("WalkX", newX);
        animator.SetFloat("WalkY", newY);
    }

}
