using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // 커서를 숨김
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
