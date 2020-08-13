using UnityEngine;

namespace Weapons
{
    public class SemiAutomaticWeapon : Weapon
    {
        public override void PullMainTrigger()
        {
            if (RemainingCooldown < 0f)
            {
                ShootProjectile(projectile, muzzle);
            }
        }
    }
}