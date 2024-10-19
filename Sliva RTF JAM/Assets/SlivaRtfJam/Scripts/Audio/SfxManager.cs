using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Vector2 pitchRange;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public void PlayOneShot(List<AudioClip> clips)
    {
        PlayOneShot(clips.GetRandomElement());
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(clip);
        Debug.Log(clip.name);
    }
}