using UnityEngine;

namespace Weapons.SemiAutomatics
{
    // TODO rework editor
    public class DoubleBarrelShotgun: Shotgun
    {
        [SerializeField] protected Transform secondMuzzle;
        
        private bool _isSecondBarrelLoaded = true;
        private bool _isMainBarrelLoaded = true;
        
        public override void PullMainTrigger()
        {
            if (_isMainBarrelLoaded)
            {
                ammo.ShootInCone(muzzle,projectileData,coneRadius,coneHeight);
                _isMainBarrelLoaded = false;
            }
        }

        public override void PullSecondaryTrigger()
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