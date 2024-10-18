using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SlivaRtfJam.Scripts.Shop
{
    public class ShopTriggerEngeneer : MonoBehaviour
    {
        public UnityEvent<GameObject> showOnBuy;
        public UnityEvent<GameObject> hideOnBuy;
        public event Action<GameObject> UseShopEvent; 

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<EngineerBuildSystem>())
            {
                other.GetComponent<PlayerMovement>().OnUseShop += UseShop;
                showOnBuy.Invoke(other.gameObject);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<EngineerBuildSystem>())
            {
                other.GetComponent<PlayerMovement>().OnUseShop -= UseShop;
                hideOnBuy.Invoke(other.gameObject);
            }
        }

        private void UseShop(GameObject characterGameObject)
        {
            UseShopEvent?.Invoke(characterGameObject);
        }
    }
}