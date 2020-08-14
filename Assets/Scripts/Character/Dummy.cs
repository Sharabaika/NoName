using System;
using Character.DamageSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    public class Dummy : MonoBehaviour, IDamageable
    {
        public event Action<int> OnTakeDamage;

        public event Action<int, Vector3, Vector3> OnTakeHit;


        public void TakeDamage(int damage)
        {
            OnTakeDamage?.Invoke(damage);
        }

        public void TakeHit(int damage, Vector3 hitPos, Vector3 hitDirection)
        {
            OnTakeHit?.Invoke(damage, hitPos, hitDirection);
        }
    }
}