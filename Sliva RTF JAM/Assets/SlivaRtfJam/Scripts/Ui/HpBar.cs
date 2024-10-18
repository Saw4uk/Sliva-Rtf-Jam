using System;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Ui
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private float firstScale;
        
        private void Awake()
        {
            firstScale = spriteRenderer.gameObject.transform.localScale.x;
        }

        public void DrawProgress(float progress)
        {
            var localScale = spriteRenderer.gameObject.transform.localScale;
            spriteRenderer.gameObject.transform.localScale = new Vector3(firstScale * progress, localScale.y, localScale.z);
        }
    }
}