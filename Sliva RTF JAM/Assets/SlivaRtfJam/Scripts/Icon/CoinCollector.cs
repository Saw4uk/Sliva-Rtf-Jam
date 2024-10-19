using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using SlivaRtfJam.Scripts.Model;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private List<AudioClip> collectSfxs;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Coin coin))
        { 
            GameEconomy.Instance.Money += coin.PickUp();
            if(collectSfxs is { Count: > 0 })
                SfxManager.Instance.PlayOneShot(collectSfxs);
        }
    }
}