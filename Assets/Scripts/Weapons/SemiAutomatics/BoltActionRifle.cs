namespace Weapons.SemiAutomatics
{
    public class BoltActionRifle : Weapon
    {
        public override void PullMainTrigger()
        {
            if (CanShoot())
            {

                Shoot();
                // TODO dont know how to do it
                Positioning.Shoulder();
            }
        }
    }
}