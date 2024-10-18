using System;
using SlivaRtfJam.Scripts.Guns;
using SlivaRtfJam.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

namespace SlivaRtfJam.Scripts.Shop
{
    public class GunShop : MonoBehaviour
    {
        [SerializeField] private ShopTrigger shopTrigger;
        [SerializeField] private int costInMoney;
        [SerializeField] private GunType unblockedGunType;
        
        public UnityEvent showOnBuy;
        public UnityEvent hideOnBuy;
        private void Awake()
        {
            shopTrigger.UseShopEvent += ShopTriggerOnUseShopEvent;
            shopTrigger.showOnBuy.AddListener(InvokeShowOnBuy);
            shopTrigger.hideOnBuy.AddListener(InvokeHideOnBuy);
        }

        private void InvokeShowOnBuy(GameObject playerGameObject)
        {
            var shooting = playerGameObject.GetComponent<SoldierShooting>();
            if (shooting != null && !shooting.CanHaveGun(unblockedGunType))
            {
                showOnBuy.Invoke();
            }
        }
        
        private void InvokeHideOnBuy(GameObject playerGameObject)
        {
            hideOnBuy.Invoke();
        }

        private void ShopTriggerOnUseShopEvent(GameObject obj)
        {
            var shooting = obj.GetComponent<SoldierShooting>();
            if (shooting != null && !shooting.CanHaveGun(unblockedGunType) && GameEconomy.Instance.Money >= costInMoney)
            {
                GameEconomy.Instance.Money -= costInMoney;
                shooting.UnblockGun(unblockedGunType);
                InvokeHideOnBuy(null);
            }
        }
    }
}