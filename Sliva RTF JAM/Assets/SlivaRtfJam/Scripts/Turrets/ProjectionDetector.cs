using System;
using System.Collections.Generic;
using System.Linq;
using SlivaRtfJam.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class ProjectionDetector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out GlitchedEnemy glitchedEnemy))
            {
                Debug.Log("Entered" + glitchedEnemy.name);

                glitchedEnemy.TurnIntoSomething();
            }
            // other.ExposeSelf
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                Debug.Log("Exit" + enemy.name);
                StartCoroutine(enemy.TurnIntoGlitch());
            }

            // other.HideSelf
        }
    }
}