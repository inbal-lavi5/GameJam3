using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        BOMB_EXP,
        BIG_EXP,
        OBJECT_COLLAPSE,
        BOMB_PICKUP,
        MANA_PICKUP
    }

    [SerializeField] private List<AudioClip> sounds;

    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlaySound(Sounds sfx)
    {
        switch (sfx)
        {
            case Sounds.BOMB_EXP:
                audioSrc.PlayOneShot(sounds[(int) sfx]);
                break;
            case Sounds.BIG_EXP:
                audioSrc.PlayOneShot(sounds[(int) sfx]);
                break;
            case Sounds.OBJECT_COLLAPSE:
                audioSrc.PlayOneShot(sounds[(int) sfx]);
                break;
            case Sounds.BOMB_PICKUP:
                audioSrc.PlayOneShot(sounds[(int) sfx]);
                break;
            case Sounds.MANA_PICKUP:
                audioSrc.PlayOneShot(sounds[(int) sfx]);
                break;
        }
    }
}