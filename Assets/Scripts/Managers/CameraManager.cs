using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private void Awake() {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // 커서를 숨김
        Cursor.visible = false;
    }

    public void CursorVisible(bool isVisible) {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void CursorVisible() {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible == true ? CursorLockMode.None : CursorLockMode.Locked;
    }   
}
