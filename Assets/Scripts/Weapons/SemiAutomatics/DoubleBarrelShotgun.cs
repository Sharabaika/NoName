using UnityEngine;

namespace Weapons.SemiAutomatics
{
    // TODO rework editor
    public class DoubleBarrelShotgun: Shotgun
    {
        [SerializeField] protected Transform secondMuzzle;
        private bool _isSecondBarrelLoaded = true;
        
        
        public override void PullMainTrigger()
        {
            if (CanShoot())
            {
                WasteAmmo();
                ShootProjectiles(projectile, projectilesPerShot, muzzle);
            }
        }

        public override void PullSecondaryTrigger()
        {
            if (CanUseAbility())
            {
                _isSecondBarrelLoaded = false;
                ShootProjectiles(projectile, projectilesPerShot, secondMuzzle);
            }
        }

        public override bool CanShoot()
        {
            return remainingAmmo > 0;
        }

        public override bool CanUseAbility()
        {
            return _isSecondBarrelLoaded;
        }

        public override void Reload()
        {
            base.Reload();
            _isSecondBarrelLoaded = true;
        }
    }
}