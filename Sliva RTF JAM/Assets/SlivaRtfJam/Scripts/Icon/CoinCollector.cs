using System;
using System.Collections;
using System.Collections.Generic;
using SlivaRtfJam.Scripts.Model;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Coin coin))
        {
            GameEconomy.Instance.Money += coin.PickUp();
        }
    }
}