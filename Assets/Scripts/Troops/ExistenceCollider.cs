using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Troops;

public class ExistenceCollider : MonoBehaviour
{
    private Troop _troop;

    private void Awake()
    {
        _troop = transform.parent.GetComponent<Troop>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("ExistenceCollider")) { return; }

        if (collision.transform.parent.TryGetComponent<Troop>(out Troop troop))
        {
            if (troop.FriendOrFoe != _troop.FriendOrFoe)
            {
                return;
            }

            if (troop.TroopState != TroopState.Moving)
            {
                return;
            }

            bool movingInSameDirection = troop.MoveDirection == _troop.MoveDirection;
            if (movingInSameDirection)
            {
                _troop.SetCurrentMoveSpeed(troop.CurrentMoveSpeed);
            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("ExistenceCollider")) { return; }

        if (collision.transform.parent.TryGetComponent<Troop>(out Troop troop))
        {
            if (troop.FriendOrFoe != _troop.FriendOrFoe)
            {
                return;
            }

            if (troop.TroopState != TroopState.Moving)
            {
                return;
            }


            bool movingInSameDirection = troop.MoveDirection == _troop.MoveDirection;
            if (movingInSameDirection)
            {
                _troop.SetCurrentMoveSpeed(_troop.DefaultMoveSpeed);
            }

        }
    }
}
