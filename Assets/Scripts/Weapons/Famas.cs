using System;
using Projectiles;
using UnityEngine;

namespace Weapons
{
    public class Famas : Weapon
    {
        // TODO rework for bullets and scope
        [SerializeField] private Projectile bulletPrefab;
        
        protected override void FireMain()
        {
            ShootBullet();
        }

        protected override void FireSecondary()
        {
            ShootBullet();
        }

        private Projectile ShootBullet()
        {
            // TODO mb just rotate by random Euler angle 
            return Instantiate(bulletPrefab, muzzle.transform.position,
                Quaternion.FromToRotation(muzzle.forward, RandomDirectionToConeBase()) * muzzle.rotation);
        }
    }
}