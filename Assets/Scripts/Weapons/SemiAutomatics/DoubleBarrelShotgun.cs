using UnityEngine;

namespace Weapons.SemiAutomatics
{
    // TODO rework editor
    public class DoubleBarrelShotgun: Shotgun
    {
        [SerializeField] protected Transform secondMuzzle;
        
        private bool _isSecondBarrelLoaded = true;
        private bool _isMainBarrelLoaded = true;

        protected override void OnPullMainTrigger()
        {
            if (_isMainBarrelLoaded)
            {
                ammo.ShootInCone(muzzle,projectileData,coneRadius,coneHeight);
                _isMainBarrelLoaded = false;
            }
        }

        protected override void OnPullSecondaryTrigger()
        {
            if (_isSecondBarrelLoaded)
            {
                ammo.ShootInCone(secondMuzzle,projectileData,coneRadius,coneHeight);
                _isSecondBarrelLoaded = false;
            }
        }

        public override void Reload()
        {
            base.Reload();
            _isMainBarrelLoaded = true;
            _isSecondBarrelLoaded = true;
        }
    }
}