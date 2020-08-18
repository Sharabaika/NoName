using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu]
    public class ProjectileData : ScriptableObject
    {
        public float startingSpeed = 100f;

        public float gravityAcceleration = 0f;
        public bool isGravityAffected = false;

        public float startingDamage = 30f;
        public float damageFallDist = 10f; 
        public float minDamage = 0f;
        public float minDamageDist = 50f;
    }
}