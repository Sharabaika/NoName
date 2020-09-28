using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class WeaponSubPosition : MonoBehaviour
    {
        [SerializeField] private bool rotatesWithCamera = true;

        private Vector3 _sway;
        [SerializeField] private float swayAmplitude = 1f;
        [SerializeField] private float swaySpeed = .1f;

        private Vector3 _recoilTarget;
        private Vector3 _recoil;
        [SerializeField] private float recoilMaxAmplitude = 10f;
        [SerializeField] private float recoilControllability = 0.1f;
        [SerializeField] private float recoilIntensity = 0.05f;

        private bool _isActive;


        public Transform CameraTransform { get; set; }

        public bool isActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (value)
                {
                    _sway = Vector3.zero;
                    _recoilTarget = Vector3.zero;

                    StartCoroutine(Sway());
                    // center on camera
                }
            }
        }

        public void AddRecoil(Vector3 recoil)
        {
            _recoilTarget += recoil;
            _recoilTarget = Vector2.ClampMagnitude(_recoilTarget, 5f);
        }


        private void Update()
        {
            if (isActive)
            {
                if (rotatesWithCamera)
                {
                    Vector3 angle = -transform.localEulerAngles;

                    // Sway
                    angle += _sway;

                    // Recoil
                    angle += _recoil;
                    _recoilTarget *= 1 - recoilControllability;
                    _recoil = Vector3.Lerp(_recoil, _recoilTarget, recoilIntensity);

                    // Rotation
                    var cameraPosition = CameraTransform.position;

                    transform.RotateAround(cameraPosition, CameraTransform.right, angle.x);
                    transform.RotateAround(cameraPosition, transform.up, angle.y);
                }
            }
        }

        private IEnumerator Sway()
        {
            while (isActive)
            {
                Vector3 swayTarget = Random.insideUnitCircle * swayAmplitude;
                while (isActive && _sway != swayTarget)
                {
                    _sway = Vector2.MoveTowards(_sway, swayTarget, swaySpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}