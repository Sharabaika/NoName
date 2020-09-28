using System;
using System.Collections;
using Projectiles;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    [RequireComponent(typeof(WeaponAnimator))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected float aimingFOV = 45f;
        [SerializeField] protected float rateOfFire = 400f;

        
        [SerializeField] protected Ammunition.Ammo ammo;
        [SerializeField] protected Transform muzzle;
        
        [SerializeField, Range(0f, 2f)] protected float coneRadius = 1f;
        [SerializeField, Range(0f, 2f)] protected float coneHeight = 1f;

        [SerializeField] protected int magazineCapacity = 25;
        protected int remainingAmmo;

        [SerializeField] protected float MagSwitchingTime = 1f;
        [SerializeField] protected float BoltActionTime = 0.5f;

        [SerializeField] protected ProjectileData projectileData;

        [SerializeField] protected float verticalRecoil = 10f;
        [SerializeField] protected float horizontalRecoil = 5f;

        // Delegates
        public event Action onShoot;
        public event Action onMagSwitch;
        public event Action onBoltAction;
        
        public float AimingFov => aimingFOV;
        public WeaponPositioning Positioning { get; private set; }
        protected float RemainingCooldown => lastFired + _cooldown - Time.time;

        protected float lastFired = float.NegativeInfinity;

        private WeaponAnimator _weaponAnimator;
        private float _cooldown;


        public void PullMainTrigger()
        {
            _weaponAnimator.PullMainTrigger();
            OnPullMainTrigger();
        }

        public void ReleaseMainTrigger()
        {
            _weaponAnimator.ReleaseMainTrigger();
            OnReleaseMainTrigger();
        }

        public void PullSecondaryTrigger()
        {
            // animator
            OnPullSecondaryTrigger();
        }

        public void ReleaseSecondaryTrigger()
        {
            // animator
            OnReleaseSecondaryTrigger();
        }
        
        // Triggers
        protected virtual void OnPullMainTrigger(){}

        protected virtual void OnReleaseMainTrigger(){}

        protected virtual void OnPullSecondaryTrigger(){}

        protected virtual void OnReleaseSecondaryTrigger(){}

        protected virtual bool CanReload()
        {
            return remainingAmmo < magazineCapacity;
        }
        protected bool IsNeededToPullTheBolt()
        {
            return remainingAmmo == 0;
        }

        protected bool isReloading = false;
        protected bool isMagChanged = true;
        protected bool isBoltPulled = true;
        private Coroutine _reloadingCoroutine;
        
        protected virtual IEnumerator Reloading()
        {
            isReloading = true;

            if (isMagChanged == false)
            {
                _weaponAnimator.SwitchMag();
                onMagSwitch?.Invoke();
                yield return new WaitForSeconds(MagSwitchingTime);
                isMagChanged = true;
                remainingAmmo = magazineCapacity;
            }

            if (isBoltPulled == false)
            {
                _weaponAnimator.PullTheBolt();
                yield return new WaitForSeconds(BoltActionTime);
                isBoltPulled = true;
            }

            isReloading = false;
        }

        public void Reload()
        {
            // TODO sync with animator
            if (CanReload() && isReloading == false)
            {
                isMagChanged = false;
                isBoltPulled = !IsNeededToPullTheBolt();
                _reloadingCoroutine = StartCoroutine(Reloading());
            }
        }

        public void InterruptReloading()
        {
            isReloading = false;
            if(_reloadingCoroutine!=null)
                StopCoroutine(_reloadingCoroutine);
            _weaponAnimator.InterruptReloading();
        }

        
        // Shooting
        protected virtual bool CanShoot()
        {
            // TODO lmao what is it
            return remainingAmmo > 0 && RemainingCooldown < 0f && !isReloading && isBoltPulled && Positioning.CanShoot;
        }

        protected virtual void WasteAmmo(int amount=1)
        {
            if(amount>remainingAmmo) throw new Exception("cannot waste more ammo than u have");
            remainingAmmo -= amount;
        }


        protected virtual Vector3 GetRecoil()
        {
            Vector3 p = Random.insideUnitCircle;
            p.x *= horizontalRecoil;
            p.y *= verticalRecoil;
            return p;
        }
        
        protected virtual void Shoot()
        {
            if (!CanShoot()) return;

            lastFired = Time.time;
            ammo.ShootStraight(muzzle, projectileData);
            onShoot?.Invoke();
            Positioning.CurrentSubPos.AddRecoil(GetRecoil());
            WasteAmmo();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(muzzle.position,muzzle.forward);
        }
#endif

        #region MonobehaviorEvents

        protected virtual void OnEnable()
        {
            remainingAmmo = magazineCapacity;
            Positioning = GetComponentInChildren<WeaponPositioning>();
            _weaponAnimator = GetComponent<WeaponAnimator>();
        }

        #endregion

        private void OnValidate()
        {
            if (rateOfFire > 0f)
            {
                _cooldown = 60f / rateOfFire;
            }
            else
            {
                _cooldown = 1f;
            }
        }
    }
}