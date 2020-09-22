using System;
using UnityEngine;

namespace Weapons
{
    // TODO rework animators for only one controller
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimator : MonoBehaviour
    {
        [SerializeField] private string idleString = "Iddle";
        [SerializeField] private string switchMagString = "SwitchMag";
        [SerializeField] private string pullTheBoltString = "PullTheBolt";
        [SerializeField] private string isMainTriggerPulledString = "isMainTriggerPulled";

        public bool isMainTriggerPulled
        {
            get => _animator.GetBool(isMainTriggerPulledString);
            set => _animator.SetBool(isMainTriggerPulledString, value);
        }

        private Animator _animator;

        public void PullMainTrigger()
        {
            isMainTriggerPulled = true;
        }

        public void ReleaseMainTrigger()
        {
            isMainTriggerPulled = false;
        }

        public void InterruptReloading()
        {
            _animator.Play(idleString);
        }

        public void SwitchMag()
        {
            _animator.CrossFade(switchMagString,0,0);
        }

        public void PullTheBolt()
        {
            _animator.CrossFade(pullTheBoltString, 0, 0);
        }

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }
    }
}