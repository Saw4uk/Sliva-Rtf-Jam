using System;
using TMPro;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Ui
{
    public class SoldierInfoDrawer : MonoBehaviour
    {
        [SerializeField] private SoldierShooting soldierShooting;
        [SerializeField] private TMP_Text ammoString;

        private void Awake()
        {
            soldierShooting.AmmoChanged.AddListener(OnAmmoChanged);
            OnAmmoChanged(soldierShooting.ChosedGun.CurrentAmmo, soldierShooting.ChosedGun.CurrentAmmoTotal);
        }

        private void OnAmmoChanged(int arg0, int arg1)
        {
            ammoString.text = $"{arg0}/{arg1}";
        }
    }
}