using UnityEngine;

namespace Weapons
{
    public class SemiAutomaticWeapon : Weapon
    {
        public override void PullMainTrigger()
        {
            if (RemainingCooldown < 0f && remainingAmmo>0)
            {
                ShootProjectile(projectile, muzzle);
                WasteAmmo();
            }
        }
    }
}