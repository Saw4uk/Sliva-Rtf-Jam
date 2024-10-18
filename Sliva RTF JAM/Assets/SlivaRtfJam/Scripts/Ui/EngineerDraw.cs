using System;
using SlivaRtfJam.Scripts.Model;
using TMPro;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Ui
{
    public class EngineerDraw : MonoBehaviour
    {
        [SerializeField] private PlayerModel EngineerModel;
        [SerializeField] private TMP_Text healsAmount;
        private void Awake()
        {
            EngineerModel.parametersChanged += EngineerModelOnparametersChanged;
            EngineerModelOnparametersChanged();
        }

        private void EngineerModelOnparametersChanged()
        {
            healsAmount.text = EngineerModel.HealsAmount.ToString();
        }
    }
}