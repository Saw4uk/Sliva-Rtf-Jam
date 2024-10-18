using System;
using SlivaRtfJam.Scripts.Ui;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Healthable : MonoBehaviour
    {
        [SerializeField] private float maxHp;
        [SerializeField] private HpBar hpBar;
        private float hp;
        public UnityEvent<float, float> OnChangeHp;
        public UnityEvent OnDie;
        public float Hp
        {
            get => hp;
            set
            {
                hp = value;
                OnChangeHp?.Invoke(hp, maxHp);

                if (hp <= 0)
                {
                    OnDie?.Invoke();
                }
                if (hpBar != null)
                {
                    hpBar.DrawProgress(hp/maxHp);
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