using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] private Camera cameraTop;
    [SerializeField] private Camera cameraBottom;
    [SerializeField] public ExpBar playerExpBar;

    [SerializeField] public Timer playerTimer;
    // [SerializeField] public ScreenEffectsManager screenManager;

    [SerializeField] private float playerHeight = 3.5f;
    [SerializeField] public float rotationSpeed = 100;

    // [SerializeField] private int timeToRemovePart;
    // [SerializeField] private int powerUpsTime;

    [SerializeField] public float moveSpeed = 40;
    [SerializeField] private int speedToAdd;


    private Rigidbody rb;
    private Vector3 moveDir;
    private bool breaking = true;
    private float curSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
    }


    void Update()
    {
        curSpeed = (moveSpeed != 0) ? moveSpeed : curSpeed;

        if (Input.GetKey(KeyCode.S))
        {
            activateTopView();
        }

        else
        {
            activateNormalView();
        }

        //todo remove at end - hack for fast explosion and move to next level
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLevel();
        }
    }


    private void activateNormalView()
    {
        Time.timeScale = 1;
        playerTimer.scaleTimeNormal();
        moveSpeed = curSpeed;
        cameraBottom.enabled = true;
        cameraTop.enabled = false;
    }


    private void activateTopView()
    {
        Time.timeScale = 0.1f;
        playerTimer.scaleTimeUp();
        moveSpeed = 0;
        cameraBottom.enabled = false;
        cameraTop.enabled = true;
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
        playerExpBar.addExp();
        gameManager.AddRigidChildren(otherTransform.parent.parent);
        ShakePlayer();
        gameManager.PlaySound(SoundManager.Sounds.OBJECT_COLLAPSE);
    }


    private void timeHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);
        playerTimer.addTime();
        // screenManager.timedOn();
        gameManager.ManageScreen(ScreenEffectsManager.Effects.TIME);
    }

    private void bombHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);
    }

    private void speedHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        gameManager.PlaySound(SoundManager.Sounds.MANA_PICKUP);
        StartCoroutine(addSpeed());
    }


    private void stopHandler(GameObject other)
    {
        other.GetComponent<Particle>().Detonate();
        other.SetActive(false);
        gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);
        StartCoroutine(stopBreaking());
    }


    IEnumerator stopBreaking()
    {
        breaking = false;
        // screenManager.blockOn();
        gameManager.ManageScreen(ScreenEffectsManager.Effects.STOP);
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(gameManager.powerUpsTime);

        breaking = true;
        // screenManager.normal();
        gameManager.ManageScreen(ScreenEffectsManager.Effects.NORMAL);
        rb.constraints |= RigidbodyConstraints.FreezePositionY;
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, playerHeight, pos.z);
    }


    IEnumerator addSpeed()
    {
        moveSpeed += speedToAdd;
        // screenManager.speedOn();
        gameManager.ManageScreen(ScreenEffectsManager.Effects.SPEED);

        yield return new WaitForSeconds(gameManager.powerUpsTime);
        
        // screenManager.normal();
        gameManager.ManageScreen(ScreenEffectsManager.Effects.NORMAL);
        moveSpeed -= speedToAdd;
    }


    /**
     * shit to do before moving to next level:
     * rotate, scale, explosion particle...
     */
    public void NextLevel()
    {
        breaking = false;

        // explode
        transform.DOScale(new Vector3(50, 20, 15), 1.5f);
        transform.DOLocalRotate(new Vector3(15, 270, 10), 15).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear)
            .SetRelative();
        // camera.cameraUp();
        // gameManager.PlaySound(SoundManager.Sounds.BIG_EXP);
        // StartCoroutine(MoveScene());
    }


    /* shakes the player a bit on object hit #1# */
    private void ShakePlayer()
    {
        Vector3 localEulerAngles = transform.localEulerAngles;
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(transform.DOShakePosition(0.1f))
            .Append(transform.DOShakeRotation(0.2f, 10f, 10, 10))
            .Append(transform.DORotate(new Vector3(0, localEulerAngles.y, 0), 0f));
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