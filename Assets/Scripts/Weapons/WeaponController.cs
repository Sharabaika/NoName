using UnityEngine;

namespace Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Transform weaponTransform;
        
        public void RotateWeapon(Quaternion rotation)
        {
            weaponTransform.rotation = rotation;
        }

        
        private void Update()
        {
            // Shooting
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                weapon.PullMainTrigger();
            }
            // || Input.GetKey(KeyCode.Mouse0)==false may remove bags
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                weapon.ReleaseMainTrigger();
            }
            
            
            // Weapon ability
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                weapon.PullSecondaryTrigger();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                weapon.ReleaseSecondaryTrigger();
            }

            
            // Reloading
            if (Input.GetKeyDown(KeyCode.R))
            {
                weapon.Reload();
            }
        }
    }
}