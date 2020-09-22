using System;
using System.Collections;
using Projectiles;
using UnityEditor;
using UnityEngine;

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

        [SerializeField] protected ProjectileData projectileData;

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


        public virtual void Reload()
        {
            _weaponAnimator.Reload(remainingAmmo>0);
            remainingAmmo = magazineCapacity;
        }

        protected virtual bool CanShoot()
        {
            return remainingAmmo > 0 && RemainingCooldown < 0f;
        }

        protected virtual void WasteAmmo(int amount=1)
        {
            if(amount>remainingAmmo) throw new Exception("cannot waste more ammo than u have");
            remainingAmmo -= amount;
        }
        
        protected virtual void Shoot()
        {
            lastFired = Time.time;
            ammo.ShootInCone(muzzle,projectileData,coneRadius,coneHeight);
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