using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlivaRtfJam.Scripts.Ui
{
    public class WinLoseSceneController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonPressed);
            exitButton.onClick.AddListener(OnExitButtonPressed);
            mainMenuButton.onClick.AddListener(OnMenuButtonPressed);
        }

        private void OnPlayButtonPressed()
        {
            SceneManager.LoadScene(0);
        }
        private void OnMenuButtonPressed()
        {
            SceneManager.LoadScene(1);
        }

        private void OnExitButtonPressed()
        {
            Application.Quit();
        }
    }
}