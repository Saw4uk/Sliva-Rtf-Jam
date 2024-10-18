﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Healthable : MonoBehaviour
    {
        [SerializeField] private float maxHp;
        private float hp;
        public UnityEvent<float, float> OnChangeHp;
        public UnityEvent OnDie;
        public float Hp
        {
            get => hp;
            set
            {
                hp = Math.Min(value, maxHp);
                OnChangeHp?.Invoke(hp, maxHp);
                if (hp <= 0)
                {
                    OnDie?.Invoke();
                }
                // Debug.Log(hp);
            }
        }

        private void Start()
        {
            hp = maxHp;
        }

        public void TakeDamage(float damage)
        {
            Hp -= damage;
        }
    }
}