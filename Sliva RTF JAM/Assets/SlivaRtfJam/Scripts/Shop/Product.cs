using System;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Shop
{
    [Serializable]
    public class Product<T>
    {
        [SerializeField] private T productPrefab;
        [SerializeField] private bool isUnlimited;
        [SerializeField] private int count;
        [SerializeField] private int cost;

        public T ProductPrefab => productPrefab;
        public bool IsUnlimited => isUnlimited;
        public int Count => count;
        public int Cost => cost;
    }
}