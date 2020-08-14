using System.Collections;
using UnityEngine;

namespace Weapons.Automatics
{
    public class Famas : AutomaticWeapon
    {
        [SerializeField] private float burstRateOfFire = 0.3f;
        [SerializeField] private int burstCount = 3;
        public float RemainingBurstShootingCooldown => lastFired + burstRateOfFire - Time.time;

        private bool _isBursting = false;
        
        public override void PullMainTrigger()
        {
            if (_isBursting == false)
            {
                shootingCoroutine = StartCoroutine(AutomaticFire());
            }
        }

        public override void ReleaseMainTrigger()
        {
            if (_isBursting == false && shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
            }
        }

        public override void PullSecondaryTrigger()
        {
            if (CanUseAbility() && _isBursting==false)
            {
                if (shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine);
                }
                shootingCoroutine = StartCoroutine(BurstShooting(burstCount));
            }
        }


        private IEnumerator BurstShooting(int count)
        {
            _isBursting = true;
            lastAbilityUse = Time.time;
            for (int i = 0; i < count && remainingAmmo > 0; i++)
            {
                yield return new WaitForSeconds(RemainingBurstShootingCooldown);
                WasteAmmo();
                var newProjectile = ShootProjectile(projectile, muzzle);
            }

            _isBursting = false;
        }
    }
}