using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

namespace Weapons
{
    public class WeaponPositioning : MonoBehaviour
    {
        public enum State
        {
            Aiming,
            Shoulder,
            Hidden
        }
        
        [SerializeField] private Transform shoulderPos; 
        [SerializeField] private Transform aimingPos;
        [SerializeField] private Transform hiddenPos;

        public Transform weaponTransform;
        public Transform camTransform;

        private void Awake()
        {
            camTransform = Camera.main.transform;
        }

        // TODO Rotate around camera pos
        public void RotateWeapon(Transform cameraT)
        {
            var angle = aimingPos.localEulerAngles.x - cameraT.localEulerAngles.x;
            var cameraPosition = cameraT.position;
            aimingPos.RotateAround(cameraPosition,cameraT.right,-angle);
            shoulderPos.RotateAround(cameraPosition,cameraT.right,-angle);
        }

        public void Aim()
        {
            SetWeaponState(aimingPos);
        }

        public void Shoulder()
        {
            SetWeaponState(shoulderPos);
        }

        public void Hide()
        {
            SetWeaponState(hiddenPos);
        }

        private void SetWeaponState(Transform target)
        {
            if (_stateChangingCoroutine != null)
            {
                StopCoroutine(_stateChangingCoroutine);
            }
            _stateChangingCoroutine = StartCoroutine(ChangeState(target, 0.2f));
        }

        private Coroutine _stateChangingCoroutine;
        private IEnumerator ChangeState(Transform target, float time)
        {
            weaponTransform.parent = target;
            var startingPos = weaponTransform.localPosition;
            var startingRot = weaponTransform.localRotation;
            var t = 0f;
            while (t<=time)
            {
                t += Time.deltaTime;
                weaponTransform.localPosition = Vector3.Lerp(startingPos,Vector3.zero, t/time);
                weaponTransform.localRotation = Quaternion.Lerp(startingRot,Quaternion.identity, t/time);
                yield return null;
            }
        }
    }
}