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
    [SerializeField] public float moveSpeed = 40;
    [SerializeField] public float rotationSpeed = 100;
    [SerializeField] public ManaBar playerExpBar;
    [SerializeField] private Camera cameraTop;
    [SerializeField] private Camera cameraBottom;
    

    private Rigidbody rb;
    private Vector3 moveDir;
    private bool breaking = true;
    private Transform ball;
    private float curSpeed;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ball = transform.GetChild(0);
        moveDir = new Vector3(0, 0f, 1f).normalized;
    }

    
    void Update()
    {
        curSpeed = (moveSpeed != 0) ? moveSpeed : curSpeed; 

        if (Input.GetKey(KeyCode.S))
        {
            Time.timeScale = 0.1f;
            moveSpeed = 0;
            cameraBottom.enabled = false;
            cameraTop.enabled = true;
        }

        else
        {
            Time.timeScale = 1;
            moveSpeed = curSpeed;
            cameraBottom.enabled = true;
            cameraTop.enabled = false;
        }
        
        //todo remove at end - hack for fast explosion and move to next level
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLevel();
        }
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
        Transform otherTransform = other.transform;

        if (breaking)
        {
            switch (otherTransform.tag)
            {
                case "Plane":
                    break;
                
                case "PickUp1":
                    other.gameObject.SetActive(false);
                    gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);
                    StartCoroutine(stopBreaking());
                    break;

                case "PickUpMana":
                    moveSpeed += 15;
                    // playerManaBar.addManaBeMaca();
                    other.gameObject.GetComponent<BlueParticle>().Detonate();
                    gameManager.PlaySound(SoundManager.Sounds.MANA_PICKUP);
                    break;
            
                case "PickUp":
                    
                    ball.DOScale(new Vector3(ball.localScale.x+2, 4, ball.localScale.z+2), 1.5f);
                    // GetComponent<Camera>().changeCameraPosition();
                    other.gameObject.GetComponent<Explosion>().Dest();
                    gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);
                    break;
            
                case "Collapse":
                    // add rigid body to all children
                    playerExpBar.addMana();
                    Transform parentParent = otherTransform.parent.parent;
                    parentParent.tag = "Collapsed";
                    AddRigidChildren(parentParent);
                    ShakePlayer();
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
    }

    
    /**
     * shakes the player a bit on object hit
     */
    protected void ShakePlayer()
    {
        Vector3 localEulerAngles = transform.localEulerAngles;
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(transform.DOShakePosition(0.1f))
            .Append(transform.DOShakeRotation(0.2f, 10f, 10, 10))
            .Append(transform.DORotate(new Vector3(0, localEulerAngles.y, 0), 0f));
    }
    
    
    /**
     * adds rigidbody to all children
     */
    protected void AddRigidChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.GetComponent<Rigidbody>() == null)
            {
                child.gameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f),
                    Random.Range(0f, 0.5f));
                child.tag = "Collapsed";
            }

            if (child.childCount > 0)
            {
                AddRigidChildren(child);
            }
        }
    }

    
    /**
     * shit to do before moving to next level:
     * rotate, scale, explosion particle...
     */
    public void NextLevel()
    {
        breaking = false;
     
        // explode
        Destroy(rb);
        transform.DOScale(new Vector3(50, 20, 15), 1.5f);
        transform.DOLocalRotate(new Vector3(15, 270, 10), 15).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();
        // camera.cameraUp();
        // gameManager.PlaySound(SoundManager.Sounds.BIG_EXP);
        StartCoroutine(MoveScene());
    }

    
    /**
     * moves to next scene after 10 secs - so we'll see the explosion
     */
    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(10);
        gameManager.NextLevel();
    }
    
    IEnumerator stopBreaking()
    {
        breaking = false;
        yield return new WaitForSeconds(5);
        breaking = true;
    }
}