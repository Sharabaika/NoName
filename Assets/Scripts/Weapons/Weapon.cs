using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    // TODO remove secondaryAttack  
    public abstract class Weapon : MonoBehaviour
    {
        // TODO serialize like separate values MainCooldown and SecondaryCooldown
        // TODO rework)0
        [SerializeField]protected float[] cooldowns = new float[]{1f,1f};

        [SerializeField] protected Transform muzzle;

        [SerializeField, Range(0f, 2f)] protected float ConeRadius = 1f;
        [SerializeField, Range(0f, 2f)] protected float ConeHeight = 1f;


        public void TryFireMain() => TryFire(0);
        public void TryFireSecondary() => TryFire(1);

        public float RemainingMainCooldown =>GetRemainingCooldown(0);
        public float RemainingSecondaryCooldown => GetRemainingCooldown(1);

        // TODO add automatic/single modes, use GetKeyDown/GetKey 

        private float[] lastFired = new[] {float.MinValue, float.MinValue};

        private float GetRemainingCooldown(int mode)
        {
            // if(mode<0 || mode>1) throw new ArgumentOutOfRangeException();
            return Mathf.Max(0, lastFired[mode] + cooldowns[mode] - Time.time);
        }

        protected abstract void FireMain();
        protected abstract void FireSecondary();    
        
        private void TryFire(int mode)
        {
            if (GetRemainingCooldown(mode) <= float.Epsilon )
            {
                FireMode(mode);
            }
            else
            {
                // TODO cant fire
            }
        }

        private void FireMode(int mode)
        {
            // TODO add fire effects
            
            lastFired[mode] = Time.time;
            // TODO seems not good
            switch (mode)
            {
                case 0:
                    FireMain();
                    return;
                case 1:
                    FireSecondary();
                    return;
                default:
                    return;
            }
        }

        protected Vector3 RandomDirectionToConeBase()
        {
            var baseVector = Random.insideUnitCircle * ConeRadius;
            var pointPos = muzzle.forward * ConeHeight + muzzle.up * baseVector.y + muzzle.right * baseVector.x;
            return pointPos.normalized;
        }
        
#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (muzzle != null)
            {
                var coneBase = muzzle.position + muzzle.forward * ConeHeight;
                Handles.DrawWireDisc(coneBase, muzzle.forward, ConeRadius);
                Handles.DrawLine(coneBase + muzzle.up * ConeRadius, muzzle.position);
                Handles.DrawLine(coneBase + muzzle.up * (-ConeRadius), muzzle.position);
                Handles.DrawLine(coneBase + muzzle.right * ConeRadius, muzzle.position);
                Handles.DrawLine(coneBase + muzzle.right * (-ConeRadius), muzzle.position);
            }
        }
#endif
    }
}