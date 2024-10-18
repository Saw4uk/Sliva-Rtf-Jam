using System;
using SlivaRtfJam.Scripts.Model;
using TMPro;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Ui
{
    public class SoldierInfoDrawer : MonoBehaviour
    {
        [SerializeField] private SoldierShooting soldierShooting;
        [SerializeField] private PlayerModel soldierModel;
        [SerializeField] private TMP_Text ammoString;
        [SerializeField] private TMP_Text healsAmount;

        private void Awake()
        {
            soldierShooting.AmmoChanged.AddListener(OnAmmoChanged);
            OnAmmoChanged(soldierShooting.ChosedGun.CurrentAmmo, soldierShooting.ChosedGun.CurrentAmmoTotal);
            soldierModel.parametersChanged += SoldierModelOnparametersChanged;
            SoldierModelOnparametersChanged();
        }

        private void SoldierModelOnparametersChanged()
        {
            healsAmount.text = soldierModel.HealsAmount.ToString();
        }

        private void OnAmmoChanged(int arg0, int arg1)
        {
            ammoString.text = $"{arg0}/{arg1}";
        }
    }
}