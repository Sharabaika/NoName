using System;
using System.Collections;
using System.Diagnostics;
using Character.CharacterMovement;
using Player;
using UnityEngine;
using UnityEngine.XR;
using Debug = UnityEngine.Debug;

namespace Weapons
{
    [RequireComponent(typeof(FPSCamera),typeof(PlayerMovement), typeof(PlayerInput))]
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon mainWeapon;
        [SerializeField] private Weapon secondaryWeapon;
        [SerializeField] private Transform weaponsParent;
        
        public WeaponPositioning Positioning => _activeWeapon.Positioning;

        public bool CanReload
        {
            get => _canReload;
            set
            {
                _canReload = value;
                if(value == false) _activeWeapon.InterruptReloading();
            }
        }
        private bool _canReload = true;

        public WeaponPositioningRestrictions PositioningRestrictions
        {
            get => _positioningRestrictions;
            set
            {
                _positioningRestrictions = value;
                UpdateWeaponPositioning();
            }
        }
        private WeaponPositioningRestrictions _positioningRestrictions = WeaponPositioningRestrictions.None;
        public Transform WeaponsParent => weaponsParent;

        private PlayerInput _input;
        private Weapon _activeWeapon;
        private FPSCamera _FPScamera;
        private Transform _cameraT;

        #region Monobehavior callbacks

        private void Update()
        {
            if (_input.isMainWeaponSlotKey)
            {
                ChangeWeapon(mainWeapon);
            }
            else if (_input.isSecondaryWeaponSlotKey)
            {
                ChangeWeapon(secondaryWeapon);
            }

            if (_activeWeapon != null)
            {
                _activeWeapon.Positioning.RotateWeapon(_cameraT);
                
                // TODO remove release main trigger etc

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
                    if(CanReload)_activeWeapon.Reload();
                }

                UpdateWeaponPositioning();
            }
        }

        private void Awake()
        {
            _FPScamera = GetComponent<FPSCamera>();
            _cameraT = _FPScamera.AttachedCamera.transform;
            _input = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            var movement = GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.Machine.OnStateChange += UpdateMovementStateRestrictions;
            }
            
            if (mainWeapon != null)
            {
                mainWeapon.gameObject.SetActive(true);
            }
            if (secondaryWeapon != null)
            {
                secondaryWeapon.gameObject.SetActive(true);
                secondaryWeapon.Positioning.PositionWeapon(WeaponPositioning.State.Hidden);
            }
            ChangeWeapon(mainWeapon);
        }


        private void OnDestroy()
        {
            var movement = GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.Machine.OnStateChange -= UpdateMovementStateRestrictions;
            }
        }
        #endregion
        
        private void UpdateMovementStateRestrictions(State newState)
        {
            PositioningRestrictions = newState.Restrictions;
            CanReload = newState.CanReload;
        }
        
        private void UpdateWeaponPositioning()
        {
            if(_activeWeapon is null) return;
            
            if (_input.isAimKey && _positioningRestrictions==WeaponPositioningRestrictions.None)
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Aiming);
                _FPScamera.SwitchToAimingFOV(_activeWeapon.AimingFov,_activeWeapon.Positioning.ADSTime);
                return;
            }
            
            if(_input.isAimKey == false && _positioningRestrictions!=WeaponPositioningRestrictions.ForceLower)
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Up);
            }
            else
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Down);
            }
            _FPScamera.SwitchToNormalFOV();
        }

        private void ChangeWeapon(Weapon weapon)
        {
            // TODO rework as coroutine 
            if(weapon == _activeWeapon) return;

            if (_activeWeapon != null)
            {
                Positioning.PositionWeapon(WeaponPositioning.State.Hidden);
                _activeWeapon.InterruptReloading();
            }

            _activeWeapon = weapon;
            UpdateWeaponPositioning();
        }
    }
}