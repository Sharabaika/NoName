using System;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivity = 1f;

    public Transform CameraTransform => cameraTransform;

    private Transform _transform;
    private float _xRot = 0f;

    private void Start()
    {
        _transform = transform;
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, -90f, 90f);
        
        cameraTransform.localRotation = Quaternion.Euler(_xRot,0f,0f);
        
        // if camera is not on the vertical axis
        transform.localRotation *= Quaternion.AngleAxis(mouseX, Vector3.up);
    }
}