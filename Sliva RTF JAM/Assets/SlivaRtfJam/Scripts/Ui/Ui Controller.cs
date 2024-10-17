using System;
using System.Collections;
using System.Collections.Generic;
using SlivaRtfJam.Scripts.Model;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Awake()
    {
        GameEconomy.Instance.moneyChanged.AddListener(OnMoneyChanged);
        OnMoneyChanged(GameEconomy.Instance.Money);
    }

    private void OnMoneyChanged(int arg0)
    {
        moneyText.text = arg0.ToString();
    }
}
