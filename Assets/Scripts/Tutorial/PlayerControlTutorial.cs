using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerControlTutorial : PlayerControl
{
    // private int pickUpCollected = 0;
    private int pickUpExplode = 0;
    private TutorialManager tutorialManager;
    private int stage = 1;

    private void Start()
    {
        tutorialManager = gameManager.GetComponent<TutorialManager>();
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
        playerPickupCounter.SetUpMax(gameManager.pickUpsToCollectTillExplosion);
        playerManaBar.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!playing) return;
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.Space))
        {
            if (stage != 3)
            {
                SetPlayerAsNOTBreaking();
            }
            else
            {
                ManaBarHandle(position);
            }
        }
        else
        {
            playerManaBar.dec = false;
            SetPlayerAsBreaking(position);
        }

        if (Tut1Ended() && stage == 1)
        {
            tutorialManager.NextStage();
            stage++;
        }

        if (Tut2Ended() && stage == 2)
        {
            stage++;
        }

        if (Tut3Ended() && stage == 3)
        {
            stage++;
        }

        //todo remove at end - hack for fast explosion and move to next level
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLevel();
        }
    }


    bool Tut1Ended()
    {
        return pickUpExplode >= 3;
    }

    bool Tut2Ended()
    {
        return pickUpCollected >= 1;
    }

    bool Tut3Ended()
    {
        return false;
    }

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        Transform otherTransform = other.transform;

        if (otherTransform.CompareTag("PickUpMana"))
        {
            playerManaBar.addManaBeMaca();
            other.gameObject.GetComponent<BlueParticle>().Detonate();
            if (playing)
            {
                gameManager.PlaySound(SoundManager.Sounds.MANA_PICKUP);
            }
        }

        if (breaking)
        {
            switch (otherTransform.tag)
            {
                case "Plane":
                    break;
                case "PickUp":
                    other.gameObject.GetComponent<Explosion>().Detonate();
                    if (playing)
                    {
                        gameManager.PlaySound(SoundManager.Sounds.BOMB_EXP);
                        pickUpExplode++;
                    }

                    break;
                case "Collapse":
                    // add rigid body to all children
                    Transform parentParent = otherTransform.parent.parent;
                    StartCoroutine(Regenerate(parentParent));

                    // print(parentParent.name);
                    // Instantiate(parentParent.gameObject, parentParent.position, parentParent.rotation);

                    parentParent.tag = "Collapsed";
                    AddRigidChildren(parentParent);
                    CheckCollision(otherTransform);
                    if (playing)
                    {
                        ShakePlayer();
                    }

                    gameManager.PlaySound(SoundManager.Sounds.OBJECT_COLLAPSE);
                    break;
                case "Collapsed":
                    break;
                default:
                    GameObject otherGameObject = other.gameObject;
                    if (otherGameObject.GetComponent<Rigidbody>() == null)
                    {
                        otherGameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f),
                            Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
                    }

                    break;
            }
        }
        else if (playing)
        {
            if (otherTransform.CompareTag("PickUp"))
            {
                pickUpCollected++;
                playerPickupCounter.AddPickup();

                other.gameObject.GetComponent<Explosion>().Dest();
                gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);

                if (playerPickupCounter.CollectedAll())
                {
                    NextLevel();
                }
            }
        }
    }

    IEnumerator Regenerate(Transform parentParent)
    {
        GameObject instantiate = Instantiate(parentParent.gameObject, parentParent.position, parentParent.rotation,
            parentParent.parent);
        instantiate.SetActive(false);
        yield return new WaitForSeconds(3f);
        instantiate.SetActive(true);
    }
}