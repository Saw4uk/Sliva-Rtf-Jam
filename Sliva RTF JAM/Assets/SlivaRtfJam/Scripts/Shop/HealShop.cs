using SlivaRtfJam.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

namespace SlivaRtfJam.Scripts.Shop
{
    public class HealShop : MonoBehaviour
    {
        [SerializeField] private ShopTrigger shopTrigger;
        [SerializeField] private int costInMoney;

        public UnityEvent showOnBuySoldier;
        public UnityEvent hideOnBuy;
        public UnityEvent showOnBuyEngineer;
        private void Awake()
        {
            shopTrigger.UseShopEvent += ShopTriggerOnUseShopEvent;
            shopTrigger.showOnBuy.AddListener(InvokeShowOnBuy);
            shopTrigger.hideOnBuy.AddListener(InvokeHideOnBuy);
        }

        private void InvokeShowOnBuy(GameObject playerGameObject)
        {
            var playerModel = playerGameObject.GetComponent<PlayerModel>();
            if (playerModel == null) return;
            if(playerModel.IsSoldier)
                showOnBuySoldier.Invoke();
            else
                showOnBuyEngineer.Invoke();
        }
        
        private void InvokeHideOnBuy(GameObject playerGameObject)
        {
            hideOnBuy.Invoke();
        }

        private void ShopTriggerOnUseShopEvent(GameObject obj)
        {
            var playerModel = obj.GetComponent<PlayerModel>();
            if (playerModel != null && GameEconomy.Instance.Money >= costInMoney)
            {
                GameEconomy.Instance.Money -= costInMoney;
                playerModel.HealsAmount += 1;
                if(GameEconomy.Instance.Money < costInMoney)
                    InvokeHideOnBuy(null);
            }
        }
    }
}