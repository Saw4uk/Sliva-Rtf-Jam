using System;
using SlivaRtfJam.Scripts.Guns;
using SlivaRtfJam.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

namespace SlivaRtfJam.Scripts.Shop
{
    public class SearchlightShop : MonoBehaviour
    {
        [SerializeField] private ShopTrigger shopTrigger;
        [SerializeField] private int costInMoney;
        
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
            var builder = playerGameObject.GetComponent<EngineerBuildSystem>();
            if (builder != null)
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
            var builder = obj.GetComponent<EngineerBuildSystem>();
            if (builder != null && GameEconomy.Instance.Money >= costInMoney)
            {
                GameEconomy.Instance.Money -= costInMoney;
                builder.AddSearchlight(1);
            }
        }
    }
}