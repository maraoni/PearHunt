using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    Transform Target = null;

    [Header("Camera Settings")] [SerializeField]
    private Camera playerCamera;

    [Header("Player Camera Settings")] [SerializeField]
    private float lookSensitivity = 10f;

    [SerializeField, Range(-2, -10)] private float cameraOffset = -5f;
    [SerializeField] private float HorizontalClamp = 80f;
    [SerializeField] private float targetBodyRotateSpeed = 10f;

    // Helps to determine the look direction
    private Vector2 _lookDirection = Vector3.zero;
    private Vector2 _playerTargetRotation = Vector3.zero;

    public void InitializeCamera(Transform aTarget)
    {
        Target = aTarget;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Target == null) return;

        // Fetch mouse axis data
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        // Offset camera distance from body
        playerCamera.transform.localPosition = new Vector3(0f, 0f, cameraOffset);

        //camera logic here
        _lookDirection.x += mouseX;
        _lookDirection.y = Mathf.Clamp(_lookDirection.y - mouseY, -HorizontalClamp, HorizontalClamp);

        // Player Body Rotation
        _playerTargetRotation.x = Mathf.LerpAngle(_playerTargetRotation.x, _lookDirection.x,
            Time.deltaTime * targetBodyRotateSpeed);

        // Apply the rotation to the player body
        Target.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

        // Apply the rotation to the camera
        transform.rotation = Quaternion.Euler(_lookDirection.y, _lookDirection.x, 0f);
    }
}