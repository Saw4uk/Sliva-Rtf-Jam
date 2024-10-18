using System;
using SlivaRtfJam.Scripts.Model;
using TMPro;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Ui
{
    public class EngineerDraw : MonoBehaviour
    {
        [SerializeField] private PlayerModel EngineerModel;
        [SerializeField] private EngineerBuildSystem engineerBuildSystem;
        [SerializeField] private TMP_Text healsAmount;
        [SerializeField] private TMP_Text searchlightAmount;
        private void Awake()
        {
            EngineerModel.parametersChanged += EngineerModelOnparametersChanged;
            engineerBuildSystem.OnSearchlightCountChanged += OnSearchlightCountChanged;
            EngineerModelOnparametersChanged();
            OnSearchlightCountChanged(engineerBuildSystem.SearchlightCount);
        }

        private void EngineerModelOnparametersChanged()
        {
            healsAmount.text = EngineerModel.HealsAmount.ToString();
        }

        private void OnSearchlightCountChanged(int searchlightCount)
        {
            searchlightAmount.text = searchlightCount.ToString();
        }
    }
}