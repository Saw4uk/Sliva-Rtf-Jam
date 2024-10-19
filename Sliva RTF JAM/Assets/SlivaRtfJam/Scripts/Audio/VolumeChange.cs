using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Moonsters
{
    public class VolumeChange : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private AudioMixer audioGroup;
        [SerializeField] private VolumeGroup volumeGroup;
        [SerializeField][Range(0, 1)] private float startValue;

        private void Start()
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            audioGroup.SetFloat(volumeGroup.ToString(), FloatToMixer(startValue));
            audioGroup.GetFloat(volumeGroup.ToString(), out var volume);
            volumeSlider.value = MixerToFloat(volume);
        }

        public void OnVolumeChanged(float value)
        {
            audioGroup.SetFloat(volumeGroup.ToString(), FloatToMixer(value));
        }

        public static float MixerToFloat(float value)
        {
            return Mathf.Pow(10, value / 20);
        }

        public static float FloatToMixer(float value)
        {
            if(value == 0)
            {
                return -80;
            }

            return Mathf.Log10(value) * 20;
        }
    }

    public enum VolumeGroup
    {
        Sfx,
        Music
    }
}