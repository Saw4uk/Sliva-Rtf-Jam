using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;

public class GunPanel : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<GunType, GunInfoDrawer> gunInfoDrawers;
    [SerializeField] private SoldierShooting soldierShooting;

    private void Awake()
    {
        soldierShooting.GunParametersChanged.AddListener(ReDraw);
        ReDraw();
    }

    private void ReDraw()
    {
        foreach (var pair in gunInfoDrawers)
        {
            var drawer = pair.Value;
            var type = pair.Key;
            drawer.Draw(soldierShooting.CanHaveGun(type), soldierShooting.ChoosenGunType == type);
        }
    }
}
