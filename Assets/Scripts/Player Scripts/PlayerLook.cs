using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    private float xRotation = 0f;
    public Transform followTarget;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;


    private bool isZoom;
    private bool leftTilt;
    private bool rightTilt;

    Vector2 inputValue;

    Cinemachine3rdPersonFollow _3rdParam_base;

    [SerializeField] private float leftTilt_X;
    [SerializeField] private float rightTilt_X;

    private float zoomDistance;
    private float delta_tilt_X;

    public GameObject sphere;

    Vector3 recoilRotation;
    Recoil cam_Recoil;


    public bool IsZoom
    {
        get => isZoom;
        set
        {
            if (isZoom != value)
            {
                isZoom = value;

                zoomDistance = isZoom == true ? 1 : 2;

            }
        }
    }

    public bool LeftTilt
    {
        get => leftTilt;
        set
        {
            if (leftTilt != value)
            {
                leftTilt = value;

                delta_tilt_X = leftTilt == true ? delta_tilt_X - leftTilt_X : delta_tilt_X + leftTilt_X;
            }
        }
    }

    public bool RightTilt
    {
        get => rightTilt;
        set
        {
            if (rightTilt != value)
            {
                rightTilt = value;

                delta_tilt_X = rightTilt == true ? delta_tilt_X - rightTilt_X : delta_tilt_X + rightTilt_X;
            }
        }
    }

    private void Awake()
    {
        _3rdParam_base = cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cam_Recoil = cam.transform.GetComponent<Recoil>();
        delta_tilt_X = _3rdParam_base.ShoulderOffset.x;
        zoomDistance = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLook(inputValue);

        _3rdParam_base.CameraDistance = Mathf.Lerp(_3rdParam_base.CameraDistance, zoomDistance, 0.1f);
        _3rdParam_base.ShoulderOffset.x = Mathf.Lerp(_3rdParam_base.ShoulderOffset.x, delta_tilt_X, 0.1f);


    }

    private void FixedUpdate() {
        //화면 정중앙에 레이 발사
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            sphere.transform.position = hit.point;   
        }
    }

    public void ProcessLook(Vector2 input)
    {
        
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);


        //반동이 있다면 recoilrotation에 적용
        recoilRotation = Vector3.Slerp(recoilRotation, cam_Recoil.targetRotation, cam_Recoil.snappiness * Time.deltaTime);
        // currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        // transform.localRotation = Quaternion.Euler(currentRotation);

        Quaternion newRotation = Quaternion.Euler(new Vector3(xRotation, 0, 0) + recoilRotation); // y축은 캐릭터의 회전
        followTarget.transform.localRotation = newRotation; // 카메라 로컬 회전 적용

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity); // 캐릭터의 y축 회전
        followTarget.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    void OnLookAt(InputValue value)
    {
        inputValue = value.Get<Vector2>();
    }


}
