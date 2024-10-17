using System.Collections;
using System.Collections.Generic;
using SlivaRtfJam.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class GunInfoDrawer : MonoBehaviour
{
    [SerializeField] private GameObject onActive;
    [SerializeField] private GameObject onInactive;
    [SerializeField] private Image isSelected;

    public void Draw(bool isActive, bool isSelected)
    {
        onActive.gameObject.SetActive(isActive);
        onInactive.gameObject.SetActive(!isActive);
        this.isSelected.enabled = isSelected;
    }
}
