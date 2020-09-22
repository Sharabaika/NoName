using System.Collections;
using UnityEngine;

namespace Weapons
{
    
    // TODO rework as flag
    public enum WeaponPositioningRestrictions
    {
        None,
        ForceLower,
        CantAim
    }
    public class WeaponPositioning : MonoBehaviour
    {
        [SerializeField] private Transform shoulderPos;
        [SerializeField] private Transform aimingPos;
        [SerializeField] private Transform runningPos;

        [SerializeField] private float aimDownSideTime=0.2f;

        [SerializeField] private float shakingDegrees;
        [SerializeField] private float shakingSpeed;

        [SerializeField]private Transform weaponTransform;

        public Animator Animator { get; private set; }
        public float ADSTime => aimDownSideTime;

        public enum State
        {
            Aiming,
            Up,
            Down,
            Hidden,
            ChangingState
        }

        public State CurrentState { get; private set; } = State.Up;
        private State _targetState= State.Up;

        private Transform GetStateTransform(State state)
        {
            switch (state)
            {
                case State.Aiming:
                    return aimingPos;
                
                case State.Down:
                case State.Hidden:
                    return runningPos;
                
                case State.Up:
                    return shoulderPos;
            }

            return null;
        }

        private void OnEnable()
        {
            Animator = GetComponent<Animator>();
        }

        public void RotateWeapon(Transform cameraT)
        {
            var angle = aimingPos.localEulerAngles.x - cameraT.localEulerAngles.x;
            var cameraPosition = cameraT.position;
            aimingPos.RotateAround(cameraPosition,cameraT.right,-angle);
            shoulderPos.RotateAround(cameraPosition,cameraT.right,-angle);
        }
        
        public void PositionWeapon(State state, float requiredTIme = 0.2f)
        {
            if (_targetState == state) return;
            _targetState = state;

            if (_positioningWeaponCoroutine != null)
            {
                StopCoroutine(_positioningWeaponCoroutine);
            }

            if (_weaponShakingCoroutine != null)
            {
                StopCoroutine(_weaponShakingCoroutine);
            }
            
            _positioningWeaponCoroutine = StartCoroutine(PositioningWeapon(state, requiredTIme));
        }

        private Coroutine _positioningWeaponCoroutine;

        private IEnumerator PositioningWeapon(State state, float time)
        {
            if (CurrentState == State.Hidden)
            {
                weaponTransform.gameObject.SetActive(true);
            }

            CurrentState = State.ChangingState;

            weaponTransform.parent = GetStateTransform(state);
            var startingPos = weaponTransform.localPosition;
            var startingRot = weaponTransform.localRotation;

            var t = 0f;
            while (t <= time)
            {
                t += Time.deltaTime;
                weaponTransform.localPosition = Vector3.Lerp(startingPos, Vector3.zero, t / time);
                weaponTransform.localRotation = Quaternion.Lerp(startingRot, Quaternion.identity, t / time);
                yield return null;
            }

            CurrentState = state;
            if (CurrentState == State.Hidden)
            {
                weaponTransform.gameObject.SetActive(false);
            }
            else
            {
                StartShakingWeapon();
            }
        }

        
        private Coroutine _weaponShakingCoroutine;
        private Coroutine _weaponShakeToCertainRotationCoroutine;

        private void StartShakingWeapon()
        {
            // BUG doesnt work
            var shakingDegreeMult = 1f;
            if (CurrentState == State.Aiming) shakingDegreeMult = 0.15f;
            if (CurrentState == State.Down) shakingDegreeMult = 3f;
            _weaponShakingCoroutine =
                StartCoroutine(WeaponShaking(shakingDegrees * shakingDegreeMult, shakingSpeed * shakingDegreeMult));
        }

        private IEnumerator WeaponShaking(float deltaDegrees, float speed)
        {
            while (CurrentState != State.ChangingState)
            {
                float x = Random.Range(-deltaDegrees, deltaDegrees);
                float y = Random.Range(-deltaDegrees, deltaDegrees);
                yield return StartCoroutine(RotateWeaponRelativeToStartingRot(x, y, speed));
            }
        }
        
        private IEnumerator RotateWeaponRelativeToStartingRot(float x,float y, float speed)
        {
            Quaternion xRot = Quaternion.AngleAxis(x , Vector3.right);
            Quaternion yRot = Quaternion.AngleAxis(y , Vector3.up);
            Quaternion targetRotation = Quaternion.identity * xRot * yRot;

            while (weaponTransform.localRotation != targetRotation && CurrentState!= State.ChangingState)
            {
                weaponTransform.localRotation = Quaternion.RotateTowards(weaponTransform.localRotation, targetRotation,
                    Time.deltaTime * speed);
                yield return null;
            }
        }
    }
}