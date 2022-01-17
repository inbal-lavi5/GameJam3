using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        OBJECT_COLLAPSE,
        BOMB_EXP,
        GOOD_PICKUP,
        BAD_PICKUP,
        BIG_EXP,
    }

    [SerializeField] private AudioClip OBJECT_COLLAPSE;
    [SerializeField] private AudioClip BOMB_EXP;
    [SerializeField] private AudioClip GOOD_PICKUP;
    [SerializeField] private AudioClip BAD_PICKUP;
    [SerializeField] private AudioClip BIG_EXP;

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
            case Sounds.GOOD_PICKUP:
                audioSrc.PlayOneShot(GOOD_PICKUP);
                break;
            case Sounds.BAD_PICKUP:
                audioSrc.PlayOneShot(BAD_PICKUP);
                break;
        }
    }
}