using System.Collections;
using System.Runtime.InteropServices;
using Projectiles;
using UnityEngine;

namespace Weapons
{
    public class AutomaticWeapon : Weapon
    {
        // protected IEnumerator shootingCoroutine;
        protected Coroutine shootingCoroutine;

        public override void PullMainTrigger()
        {
            shootingCoroutine = StartCoroutine(AutomaticFire());
        }

        public override void ReleaseMainTrigger()
        {
            StopCoroutine(shootingCoroutine);
        }

        protected virtual IEnumerator AutomaticFire(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            while (remainingAmmo > 0)
            {
                yield return new WaitForSeconds(RemainingCooldown);
                WasteAmmo();
                var newProjectile = ShootProjectile(projectile, muzzle);
            }
        }

    }
}