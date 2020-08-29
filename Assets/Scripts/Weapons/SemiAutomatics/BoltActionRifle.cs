namespace Weapons.SemiAutomatics
{
    public class BoltActionRifle : Weapon
    {
        protected override void OnPullMainTrigger()
        {
            if (CanShoot())
            {

                Shoot();
                // TODO dont know how to do it
                Positioning.PositionWeapon(WeaponPositioning.State.Shoulder);
            }
        }
    }
}