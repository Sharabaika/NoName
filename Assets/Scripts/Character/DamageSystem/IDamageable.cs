using System;
using UnityEngine;

namespace Character.DamageSystem
{
    public interface IDamageable
    {
        void TakeDamage(int damage);

        void TakeHit(int damage, Vector3 hitPos, Vector3 hitDirection);

        /// <summary>
        /// (int damageAmount)
        /// </summary>
        event Action<int> OnTakeDamage;

        /// <summary>
        /// (int damage, Vector3 pos, Vector3 hitDirection)
        /// </summary>
        event Action<int, Vector3, Vector3> OnTakeHit;
    }
}