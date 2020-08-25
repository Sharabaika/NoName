using System;
using Projectiles;
using UnityEditor;
using UnityEngine;

namespace Weapons
{
    // TODO add ammo, ShootAmmo instead of ShootProjectile
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

        [SerializeField] private WeaponPositioning positioningPrefab;

        // TODO rework 
        public float AimingFov => aimingFOV;
        public WeaponPositioning Positioning { get; private set; }
        public float RemainingCooldown => lastFired + _cooldown - Time.time;

        private WeaponController _controller;

        public WeaponController Controller
        {
            get => _controller;
            set
            {
                _controller = value;
                var weaponsParent = _controller.WeaponsParent;
                if (Positioning != null)
                {
                    Destroy(Positioning.gameObject);
                }
                Positioning = Instantiate(positioningPrefab, weaponsParent.position,
                    weaponsParent.rotation, weaponsParent);
                Positioning.weaponTransform = transform;
            }
        }


        public virtual void PullMainTrigger(){}

        public virtual void ReleaseMainTrigger(){}

        public virtual void PullSecondaryTrigger(){}

        public virtual void ReleaseSecondaryTrigger(){}
        
        public virtual void Reload()
        {
            remainingAmmo = magazineCapacity;
        }

        private float _cooldown;

        protected float lastFired = float.NegativeInfinity;

        protected virtual bool CanShoot()
        {
            return remainingAmmo > 0 && RemainingCooldown < 0f;
        }

        protected virtual void WasteAmmo(int amount=1)
        {
            if(amount>remainingAmmo) throw new Exception("cannot waste more ammo than have");
            remainingAmmo -= amount;
        }
        
        protected virtual void Shoot()
        {
            lastFired = Time.time;
            ammo.ShootInCone(muzzle,projectileData,coneRadius,coneHeight);
            WasteAmmo();
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (muzzle != null)
            {
                var coneBase = muzzle.position + muzzle.forward * coneHeight;
                Handles.DrawWireDisc(coneBase, muzzle.forward, coneRadius);
                Handles.DrawLine(coneBase + muzzle.up * coneRadius, muzzle.position);
                Handles.DrawLine(coneBase + muzzle.up * (-coneRadius), muzzle.position);
                Handles.DrawLine(coneBase + muzzle.right * coneRadius, muzzle.position);
                Handles.DrawLine(coneBase + muzzle.right * (-coneRadius), muzzle.position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(muzzle.position,muzzle.forward);
        }
#endif

        #region MonobehaviorEvents

        protected virtual void Awake()
        {
            remainingAmmo = magazineCapacity;
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