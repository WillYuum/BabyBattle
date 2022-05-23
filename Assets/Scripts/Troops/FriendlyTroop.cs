using System.Collections;
using System.Collections.Generic;
using Buildings;
using DG.Tweening;
using UnityEngine;


namespace Troops
{
    public class FriendlyTroop : Troop, ITroopBuildingInteraction
    {
        public bool TryAccessBuilding(BuildingCore buildingCore)
        {
            NotifyFollowers();
            return true;
        }

        public void MoveToIdlePositionInBuilding(Transform target)
        {
            float duration = Mathf.Abs(target.position.x - transform.position.x) / DefaultMoveSpeed;
            transform.DOMoveX(target.position.x, duration).OnComplete(() =>
            {
                ChangeState(TroopState.Idle);
            });
        }

        public void MoveOutOfBuilding(EntityDirection direction)
        {
            SetMoveDirection(direction);

            ChangeState(TroopState.Moving);
        }

    }
}