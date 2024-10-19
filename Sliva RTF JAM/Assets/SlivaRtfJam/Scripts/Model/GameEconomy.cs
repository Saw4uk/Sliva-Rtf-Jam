using System;
using UnityEngine;
using UnityEngine.Events;

namespace SlivaRtfJam.Scripts.Model
{
    public class GameEconomy : MonoBehaviour
    {
        private static GameEconomy instance;

        public static GameEconomy Instance => instance;

        private int money;

        public int Money
        {
            get => money;
            set
            {
                money = value;
                moneyChanged.Invoke(money);
            }
        }

        public UnityEvent<int> moneyChanged;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                throw new Exception("Singleton two");
            }

            Money = 250;
        }
    }
}