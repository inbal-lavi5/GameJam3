using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        MENU,
        PAGAN,
        VILLAGE,
        CITY,
        WINNING,
        LOSING,
        OBJECT_COLLAPSE,
        GOOD_PICKUP,
        BAD_PICKUP,
        BOMB_EXP,
    }
    [SerializeField] private AudioClip MENU;
    [SerializeField] private AudioClip PAGAN;
    [SerializeField] private AudioClip VILLAGE;
    [SerializeField] private AudioClip CITY;
    [SerializeField] private AudioClip WINNING;
    [SerializeField] private AudioClip LOSING;
    [SerializeField] private AudioClip[] OBJECT_COLLAPSE;
    [SerializeField] private AudioClip GOOD_PICKUP;
    [SerializeField] private AudioClip BAD_PICKUP;
    [SerializeField] private AudioClip BOMB_EXP;
    
    [SerializeField] private GameObject OnButton;
    [SerializeField] private GameObject OffButton;

    static AudioSource audioSrc;
    static bool on = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.PlayOneShot(MENU);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(Sounds sfx)
    {
        switch (sfx)
        {
            case Sounds.MENU:
                audioSrc.Stop();
                audioSrc.PlayOneShot(MENU);
                break;
            
            case Sounds.PAGAN:
                audioSrc.Stop();
                audioSrc.PlayOneShot(PAGAN);
                break;
          
            case Sounds.VILLAGE:
                audioSrc.Stop();
                audioSrc.PlayOneShot(VILLAGE);
                break;
            
            case Sounds.CITY:
                audioSrc.Stop();
                audioSrc.PlayOneShot(CITY);
                break;     

            case Sounds.WINNING:
                audioSrc.Stop();
                audioSrc.PlayOneShot(WINNING);
                break;  
            
            case Sounds.LOSING:
                audioSrc.Stop();
                audioSrc.PlayOneShot(LOSING);
                break;
            
            case Sounds.OBJECT_COLLAPSE:
                int i = Random.Range(0, OBJECT_COLLAPSE.Length);
                audioSrc.PlayOneShot(OBJECT_COLLAPSE[i]);
                break;
            
            case Sounds.GOOD_PICKUP:
                audioSrc.PlayOneShot(GOOD_PICKUP);
                break;
            
            case Sounds.BAD_PICKUP:
                audioSrc.PlayOneShot(BAD_PICKUP);
                break;

            case Sounds.BOMB_EXP:
                audioSrc.PlayOneShot(BOMB_EXP);
                break;
        }
    }

    public void OnOffAudio()
    {
        if (on)
        {
            on = false;
            audioSrc.volume = 0;
            OffButton.SetActive(true);
            OnButton.SetActive(false);
        }
        else
        {
            on = true;
            audioSrc.volume = 0.2f;
            OffButton.SetActive(false);
            OnButton.SetActive(true);
        }
    }
}