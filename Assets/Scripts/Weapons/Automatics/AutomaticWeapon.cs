using System.Collections;
using UnityEngine;

namespace Weapons.Automatics
{
    public class AutomaticWeapon : Weapon
    {
        // protected IEnumerator shootingCoroutine;
        protected Coroutine shootingCoroutine;

        protected override void OnPullMainTrigger()
        {
            shootingCoroutine = StartCoroutine(AutomaticFire());
        }

        protected override void OnReleaseMainTrigger()
        {
            StopCoroutine(shootingCoroutine);
        }

        protected virtual IEnumerator AutomaticFire(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            while (remainingAmmo > 0)
            {
                yield return new WaitForSeconds(RemainingCooldown);
                Shoot();
            }
        }

    }
}