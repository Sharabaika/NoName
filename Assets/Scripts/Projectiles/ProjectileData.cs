using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu]
    public class ProjectileData : ScriptableObject
    {
        public float StartingSpeed = 100f;
        public float GravityAcceleration = 10f;
        public int   Damage = 1;
    }
}