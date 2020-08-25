using Projectiles;
using UnityEngine;

namespace Ammunition
{
    public class ShotgunAmmo : Ammo
    {
        [SerializeField] private int numberOfProjectiles = 15;

        public override void ShootInCone(Transform muzzle, ProjectileData data, float coneRadius, float coneHeight)
        {
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var rotation = muzzle.rotation *
                               Quaternion.FromToRotation(Vector3.forward,
                                   RandomDirectionToConeBase(coneRadius, coneHeight));
                Instantiate(projectile, muzzle.position, rotation).ProjectileData = data;
            }
        }
    }
}