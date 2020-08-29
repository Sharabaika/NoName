using System.Collections;
using UnityEngine;

namespace Weapons.Automatics
{
    public class BurstingAutomaticWeapon : AutomaticWeapon
    {
        [SerializeField] private float burstRateOfFire = 0.3f;
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstCooldown = 1f;
        
        private float RemainingBurstShootingCooldown => lastFired + burstRateOfFire - Time.time;

        private bool CanUseAbility()
        {
            return RemainingBurstShootingCooldown < 0f && remainingAmmo > 0;
        }
        
        private float _lastBurstTiming = float.NegativeInfinity;
        private bool _isBursting = false;

        protected override void OnPullMainTrigger()
        {
            if (_isBursting == false)
            {
                shootingCoroutine = StartCoroutine(AutomaticFire());
            }
        }

        protected override void OnReleaseMainTrigger()
        {
            if (_isBursting == false && shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
            }
        }

        protected override void OnPullSecondaryTrigger()
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
            _lastBurstTiming = Time.time;
            for (int i = 0; i < count && remainingAmmo > 0; i++)
            {
                yield return new WaitForSeconds(RemainingBurstShootingCooldown);
                Shoot();
            }

            _isBursting = false;
        }
    }
}