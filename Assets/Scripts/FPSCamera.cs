using System;
using System.Collections;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Camera attachedCamera;
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float normalFOV = 68f;


    public Camera AttachedCamera => attachedCamera;
    public Transform CameraTransform => _cameraTransform;

    private float _aimingFov = 45f;
    private Transform _cameraTransform;
    private Transform _transform;
    private float _xRot = 0f;

    public void SwitchToAimingFOV(float aimingFOV = 45f ,float time = 0.2f)
    {
        if (_FOVlerpingCoroutine != null)
        {
            StopCoroutine(_FOVlerpingCoroutine);
        }

        _aimingFov = aimingFOV;
        _FOVlerpingCoroutine = StartCoroutine(LerpFOV(normalFOV, _aimingFov, time));
    }

    public void SwitchToNormalFOV(float time = 0.2f)
    {
        if (_FOVlerpingCoroutine != null)
        {
            StopCoroutine(_FOVlerpingCoroutine);
        }

        _FOVlerpingCoroutine = StartCoroutine(LerpFOV(_aimingFov, normalFOV, time));
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

    private float FOV
    {
        get => attachedCamera.fieldOfView;
        set => attachedCamera.fieldOfView = value;
    }


    private Coroutine _FOVlerpingCoroutine;

    private IEnumerator LerpFOV(float from, float to, float overTime)
    {
        var t = Mathf.InverseLerp(from, to, FOV) * overTime;
        while (t<=overTime)
        {
            t += Time.deltaTime;
            FOV = Mathf.Lerp(from, to, t / overTime);
            yield return null;
        }
    } 

    private void Awake()
    {
        _transform = transform;
        _cameraTransform = attachedCamera.transform;
        LockCursor();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, -90f, 90f);
        
        _cameraTransform.localRotation = Quaternion.Euler(_xRot,0f,0f);
        
        // if camera is not on the vertical axis
        transform.localRotation *= Quaternion.AngleAxis(mouseX, Vector3.up);
    }

    private void OnValidate()
    {
        AttachedCamera.fieldOfView = normalFOV;
    }
}