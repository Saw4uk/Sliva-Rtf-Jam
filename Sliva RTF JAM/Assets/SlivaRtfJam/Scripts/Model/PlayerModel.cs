using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Model
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private bool isSoldier;
        [SerializeField] private int healDelaySeconds;
        [SerializeField] private int healHP;
        private int healsAmount;
        private Healthable healthable;
        private PlayerMovement playerMovement;
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

        private void Awake()
        {
            healthable = GetComponent<Healthable>();
            if (healthable == null)
                throw new Exception();
        }

        public void Heal()
        {
            StartCoroutine(HealEnumerator());
        }

        private IEnumerator HealEnumerator()
        {
            playerMovement.IsBlocked = true;
            characterAnimator.SetTrigger("Heal");
            yield return new WaitForSeconds(healDelaySeconds);
            healthable.Hp += healHP;
            healsAmount -= 1;
            playerMovement.IsBlocked = false;
        }
    }
}