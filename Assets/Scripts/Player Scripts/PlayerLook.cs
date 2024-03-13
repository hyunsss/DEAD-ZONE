using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    Vector2 inputValue;
    
    private void Awake() {
        cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessLook(inputValue);
    }

    public void ProcessLook(Vector2 input) {
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cinemachine.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    void OnLookAt(InputValue value) {
        inputValue = value.Get<Vector2>();
    }
}
