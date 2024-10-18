using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace SlivaRtfJam.Scripts.Model
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private bool isSoldier;
        [SerializeField] private int healDelaySeconds;
        [SerializeField] private int healHP;
        private int healsAmount;
        [SerializeField] private Healthable healthable;
        [SerializeField] private PlayerMovement playerMovement;
        private bool isHealing;
        public int HealsAmount
        {
            get => healsAmount;
            set
            {
                healsAmount = value;
                parametersChanged?.Invoke();
            }
        }

        public bool IsSoldier => isSoldier;

        public event Action parametersChanged;
        public UnityEvent startHealing;
        public UnityEvent endHealing;

        private void Awake()
        {
            healthable = GetComponent<Healthable>();
            if (healthable == null)
                throw new Exception();
        }

        public void Heal()
        {
            if(!isHealing)
                StartCoroutine(HealEnumerator());
        }

        private IEnumerator HealEnumerator()
        {
            isHealing = true;
            playerMovement.IsBlocked = true;
            characterAnimator.SetTrigger("Heal");
            startHealing.Invoke();
            yield return new WaitForSeconds(healDelaySeconds);
            healthable.Hp += healHP;
            HealsAmount -= 1;
            playerMovement.IsBlocked = false;
            isHealing = false;
            endHealing.Invoke();
        }
    }
}