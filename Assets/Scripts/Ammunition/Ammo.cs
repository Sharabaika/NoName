using Projectiles;
using UnityEngine;

namespace Ammunition
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] protected Projectile projectile;

        // unused
        public virtual void ShootStraight(Transform muzzle, ProjectileData data)
        {
            Instantiate(projectile, muzzle.position, muzzle.rotation).ProjectileData = data;
        }

        public virtual void ShootInCone(Transform muzzle, ProjectileData data, float coneRadius, float coneHeight)
        {
            var rotation = muzzle.rotation *
                           Quaternion.FromToRotation(Vector3.forward,
                               RandomDirectionToConeBase(coneRadius, coneHeight));
            Instantiate(projectile, muzzle.position, rotation).ProjectileData = data;
        }
        
        protected Vector3 RandomDirectionToConeBase(float coneRadius, float coneHeight)
        {
            var baseVector = Random.insideUnitCircle * coneRadius;
            var pointPos = Vector3.forward * coneHeight + Vector3.up * baseVector.y + Vector3.right * baseVector.x;
            return pointPos.normalized;
        }
    }
}