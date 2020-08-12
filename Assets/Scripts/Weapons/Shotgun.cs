using System;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class Shotgun: Weapon
    {
        [SerializeField] private ParticleSystem leftBarrel;
        [SerializeField] private ParticleSystem rightBarrel;


        protected override void FireMain()
        {
            Debug.Log("kek");
            leftBarrel.Play();
        }

        protected override void FireSecondary()
        {
            rightBarrel.Play();
        }
    }
}