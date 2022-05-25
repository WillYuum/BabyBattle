using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Troops;

public class SharpShooter : Troop
{
    public override void Attack(IDamageable targets = null, IDamageable[] multipleTargets = null)
    {
            #if UNITY_EDITOR
        if(targets == null){
            Debug.LogError("Targets is null, make sure you are passing in a target");
            return; 
        }
#endif

            var damageAction = new TakeDamageAction
            {
                DamageAmount = AttackDamage,
                DamagedByTroop = _troopType,
            };

            targets.TakeDamage(damageAction);
            
    }
}
