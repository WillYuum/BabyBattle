using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { SmallBulding, LargeBuilding };

public class Building : MonoBehaviour
{
    private float _currentHealth;

    private List<Troop> _troops = new List<Troop>();

    public void TakeDamage(BuildingTakeDamageAction damageAction)
    {
        _currentHealth -= damageAction.damageAmountOnBuilding;


        if (_currentHealth <= 0)
        {
            MakeTroopsLeaveBuilding(damageAction);
            DestroyBuilding();
        }
    }

    public virtual void DestroyBuilding()
    {
        Destroy(gameObject, 2.0f);
    }

    private void MakeTroopsLeaveBuilding(BuildingTakeDamageAction damageAction)
    {
        var troopTakeDamageAction = new TroopTakeDamageAction
        {
            damageAmount = damageAction.damageAmountOnTroop,
            damagedByTroop = damageAction.damageByTroop,
        };

        foreach (Troop troop in _troops)
        {
            troop.TakeDamage(troopTakeDamageAction);
        }
    }
}





public struct BuildingTakeDamageAction
{
    public float damageAmountOnBuilding;
    public float damageAmountOnTroop;
    public TroopType damageByTroop;
}