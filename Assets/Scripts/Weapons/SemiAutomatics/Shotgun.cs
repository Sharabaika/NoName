using Projectiles;
using UnityEngine;

namespace Weapons.SemiAutomatics
{
    public class Shotgun : SemiAutomaticWeapon
    {
        [SerializeField] protected int projectilesPerShot = 50;
        
        protected Projectile[] ShootProjectiles(Projectile projectileToShoot, int count, Transform muzzleTransform)
        {
            var res = new Projectile[count];
            for (int i = 0; i < count; i++)
            {
                res[i] = ShootProjectile(projectileToShoot, muzzleTransform);
            }

            return res;
        }

        public override void PullMainTrigger()
        {
            ShootProjectiles(projectile,projectilesPerShot,muzzle);
        }
    }
}