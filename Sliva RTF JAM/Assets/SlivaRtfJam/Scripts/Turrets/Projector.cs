using System;
using DefaultNamespace;
using SlivaRtfJam.Scripts.Guns;
using UnityEngine;
using UnityEngine.Serialization;

namespace SlivaRtfJam.Scripts
{
    public class Projector : MonoBehaviour
    {
        [SerializeField] private Transform rotator;
        [SerializeField] private float rotationSpeed = 10;
        private bool isActive;
        public bool IsActive => isActive;

        public void Activate()
        {
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;
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
        }
    }
}