using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SlivaRtfJam.Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private TMP_Text useText;

        private int playersInZone;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerMovement>().OnUseShop += ShowShopWindow;
                useText.gameObject.SetActive(true);
                playersInZone++;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerMovement>().OnUseShop -= ShowShopWindow;
                useText.gameObject.SetActive(false);
                playersInZone--;
            }
        }

        private void ShowShopWindow()
        {
            Debug.Log("Show shop window");
        }
    }
}