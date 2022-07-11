using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayUtils.Methods
{
    public static class UtilMethods
    {
        public static bool CollidedWithPlayer(Collider2D other)
        {
            return other.CompareTag("Player");
        }

        public static bool CollidedWithTroop(Collider2D other)
        {
            return other.CompareTag("Troop");
        }
    }
}
