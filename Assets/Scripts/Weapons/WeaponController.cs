using System;
using Player;
using UnityEngine;
using UnityEngine.XR;

namespace Weapons
{
    [RequireComponent(typeof(FPSCamera),typeof(PlayerMovement))]
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon mainWeapon;
        [SerializeField] private Weapon secondaryWeapon;
        [SerializeField] private Transform weaponsParent;

        public Weapon ActiveWeapon { get; private set; }
        public WeaponPositioning Positioning => ActiveWeapon.Positioning;
        
        private bool _canAim;
        public bool CanAim
        {
            get=>_canAim;
            set
            {
                _canAim = value;
                _hidingWeapon = !_hidingWeapon == value;
            }
        }

        private bool _hidingWeapon;
        public bool HidingWeapon
        {
            get => _hidingWeapon;
            set
            {
                _hidingWeapon = value;
                _canAim = !value && _canAim;
            }
        }

        public bool CanShoulder => !HidingWeapon;
        
        public Transform WeaponsParent => weaponsParent;

        private FPSCamera _FPScamera;
        private Transform _cameraT;

        private PlayerMovement _movement;

        public void UpdateWeaponPositioning()
        {
            if (Input.GetKey(KeyCode.Mouse2) && CanAim)
            {
                Positioning.Aim();
                _FPScamera.SwitchToAimingFOV(ActiveWeapon.AimingFov,ActiveWeapon.Positioning.ADSTime);
                return;
            }
            if(CanShoulder)
            {
                Positioning.Shoulder();
            }
            else
            {
                Positioning.Hide();
            }
            _FPScamera.SwitchToNormalFOV();
        }
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
            ActiveWeapon.Positioning.Hide();
        }


        #region Monobehavior callbacks

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
            
            UpdateWeaponPositioning();


            // Reloading
            if (Input.GetKeyDown(KeyCode.R))
            {
                ActiveWeapon.Reload();
            }
        }

        private void Awake()
        {
            _FPScamera = GetComponent<FPSCamera>();
            _cameraT = _FPScamera.AttachedCamera.transform;
            
            _movement = GetComponent<PlayerMovement>();

            mainWeapon.Controller = this;
            mainWeapon.gameObject.SetActive(false);
            
            secondaryWeapon.Controller = this;
            secondaryWeapon.gameObject.SetActive(false);
        }

        #endregion
    }
}