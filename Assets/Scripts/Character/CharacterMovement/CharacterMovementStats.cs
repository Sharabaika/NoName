using UnityEngine;

namespace Player
{
    [CreateAssetMenu]
    public class CharacterMovementStats : ScriptableObject
    {
        public float WalkSpeed = 7f;
        public float RunSpeed = 15f;
        public float JumpSpeed = 7f;
        public float FallingControllability = 2f;
    }
}