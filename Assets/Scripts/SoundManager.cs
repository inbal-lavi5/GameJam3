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

    [SerializeField] private AudioClip BOMB_EXP;
    [SerializeField] private AudioClip BIG_EXP;
    [SerializeField] private AudioClip OBJECT_COLLAPSE;
    [SerializeField] private AudioClip BOMB_PICKUP;
    [SerializeField] private AudioClip MANA_PICKUP;

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
                audioSrc.PlayOneShot(BOMB_EXP);
                break;
            case Sounds.BIG_EXP:
                audioSrc.PlayOneShot(BIG_EXP);
                break;
            case Sounds.OBJECT_COLLAPSE:
                audioSrc.PlayOneShot(OBJECT_COLLAPSE);
                break;
            case Sounds.BOMB_PICKUP:
                audioSrc.PlayOneShot(BOMB_PICKUP);
                break;
            case Sounds.MANA_PICKUP:
                audioSrc.PlayOneShot(MANA_PICKUP);
                break;
        }
    }
}