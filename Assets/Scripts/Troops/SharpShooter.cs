using UnityEngine;

namespace Troops.TroopClasses
{
    public class SharpShooter : Troop
    {
        public override void Attack()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveDirection, attackDistance);

            if (hit.collider)
            {
                var damageAction = new TakeDamageAction
                {
                    DamageAmount = AttackDamage,
                    DamagedByTroop = TroopType,
                };

                hit.collider.GetComponent<IDamageable>().TakeDamage(damageAction);
            }
        }
    }
}