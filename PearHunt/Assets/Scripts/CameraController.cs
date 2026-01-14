using System.Collections;
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

    [Header("Camera Settings")]
    [SerializeField] private LayerMask cameraBlockingLayer;

    [Header("Player Camera Settings")] [SerializeField]
    private float lookSensitivity = 50f;

    [SerializeField, Range(-2, -10)] private float cameraOffset = -5f;
    [SerializeField, Range(50, 90)] private float HorizontalClamp = 80f;
    [SerializeField] private float targetBodyRotateSpeed = 10f;
    [SerializeField] private float cameraResetSpeed = 2f;

    // Helps to determine the look direction
    private Vector2 _lookDirection = Vector3.zero;
    private Vector2 _playerTargetRotation = Vector3.zero;
    private Vector3 _targetCenter;
    private Coroutine _cameraPositionResetCoroutine;
    
    private Camera _camera;

    public void InitializeCamera(Transform aTarget)
    {
        Target = aTarget;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        _camera = Camera.main;
        if (_camera == null)
        {
            _camera = new GameObject().AddComponent<Camera>();
        }
        
        _camera.transform.SetParent(transform);
        
        _camera.transform.localPosition = new Vector3(0f, 0f, cameraOffset);
        _targetCenter = Target.gameObject.GetComponent<Collider>().bounds.center;
        //Debug.Log(_targetCenter);
        transform.position += new Vector3(0f, _targetCenter.y, 0f);
    }

    void Update()
    {
        if (Target == null) return;
        
        CheckIfCameraIsBlocked(); // todo: maybe needs to be checked every .25 seconds for optimization
        UpdateCameraOnLook();
    }

    private void CheckIfCameraIsBlocked()
    {
        // Raycast from us to camera
        Vector3 direction = _camera.transform.position - transform.position;
        Vector3 start = Target.position + new Vector3(0f, _targetCenter.y, 0f);

        if (Physics.Raycast(start, direction, out RaycastHit hit, -cameraOffset, cameraBlockingLayer))
        {
            // Stop it if it's active
            if (_cameraPositionResetCoroutine != null)
            {
                StopCoroutine(_cameraPositionResetCoroutine);
                _cameraPositionResetCoroutine = null;
            }
            
            if (hit.collider)
            {
                _camera.transform.localPosition = new Vector3(0f, 0f,
                    -Mathf.Max(Vector3.Distance(start, hit.point), 1f));
            }
        }
        else
        {
            // Check if we need to reset
            if (_camera.transform.localPosition.Equals(new Vector3(0f, 0f, cameraOffset)) is false)
            {
                _cameraPositionResetCoroutine ??= StartCoroutine(ResetCameraLocalPosition());
            }
        }

        // todo: remove later maybe
        Debug.DrawRay(start, direction, Color.red);
    }

    private void UpdateCameraOnLook()
    {
        // Fetch mouse axis data
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

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

    private IEnumerator ResetCameraLocalPosition()
    {
        Vector3 start = _camera.transform.localPosition;
        
        for (float t = 0; t < 1f; t += Time.deltaTime * cameraResetSpeed)
        {
            _camera.transform.localPosition = Vector3.Lerp(start, new Vector3(0f, 0f, cameraOffset), t);
            yield return null;
        }

        _cameraPositionResetCoroutine = null;
    }
}