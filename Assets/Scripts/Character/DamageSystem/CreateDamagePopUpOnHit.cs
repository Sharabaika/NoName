using System;
using UnityEngine;

namespace Character.DamageSystem
{
    [RequireComponent(typeof(IDamageable))]
    public class CreateDamagePopUpOnHit : MonoBehaviour
    {
        // TODO mb reference ScriptableObject with style instead of Asset
        public DamagePopUp popUpPrefab;
        private IDamageable _damageable;

        private void InstantiatePopUp(int dmg, Vector3 pos, Vector3 _) => DamagePopUp.Instantiate(popUpPrefab, pos, dmg);

        private void OnEnable()
        {
            _damageable = GetComponent<IDamageable>();
            _damageable.OnTakeHit += InstantiatePopUp;
        }

        private void OnDisable()
        {
            _damageable.OnTakeHit -= InstantiatePopUp;
        }
    }
}