using System;
using SlivaRtfJam.Scripts.Guns;
using SlivaRtfJam.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

namespace SlivaRtfJam.Scripts.Shop
{
    public class ProjectorNew : MonoBehaviour
    {
        [SerializeField] private ShopTriggerEngeneer shopTrigger;

        [SerializeField] private Transform rotator;
        [SerializeField] private float rotationSpeed = 50;
        private bool isActive;
        private EngineerBuildSystem engeener;
        private Vector3 engeenerPosition;
        private bool activatedThisFrame;

        private void Awake()
        {
            shopTrigger.UseShopEvent += ShopTriggerOnUseShopEvent;
        }


        private void ShopTriggerOnUseShopEvent(GameObject obj)
        {
            var builder = obj.GetComponent<EngineerBuildSystem>();
            if (builder != null && !isActive)
            {
                isActive = true;
                engeener = builder;
                engeener.gameObject.SetActive(false);
                activatedThisFrame = true;
            }
        }


        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            if (Input.GetKey(KeyCode.A))
            {
                rotator.Rotate(transform.forward, rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rotator.Rotate(-transform.forward, rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (activatedThisFrame)
                {
                    activatedThisFrame = false;
                    return;
                }

                isActive = false;
                engeener.gameObject.SetActive(true);
            }
        }
    }
}