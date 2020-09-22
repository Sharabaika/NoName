using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        // Gunplay
        public bool isFireKeyDown => Input.GetKeyDown(KeyCode.Mouse0);
        public bool isFireKey =>Input.GetKey(KeyCode.Mouse0);
        
        public bool isAimKeyDown => Input.GetKeyDown(KeyCode.Mouse1);
        public bool isAimKey => Input.GetKey(KeyCode.Mouse1);

        public bool isMainWeaponSlotKey => Input.GetKey(KeyCode.Alpha1);
        public bool isSecondaryWeaponSlotKey => Input.GetKeyDown(KeyCode.Alpha2);

        // Movement
        public float xInput =>Input.GetAxisRaw("Horizontal");
        public float zInput =>Input.GetAxisRaw("Vertical");
        public bool spaceInput =>Input.GetKey(KeyCode.Space);
        public bool shiftInput =>Input.GetKey(KeyCode.LeftShift);
        public Vector3 movementInput => new Vector3(xInput, 0f, zInput).normalized;
    }
}