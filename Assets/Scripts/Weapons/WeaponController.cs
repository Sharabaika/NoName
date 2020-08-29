using System;
using System.Collections;
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

        public WeaponPositioning Positioning => _activeWeapon.Positioning;

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

        private Weapon _activeWeapon;
        private FPSCamera _FPScamera;
        private Transform _cameraT;

        private PlayerMovement _movement;

        public void UpdateWeaponPositioning()
        {
            if (Input.GetKey(KeyCode.Mouse2) && CanAim)
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Aiming);
                _FPScamera.SwitchToAimingFOV(_activeWeapon.AimingFov,_activeWeapon.Positioning.ADSTime);
                return;
            }
            if(CanShoulder)
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Shoulder);
            }
            else
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Hidden);
            }
            _FPScamera.SwitchToNormalFOV();
        }
        
        
        // private IEnumerator _changeWeaponCoroutine(Weapon weapon)
        // {
        //     _isChangingWeapon = true;
        //     if (_activeWeapon != null)
        //     {
        //         yield return _activeWeapon.Positioning.PositionWeapon(WeaponPositioning.State.Hidden);
        //         _activeWeapon.gameObject.SetActive(false);
        //     }
        //
        //     if (weapon != null)
        //     {
        //         weapon.gameObject.SetActive(true);
        //         yield return weapon.Positioning.PositionWeapon(WeaponPositioning.State.Shoulder);
        //         _isWeaponActive = true;
        //     }
        //     else
        //     {
        //         _isWeaponActive = false;
        //     }
        //
        //     _activeWeapon = weapon;
        //     _isChangingWeapon = false;
        // }
        
        
        private void ChangeWeapon(Weapon weapon)
        {
            if(weapon == _activeWeapon) return;

            if (_activeWeapon != null)
            {
                // _activeWeapon.Positioning.PositionWeapon(WeaponPositioning.State.Hidden);
                _activeWeapon.gameObject.SetActive(false);
            }

            _activeWeapon = weapon;
            _activeWeapon.gameObject.SetActive(true);
            // _activeWeapon.Positioning.PositionWeapon(WeaponPositioning.State.Hidden);
        }


        #region Monobehavior callbacks

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

            if (_activeWeapon != null)
            {
                _activeWeapon.Positioning.RotateWeapon(_cameraT);

                // Shooting
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _activeWeapon.PullMainTrigger();
                }

                // || Input.GetKey(KeyCode.Mouse0)==false may remove bags
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    _activeWeapon.ReleaseMainTrigger();
                }

                // Weapon ability
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    _activeWeapon.PullSecondaryTrigger();
                }

                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    _activeWeapon.ReleaseSecondaryTrigger();
                }

                // Reloading
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _activeWeapon.Reload();
                }

                UpdateWeaponPositioning();
            }
        }

        private void Awake()
        {
            _FPScamera = GetComponent<FPSCamera>();
            _cameraT = _FPScamera.AttachedCamera.transform;
            
            _movement = GetComponent<PlayerMovement>();
        }

        // private void OnEnable()
        // {
        //     if (mainWeapon != null)
        //     {
        //         mainWeapon.gameObject.SetActive(false);
        //     }
        //     else if (secondaryWeapon != null)
        //     {
        //         secondaryWeapon.gameObject.SetActive(false);
        //     }
        // }

        private void Start()
        {
            if (mainWeapon != null)
            {
                ChangeWeapon(mainWeapon);
            }
            else if (secondaryWeapon != null)
            {
                ChangeWeapon(secondaryWeapon);
            }
        }

        #endregion
    }
}