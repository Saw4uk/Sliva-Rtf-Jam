using System;
using SlivaRtfJam.Scripts.Ui;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Healthable : MonoBehaviour
    {
        [SerializeField] private float maxHp;
        [SerializeField] private HpBar HpBar;
        private float hp;
        public UnityEvent<float, float> OnChangeHp;
        public UnityEvent OnDie;
        public float Hp
        {
            get => hp;
            set
            {
                // hp = Math.Min(value, maxHp);

                hp = Math.Clamp(value, 0, maxHp);
                HpBar?.DrawProgress(hp / maxHp);
                OnChangeHp?.Invoke(hp, maxHp);
                if (hp <= 0)
                {
                    OnDie?.Invoke();
                }

                Debug.Log(gameObject.name + hp);
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

        public void RestoreHalfHP()
        {
            Hp = maxHp / 2;
        }
    }
}