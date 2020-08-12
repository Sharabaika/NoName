using System;
using UnityEngine;
using Weapons;

namespace Player
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                weapon.TryFireMain();
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                weapon.TryFireSecondary();
            }
        }
    }
}