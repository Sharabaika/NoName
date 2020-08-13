using System.Collections;
using UnityEngine;

namespace Weapons.Automatics
{
    public class Famas : AutomaticWeapon
    {
        [SerializeField] private float burstRateOfFire = 0.3f;
        [SerializeField] private int burstCount = 3;
        public float RemainingBurstShootingCooldown => lastFired + burstRateOfFire - Time.time;

        protected IEnumerator current;
        
        public override void PullMainTrigger()
        {
            current = AutomaticFire();
            StartCoroutine(current);
        }

        public override void ReleaseMainTrigger()
        {
            StopCoroutine(current);
        }

        public override void PullSecondaryTrigger()
        {
            if (CanUseAbility())
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = StartCoroutine(BurstShooting(burstCount));
            }
        }


        private IEnumerator BurstShooting(int count)
        {
            lastAbilityUse = Time.time;
            for (int i = 0; i < count && remainingAmmo > 0; i++)
            {
                yield return new WaitForSeconds(RemainingBurstShootingCooldown);
                WasteAmmo();
                var newProjectile = ShootProjectile(projectile, muzzle);
            }
        }
    }
}