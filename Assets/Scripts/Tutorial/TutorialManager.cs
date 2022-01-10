using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TutorialManager : GameManager
{
    private int randomGoal = 0;
    private int stage = 0;
    [SerializeField] private List<GameObject> tutorialList;

    void Awake()
    {
        // InstantiateMana();
        ActivateTut();
    }

    protected override void NextItemsToDestroy()
    {
        randomGoal = randomGoal >= images.Count - 1 ? 0 : randomGoal + 1;
        objectToDestroy.sprite = images[randomGoal];
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1)) //todo remove at end
        // {
        //     NextTut();
        // }

        if (Input.GetKeyDown(KeyCode.Space)) //todo remove at end
        {
            DeactivateTut();
        }
    }


    void ActivateTut()
    {
        for (var i = 0; i < tutorialList.Count; i++)
        {
            if (i == stage)
            {
                tutorialList[i].SetActive(true);
            }
            else
            {
                tutorialList[i].SetActive(false);
            }
        }

        Time.timeScale = 0f;
    }

    void DeactivateTut()
    {
        for (var i = 0; i < tutorialList.Count; i++)
        {
            tutorialList[i].SetActive(false);
        }

        Time.timeScale = 1f;
    }

    // protected new void NextItemsToDestroy()
    // {
    //     randomGoal++;
    //     objectToDestroy.sprite = images[randomGoal];
    // }

    public void NextStage()
    {
        stage++;
        ActivateTut();
        print("HERE " + stage);
    }

    // if bomb is detonated pass to next stage - put bombs
    // if bomb is collected pass to next stage - put bombs
    // if mana and bomb collected pass to next - put bombs and flowers
}