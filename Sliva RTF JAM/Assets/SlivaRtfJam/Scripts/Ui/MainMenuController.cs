using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlivaRtfJam.Scripts.Ui
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonPressed);
            exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnPlayButtonPressed()
        {
            SceneManager.LoadScene(0);
        }

        private void OnExitButtonPressed()
        {
            Application.Quit();
        }
    }
}