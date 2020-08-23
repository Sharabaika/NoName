﻿using System;
using System.Collections;
using Projectiles;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    // TODO add ammo, ShootAmmo instead of ShootProjectile
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected float cooldown = 1f;
        [SerializeField] protected float abilityCooldown = 1f;
        
        [SerializeField] protected Projectile projectile;
        [SerializeField] protected Transform muzzle;

        [SerializeField, Range(0f, 2f)] protected float coneRadius = 1f;
        [SerializeField, Range(0f, 2f)] protected float coneHeight = 1f;

        [SerializeField] protected int magazineCapacity = 25;
        protected int remainingAmmo;

        [SerializeField] protected ProjectileData projectileData;

        [SerializeField] private WeaponPositioning positioningPrefab;
        
        // TODO rework 
        public WeaponPositioning Positioning { get; private set; }

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

        public virtual void PullSecondaryTrigger()
        {
            Positioning.Aim();
        }

        public virtual void ReleaseSecondaryTrigger()
        {
            Positioning.Shoulder();
        }

        public virtual void Reload()
        {
            remainingAmmo = magazineCapacity;
        }

        public virtual bool CanShoot()
        {
            return remainingAmmo > 0 && RemainingCooldown < 0f;
        }

        public virtual bool CanUseAbility()
        {
            return true;
        }
        public float RemainingCooldown => lastFired + cooldown - Time.time;
        public float RemainingAbilityCooldown => lastAbilityUse + abilityCooldown - Time.time;

        protected float lastFired = float.NegativeInfinity;
        protected float lastAbilityUse = float.NegativeInfinity;

        protected void WasteAmmo(int amount=1)
        {
            remainingAmmo = Mathf.Max(0, remainingAmmo - amount);
        }
        
        protected virtual Projectile ShootProjectile(Projectile projectileToShoot,Transform muzzleTransform)
        {
            lastFired = Time.time;
            // TODO mb just rotate by random Euler angle 
            var proj = Instantiate(projectileToShoot, muzzleTransform.position,
                Quaternion.FromToRotation(muzzleTransform.forward, RandomDirectionToConeBase()) * muzzleTransform.rotation);
            proj.ProjectileData = projectileData;
            return proj;
        }

        protected Vector3 RandomDirectionToConeBase()
        {
            var baseVector = Random.insideUnitCircle * coneRadius;
            var pointPos = muzzle.forward * coneHeight + muzzle.up * baseVector.y + muzzle.right * baseVector.x;
            return pointPos.normalized;
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
            projectile.ProjectileData = projectileData;
        }

        #endregion
    }
}