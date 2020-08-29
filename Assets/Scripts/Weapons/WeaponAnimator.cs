using System;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimator : MonoBehaviour
    {
        [SerializeField] private string reloadAnimationString = "Reload";
        [SerializeField] private string isTacticalReloadString = "isTacticalReload";
        [SerializeField] private string isMainTriggerPulledString = "isMainTriggerPulled";

        public bool isTacticalReload
        {
            get => _animator.GetBool(isTacticalReloadString);
            set => _animator.SetBool(isTacticalReloadString, value);
        }

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

        public void Reload(bool isTactical = false)
        {
            isTacticalReload = isTactical;
            _animator.Play(reloadAnimationString,0,0);
        }

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }
    }
}