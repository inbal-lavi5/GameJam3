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
        PAUSE,
        PAGAN,
        VILLAGE,
        CITY,
        WINNING,
        LOSING,
        OBJECT_COLLAPSE,
        GOOD_PICKUP,
        BAD_PICKUP,
        BOMB_EXP,
        PLAY,
        TIMER
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
    [SerializeField] private AudioClip TIMER;

    static AudioSource mainAudioSrc;
    static AudioSource pauseAudioSrc;
    static AudioSource loseAudioSrc;
    static AudioSource winAudioSrc;

    static bool on = true;
    private int last = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        mainAudioSrc = GetComponent<AudioSource>();
        pauseAudioSrc = transform.GetChild(0).GetComponent<AudioSource>();
        loseAudioSrc = transform.GetChild(1).GetComponent<AudioSource>();
        winAudioSrc = transform.GetChild(2).GetComponent<AudioSource>();

        mainAudioSrc.PlayOneShot(MENU);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(Sounds sfx)
    {

        switch (sfx)
        {
            case Sounds.MENU:
                mainAudioSrc.PlayOneShot(MENU);
                break;
            
            case Sounds.PAUSE:
                pauseAll();
                pauseAudioSrc.Play();
                break;
            
            case Sounds.PAGAN:
                last = 1;
                stopAll();
                mainAudioSrc.PlayOneShot(PAGAN);
                break;
          
            case Sounds.VILLAGE:
                last = 1;
                stopAll();
                mainAudioSrc.PlayOneShot(VILLAGE);
                break;
            
            case Sounds.CITY:
                last = 1;
                stopAll();
                mainAudioSrc.PlayOneShot(CITY);
                break;     

            case Sounds.WINNING:
                last = 2;
                mainAudioSrc.Stop();
                winAudioSrc.PlayOneShot(WINNING);
                break;  
            
            case Sounds.LOSING:
                last = 3;
                mainAudioSrc.Stop();
                loseAudioSrc.PlayOneShot(LOSING);
                break;
            
            case Sounds.OBJECT_COLLAPSE:
                int i = Random.Range(0, OBJECT_COLLAPSE.Length);
                mainAudioSrc.PlayOneShot(OBJECT_COLLAPSE[i], 0.1f);
                break;
            
            case Sounds.GOOD_PICKUP:
                mainAudioSrc.PlayOneShot(GOOD_PICKUP);
                break;
            
            case Sounds.BAD_PICKUP:
                mainAudioSrc.PlayOneShot(BAD_PICKUP);
                break;

            case Sounds.BOMB_EXP:
                mainAudioSrc.PlayOneShot(BOMB_EXP, 0.5f);
                break;
            
            case Sounds.PLAY:
                pauseAudioSrc.Stop();
                playLast();
                break;
            
            case Sounds.TIMER:
                mainAudioSrc.PlayOneShot(TIMER, 0.5f);
                break;
            
        }
    }

    private static void stopAll()
    {
        mainAudioSrc.Stop();
        winAudioSrc.Stop();
        loseAudioSrc.Stop();
    }

    private static void pauseAll()
    {
        mainAudioSrc.Pause();
        winAudioSrc.Pause();
        loseAudioSrc.Pause();
    }

    private void playLast()
    {
        switch (last)
        {
            case 1: // pagan, village, city
                mainAudioSrc.Play();
                break;    
            
            case 2: // winning
                winAudioSrc.Play();
                break; 
            
            case 3: // losing
                loseAudioSrc.Play();
                break; 
        }
    }
    
    
    public void OnOffAudio(GameObject offBottom, GameObject onBottom)
    {
        if (on)
        {
            on = false;
            mainAudioSrc.volume = 0;
            offBottom.SetActive(true);
            onBottom.SetActive(false);
        }
        
        else
        {
            on = true;
            mainAudioSrc.volume = 0.2f;
            offBottom.SetActive(false);
            onBottom.SetActive(true);
        }
    }
}