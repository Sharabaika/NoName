using UnityEngine;

namespace Weapons
{
    public class SemiAutomaticWeapon : Weapon
    {
        public override void PullMainTrigger()
        {
            if (CanShoot())
            {
                Shoot();
            }
        }
    }
}