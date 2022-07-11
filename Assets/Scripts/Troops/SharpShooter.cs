using UnityEngine;

namespace Troops.TroopClasses
{
    public class SharpShooter : Troop
    {
        public override void Attack()
        {
            var layer = (1 << LayerMask.NameToLayer("Troop") | (1 << LayerMask.NameToLayer("Building")));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveDirection, attackDistance, layer);

            if (hit.collider)
            {
                var damageAction = new TakeDamageAction
                {
                    DamageAmount = AttackDamage,
                    DamagedByTroop = TroopType,
                };

                IDamageable damage = hit.collider.GetComponent<IDamageable>();
                damage.TakeDamage(damageAction);
            }
            else
            {
                ChangeState(TroopState.Moving);
            }
        }
    }
}