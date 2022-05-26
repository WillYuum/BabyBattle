using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Troops.TroopClasses
{
    public class BigBaby : Troop
    {
        public override void Attack()
        {
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveDirection, attackDistance);
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, MoveDirection, attackDistance + 0.5f);

            if (hit.Length > 0)
            {
                foreach (RaycastHit2D h in hit)
                {
                    if (h.collider.GetComponent<IDamageable>() != null)
                    {
                        var damageAction = new TakeDamageAction
                        {
                            DamageAmount = AttackDamage,
                            DamagedByTroop = TroopType,
                        };

                        h.collider.GetComponent<IDamageable>().TakeDamage(damageAction);
                    }
                }
            }
        }
    }
}