using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    private float xRotation = 0f;
    
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    [SerializeField] private Vector3 zoomPos;
    [SerializeField] private Vector3 leftTiltPos;
    [SerializeField] private Vector3 rightTiltPos;

    private bool isZoom;
    private bool leftTilt;
    private bool rightTilt;

    Vector2 inputValue;

    private Vector3 delta_camPos;

    public bool IsZoom {
        get => isZoom;
        set {
            if (isZoom != value) {
                isZoom = value;
            
                delta_camPos = isZoom == true ? delta_camPos + zoomPos : delta_camPos - zoomPos;
            }
        }
    }

    public bool LeftTilt {
        get => leftTilt;
        set {
            if (leftTilt != value) {
                leftTilt = value;
                
                delta_camPos = leftTilt == true ? delta_camPos + leftTiltPos : delta_camPos - leftTiltPos;
            }
        }
    }

    public bool RightTilt {
        get => rightTilt;
        set {
            if (rightTilt != value) {
                rightTilt = value;
                
                delta_camPos = rightTilt == true ? delta_camPos + rightTiltPos : delta_camPos - rightTiltPos;
            }
        }
    }

    private void Awake() {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        delta_camPos = cam.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLook(inputValue);

        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, delta_camPos, 0.1f);
    }

    public void ProcessLook(Vector2 input) {
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        Quaternion currentRotation = cam.transform.localRotation;

        // 현재 회전에서 y축과 z축의 값을 추출합니다.
        Vector3 currentEuler = currentRotation.eulerAngles;
        float yRotation = currentEuler.y;
        float zRotation = currentEuler.z;

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    void OnLookAt(InputValue value) {
        inputValue = value.Get<Vector2>();
    }

    
}
