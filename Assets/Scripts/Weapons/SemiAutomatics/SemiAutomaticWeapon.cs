using UnityEngine;

namespace Weapons
{
    public class SemiAutomaticWeapon : Weapon
    {
        protected override void OnPullMainTrigger()
        {
            if (CanShoot())
            {
                Shoot();
            }
        }
    }
}