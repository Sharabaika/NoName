using System;
using UnityEngine;
using UnityEngine.XR;

namespace Weapons
{
    [RequireComponent(typeof(FPSCamera))]
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon mainWeapon;
        [SerializeField] private Weapon secondaryWeapon;
        [SerializeField] private Transform weaponsParent;
        
        private Transform _cameraT;
        public Weapon ActiveWeapon { get; private set; }

        public Transform WeaponsParent => weaponsParent;

        private void Awake()
        {
            _cameraT = GetComponent<FPSCamera>().CameraTransform;
            mainWeapon.Controller = this;
            secondaryWeapon.Controller = this;
        }

        private void Start()
        {
            if (secondaryWeapon != null)
            {
                ChangeWeapon(secondaryWeapon);
            }
            else if (mainWeapon != null)
            {
                ChangeWeapon(mainWeapon);
            }
        }
        
        // TODO fix changing weapons while running
        private void ChangeWeapon(Weapon weapon)
        {
            if(weapon == ActiveWeapon) return;

            if (ActiveWeapon != null)
            {
                ActiveWeapon.Positioning.Hide();
                ActiveWeapon.gameObject.SetActive(false);
            }

            ActiveWeapon = weapon;
            ActiveWeapon.gameObject.SetActive(true);
            ActiveWeapon.Positioning.Shoulder();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeWeapon(mainWeapon);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeWeapon(secondaryWeapon);
            }

            ActiveWeapon.Positioning.RotateWeapon(_cameraT);

            // Shooting
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ActiveWeapon.PullMainTrigger();
            }

            // || Input.GetKey(KeyCode.Mouse0)==false may remove bags
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                ActiveWeapon.ReleaseMainTrigger();
            }


            // Weapon ability
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ActiveWeapon.PullSecondaryTrigger();
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                ActiveWeapon.ReleaseSecondaryTrigger();
            }


            // Reloading
            if (Input.GetKeyDown(KeyCode.R))
            {
                ActiveWeapon.Reload();
            }
        }
    }
}