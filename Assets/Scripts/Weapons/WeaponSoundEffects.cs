using System;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponSoundEffects : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        private Weapon _weapon;

        [SerializeField] private AudioClip _shootingEffect;
        [SerializeField] private AudioClip _reloadingEffect;
        [SerializeField] private AudioClip _breechblockEffect;
        private void OnEnable()
        {
            _weapon = GetComponent<Weapon>();
            if(_weapon!=null)_weapon.onShoot += ShootingEffect;
        }

        private void OnDisable()
        {
            if(_weapon!=null)_weapon.onShoot -= ShootingEffect;
        }

        private void ShootingEffect()
        {
            PlayClip(_shootingEffect);
        }

        private void PlayClip(AudioClip clip)
        {
            _source.Stop();
            _source.clip = clip;
            _source.Play();
        }
    }
}