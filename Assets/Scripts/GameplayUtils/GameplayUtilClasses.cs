using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayUtils.Classes
{
    public class HP
    {
        public float CurrentHP { get; private set; }
        public float MaxHP { get; private set; }

        public HP(float maxHP)
        {
            MaxHP = maxHP;
            CurrentHP = MaxHP;
        }

        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0)
            {
                CurrentHP = 0;
            }
        }

        public void Heal(float heal)
        {
            CurrentHP += heal;
            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }
        }

        public float GetHPPercentage()
        {
            return CurrentHP / MaxHP;
        }
    }
}
