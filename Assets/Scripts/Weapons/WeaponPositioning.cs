using System;
using System.Collections;
using UnityEngine;

namespace Weapons
{
    public enum WeaponPositioningRestrictions
    {
        None,
        ForceLower,
        CantAim
    }
    public class WeaponPositioning : MonoBehaviour
    {
        // TODO rework with supPositions and add recoil

        [SerializeField] private WeaponSubPosition shoulderPos;
        [SerializeField] private WeaponSubPosition aimingPos;
        [SerializeField] private WeaponSubPosition hiddenPos;

        [SerializeField] private float aimDownSideTime=0.2f;

        [SerializeField] private float shakingDegrees;
        [SerializeField] private float shakingSpeed;

        [SerializeField]private Transform weaponTransform;
        public float ADSTime => aimDownSideTime;
        public WeaponSubPosition CurrentSubPos => _currentSubPos;

        public bool CanShoot
        {
            get;
            private set;
        } = false;

        private Transform _cameraTransform;
        public Transform CameraTransform
        {
            get => _cameraTransform;
            set
            {
                _cameraTransform = value;
                shoulderPos.CameraTransform = value;
                aimingPos.CameraTransform = value;
                hiddenPos.CameraTransform = value;
            }
        }

        private bool _isActive;
        public bool isActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                _currentSubPos.isActive = value;
            }
        }
        
        private WeaponSubPosition _currentSubPos;
        private bool _isHidden = true;
        
        public void Aim()
        {
            CanShoot = true;
            PositionWeapon(aimingPos);
        }

        public void Up()
        {
            CanShoot = true;
            PositionWeapon(shoulderPos);
        }

        public void Down()
        {
            CanShoot = false;
            PositionWeapon(hiddenPos);
        }

        public void Hide()
        {
            CanShoot = false;
            PositionWeapon(hiddenPos, .1f, true);
        }

        private void PositionWeapon(WeaponSubPosition targetPos, float requiredTIme = 0.2f, bool hide = false)
        {
            if (_currentSubPos == targetPos) return;

            if (_positioningWeaponCoroutine != null)
            {
                StopCoroutine(_positioningWeaponCoroutine);
            }

            _positioningWeaponCoroutine = StartCoroutine(PositioningWeapon(targetPos, requiredTIme, hide));
        }

        private Coroutine _positioningWeaponCoroutine;
        

        private IEnumerator PositioningWeapon(WeaponSubPosition targetPos, float time, bool hideAfter = false)
        {
            
            if (_isHidden && !hideAfter)
            {
                weaponTransform.gameObject.SetActive(true);
                _isHidden = false;
            }
            
            weaponTransform.parent = targetPos.transform;
            
            _currentSubPos.isActive = false;
            _currentSubPos = targetPos;
            _currentSubPos.isActive = true;
            
            var startingPos = weaponTransform.localPosition;
            var startingRot = weaponTransform.localRotation;

            var t = 0f;
            while (t <= time)
            {
                t += Time.deltaTime;
                weaponTransform.localPosition = Vector3.Lerp(startingPos, Vector3.zero, t / time);
                weaponTransform.localRotation = Quaternion.Lerp(startingRot, Quaternion.identity, t / time);
                yield return null;
            }

            if (hideAfter && !_isHidden)
            {
                weaponTransform.gameObject.SetActive(false);
                _isHidden = true;
            }
        }

        private void OnEnable()
        {
            // TODO add hidden pos
            _currentSubPos = hiddenPos;
        }
    }
}