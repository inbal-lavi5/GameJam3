using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    protected bool playing = true;
    protected Rigidbody rb;
    public cameraManager camera;

    [SerializeField] protected float playerHeight = 3.5f;

    [SerializeField] protected float moveSpeed = 40;
    protected Vector3 moveDir;
    [SerializeField] protected float rotationSpeed = 100;

    [SerializeField] protected bool breaking = true;
    [SerializeField] protected ManaBar playerManaBar;
    [SerializeField] protected PickupCounter playerPickupCounter;

    protected int pickUpCollected = 0;
    [SerializeField] private GameObject explosion;

    [SerializeField] public timer timer;
    private Transform ball;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ball = transform.GetChild(0);
        moveDir = new Vector3(0, 0f, 1f).normalized;
        // playerPickupCounter.SetUpMax(gameManager.pickUpsToCollectTillExplosion);
    }

    void Update()
    {
        /*if (!playing) return;
        
        Vector3 position = transform.position;
        
        if (Input.GetKey(KeyCode.Space))
        {
            playerManaBar.dec = true;
            SetPlayerAsNOTBreaking();
        }
        
        else
        {
            playerManaBar.dec = false;
            SetPlayerAsBreaking(position);
        }

        //todo remove at end - hack for fast explosion and move to next level
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLevel();
        }*/
    }

    protected void ManaBarHandle(Vector3 position)
    {
        
        /*playerManaBar.dec = true;
        if (playerManaBar.isManaFinished())
        {
            SetPlayerAsBreaking(position);
        }
        else
        {
            SetPlayerAsNOTBreaking();
        }*/
    }

    /*protected void SetPlayerAsNOTBreaking()
    {
        breaking = false;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }

    protected void SetPlayerAsBreaking(Vector3 position)
    {
        breaking = true;
        transform.position = new Vector3(position.x, playerHeight, position.z);
        rb.constraints |= RigidbodyConstraints.FreezePositionY;
    }*/

    
    private void FixedUpdate()
    {
        if (!playing) return;
        
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

        if (breaking && playing)
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
                    if (playing)
                    {
                        gameManager.PlaySound(SoundManager.Sounds.MANA_PICKUP);
                    }
                    break;
            
                case "PickUp":
                
                    // pickUpCollected++;
                    // playerPickupCounter.AddPickup();
                    ball.DOScale(new Vector3(ball.localScale.x+2, 4, ball.localScale.z+2), 1.5f);
                    camera.changeCameraPosition();
                    other.gameObject.GetComponent<Explosion>().Dest();
                    gameManager.PlaySound(SoundManager.Sounds.BOMB_PICKUP);
                    break;
            
                case "Collapse":
                    // add rigid body to all children
                    playerManaBar.addMana();
                    Transform parentParent = otherTransform.parent.parent;
                    parentParent.tag = "Collapsed";
                    AddRigidChildren(parentParent);
                    CheckCollision(otherTransform);
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
     * check what we collided with and sends to the game manager for processing
     */
    protected void CheckCollision(Transform other)
    {
        var multiTag = other.gameObject.GetComponent<CustomTag>();
        if (multiTag != null)
        {
            bool destroyedItem = gameManager.AddDestroyedItem(multiTag, other.transform.position);
            if (!destroyedItem)
            {
                // timer.decTime();
                // todo think what should happen here
                // playerManaBar.decManaBeMaca();
            }
        }
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
        // gameManager.disableImage();
        breaking = true;
        playing = false;
        // Destroy(playerManaBar.transform.parent.gameObject);
        // Destroy(playerPickupCounter.gameObject);
        // Instantiate(explosion, transform.position, transform.rotation);

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