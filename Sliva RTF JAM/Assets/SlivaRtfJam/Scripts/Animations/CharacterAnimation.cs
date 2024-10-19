using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private List<AudioClip> wallkSfxs;

    public void WALLK_ANIMATION()
    {
        SfxManager.Instance.PlayOneShot(wallkSfxs);
    }
}
