using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera cameraTop;
    [SerializeField] private CinemachineVirtualCamera cameraBottom;
    [SerializeField] public ExpBar playerExpBar;

    [SerializeField] public Timer playerTimer;

    [SerializeField] private float playerHeight = 3.5f;
    [SerializeField] public float rotationSpeed = 100;

    [SerializeField] public float moveSpeed = 40;
    [SerializeField] private int speedToAdd;


    private Rigidbody rb;
    private Vector3 moveDir;
    private bool breaking = true;
    private float curSpeed;


    private void Start()
    {
        curSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
        activateNormalView();
    }


    void Update()
    {
        curSpeed = (moveSpeed != 0) ? moveSpeed : curSpeed;
        if (Input.GetKey(KeyCode.Space) && !PauseMenu.isPaused)
        {
            activateTopView();
        }
        else
        {
            activateNormalView();
        }

        if (playerExpBar.isFinished())
        {
            ZoomOut();
            playerTimer.stopTimer();
            gameManager.NextLevelScreen();
        }

        if (playerTimer.isFinished())
        {
            ZoomOut();
            gameManager.LoseScreen();
        }

        //todo remove at end - hack for fast explosion and move to next level
        if (Input.GetKeyDown(KeyCode.E))
        {
            ZoomOut();
            playerTimer.stopTimer();
            gameManager.NextLevelScreen();
        }
    }


    private void activateNormalView()
    {
        Time.timeScale = 1;
        playerTimer.scaleTimeNormal();
        moveSpeed = curSpeed;
        mainCamera.orthographic = false;
        cameraBottom.Priority = 10;
        cameraTop.Priority = 0;
    }


    private void activateTopView()
    {
        Time.timeScale = 0.1f;
        playerTimer.scaleTimeUp();
        moveSpeed = 0;
        mainCamera.orthographic = true;
        cameraBottom.Priority = 0;
        cameraTop.Priority = 10;
    }


    private void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");

        Vector3 m_EulerAngleVelocity = new Vector3(0, input * rotationSpeed, 0);
        moveDir = new Vector3(input, 0, 1).normalized;

        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        rb.velocity = transform.TransformDirection(moveDir) * moveSpeed;
        // rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (breaking)
        {
            Transform otherTransform = other.transform;
            GameObject otherGameObject = other.gameObject;

            switch (otherTransform.tag)
            {
                case "Plane":
                    break;

                case "Stop":
                    stopHandler(otherGameObject);
                    break;

                case "Speed":
                    speedHandler(otherGameObject);
                    break;

                case "Time":
                    timeHandler(otherGameObject);
                    break;

                case "Bomb":
                    bombHandler(otherGameObject);
                    break;

                case "Collapse":
                    collapseHandler(otherTransform);
                    break;

                case "Collapsed":
                    break;

                default:
                    defaultHandler(otherGameObject);
                    break;
            }
        }
    }


    private void defaultHandler(GameObject otherGameObject)
    {
        if (otherGameObject.GetComponent<Rigidbody>() == null)
        {
            gameManager.AddRigid(otherGameObject);
            // otherGameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f),
            //     Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
        }
    }

    private void collapseHandler(Transform otherTransform)
    {
        ObjectSize objectSize = otherTransform.GetComponent<ObjectSize>();
        if (objectSize.toShake()) ShakePlayer();
        playerExpBar.addExp(objectSize.GetSize());
        if (otherTransform.GetComponent<OnlyFatherFlag>() == null)
        {
            gameManager.AddRigidChildren(otherTransform.parent.parent);
        }
        else
        {
            gameManager.AddRigidChildren(otherTransform.parent);
        }
        gameManager.PlaySound(SoundManager.Sounds.OBJECT_COLLAPSE);
    }


    private void timeHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.GOOD_PICKUP);
        playerTimer.addTime();
        gameManager.ManageScreen(ScreenEffectsManager.Effects.TIME);
    }

    private void bombHandler(GameObject other)
    {
        playerExpBar.addExp(10);
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.BOMB_EXP);
    }

    private void speedHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.GOOD_PICKUP);
        StartCoroutine(addSpeed());
    }


    private void stopHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.BAD_PICKUP);
        StartCoroutine(stopBreaking());
    }


    IEnumerator stopBreaking()
    {
        gameManager.ManageScreen(ScreenEffectsManager.Effects.STOP);
        breaking = false;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(gameManager.powerUpsTime);

        breaking = true;
        rb.constraints |= RigidbodyConstraints.FreezePositionY;
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, playerHeight, pos.z);
    }


    IEnumerator addSpeed()
    {
        gameManager.ManageScreen(ScreenEffectsManager.Effects.SPEED);
        moveSpeed += speedToAdd;

        yield return new WaitForSeconds(gameManager.powerUpsTime);

        moveSpeed -= speedToAdd;
    }


    /* shakes the player a bit on object hit #1# */
    private void ShakePlayer()
    {
        Vector3 localEulerAngles = transform.localEulerAngles;
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(transform.DOShakeRotation(0.2f, 10f, 10, 10))
            .Insert(0, transform.DOShakePosition(0.1f))
            .Append(transform.DORotate(new Vector3(0, localEulerAngles.y, 0), 0f));
    }


    /**
     * shit to do before moving to next level:
     * rotate, scale, explosion particle...
     */
    public void ZoomOut()
    {
        breaking = false;

        // explode
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(transform.DOScale(new Vector3(50, 20, 15), 1.5f))
            .Insert(0, transform.DOMove(new Vector3(5f, 3.5f, 120f), 3f))
            .Insert(0, transform.DORotate(new Vector3(15, 360, 10), 50).SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear).SetRelative());

        // gameManager.PlaySound(SoundManager.Sounds.BIG_EXP);
        // StartCoroutine(MoveScene());
    }


    /*
     * moves to next scene after 10 secs - so we'll see the explosion
     #1#
    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(10);
        gameManager.NextLevel();
    }*/
}