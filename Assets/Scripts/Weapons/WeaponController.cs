using System;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(FPSCamera))]
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private Transform _weaponTransform;
        
        [SerializeField] private WeaponPositioning weaponPositioning;
        [SerializeField] private Transform shoulderTransform;

        public WeaponPositioning Positioning => weapon.positioning;

        private Transform cameraT;

        private void Awake()
        {
            cameraT = GetComponent<FPSCamera>().CameraTransform;
            Positioning.weaponTransform = weapon.transform;
        }

        private void Update()
        {
            Positioning.RotateWeapon(cameraT);
            
            // Shooting
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                weapon.PullMainTrigger();
            }
            // || Input.GetKey(KeyCode.Mouse0)==false may remove bags
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                weapon.ReleaseMainTrigger();
            }
            
            
            // Weapon ability
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                weapon.PullSecondaryTrigger();
                Positioning.Aim();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                weapon.ReleaseSecondaryTrigger();
                Positioning.Shoulder();
            }

            
            // Reloading
            if (Input.GetKeyDown(KeyCode.R))
            {
                weapon.Reload();
            }
        }
    }
}