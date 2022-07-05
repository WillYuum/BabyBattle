using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Troops;
using DG.Tweening;

public class BigEnemyCamp : MonoBehaviour, IGarrisonable
{
    private List<ITroopBuildingInteraction> _garrisonedTroops;
    public EntityDirection AttackDirection { get; private set; }

    private void Awake()
    {
        _garrisonedTroops = new List<ITroopBuildingInteraction>();

        //FIXME: Should find a better way to get the MainCamp position
        Vector3 mainCampPos = GameObject.Find("MainCamp").transform.position;
        Vector2 directionToMainCamp = mainCampPos - transform.position;
        AttackDirection = directionToMainCamp.x > 0 ? EntityDirection.Right : EntityDirection.Left;
    }

    public void GarrisonTroops(Troop troop)
    {
        AddTroop(troop);
    }

    public void UnGarrisonTroops()
    {
        AttackTroops();
    }


    private void AttackTroops()
    {
#if UNITY_EDITOR
        if (_garrisonedTroops.Count == 0)
        {
            Debug.Log("No troops to ungarrison and attack");
            return;
        }
#endif

        Sequence sequence = DOTween.Sequence();

        foreach (var troop in _garrisonedTroops)
        {
            if (troop != null)
            {
                sequence.AppendCallback(() => troop.MoveOutOfBuilding(AttackDirection));
                sequence.AppendInterval(0.5f);
            }
        }

        sequence.Play();
        _garrisonedTroops.Clear();
    }

    private void AddTroop(ITroopBuildingInteraction troop)
    {
        _garrisonedTroops.Add(troop);
    }
}
