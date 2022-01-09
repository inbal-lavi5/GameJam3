using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private bool playing = true;

    [SerializeField] private float playerHeight = 3.5f;

    public float moveSpeed = 15;
    private Vector3 moveDir;
    private Rigidbody rb;
    [SerializeField] private float rotationSpeed = 100;

    [SerializeField] private bool breaking = true;
    [SerializeField] private ManaBar playerManaBar;
    [SerializeField] private PickupBar playerPickupBar;

    private int pickUpCollected = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
        // gameManager = GameManager.Instance;
        //playerPickupBar.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!playing) return;
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.Space))
        {
            playerManaBar.dec = true;
            if (playerManaBar.isManaFinished())
            {
                breaking = true;
                transform.position = new Vector3(position.x, 3.5f, position.z);
                rb.constraints |= RigidbodyConstraints.FreezePositionY;
            }
            else
            {
                breaking = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }

            // breaking = playerManaBar.isManaFinished();
        }
        else
        {
            playerManaBar.dec = false;
            breaking = true;
            transform.position = new Vector3(position.x, 3.5f, position.z);
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLevel();
        }
    }

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

        if (breaking)
        {
            if (!otherTransform.CompareTag("Plane"))
            {
                if (otherTransform.CompareTag("PickUpMana"))
                {
                    playerManaBar.addMana();
                    Destroy(other.gameObject);
                }
                else if (otherTransform.CompareTag("Collapse"))
                {
                    // add rigid body to all children
                    Transform parentParent = otherTransform.parent.parent;
                    parentParent.tag = "Collapsed";
                    AddRigidChildren(parentParent);
                    CheckCollision(otherTransform);
                    if (playing)
                    {
                        transform.DOShakePosition(0.1f);
                        transform.DOShakeRotation(0.2f, 10f, 10, 10, false);
                        // transform.DOShakeScale(0.3f);
                    }
                }
                else
                {
                    GameObject otherGameObject = other.gameObject;
                    if (otherGameObject.GetComponent<Rigidbody>() == null)
                    {
                        otherGameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f),
                            Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
                    }
                }
            }
        }
        else if (playing)
        {
            if (otherTransform.CompareTag("PickUp"))
            {
                pickUpCollected++;
                playerPickupBar.addPickup();
                Destroy(other.gameObject);
                print("pickUpCollected: " + pickUpCollected);
                if (pickUpCollected >= playerPickupBar.pickUpsToCollectTillExplosion)
                {
                    NextLevel();
                }
            }
        }
    }

    private void CheckCollision(Transform other)
    {
        var multiTag = other.gameObject.GetComponent<CustomTag>();
        if (multiTag != null)
        {
            bool destroyedItem = gameManager.AddDestroyedItem(multiTag, other.transform);
            if (!destroyedItem)
            {
                playerManaBar.decManaBeMaca();
            }
        }
    }

    void AddRigidChildren(Transform parent)
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

    void NextLevel()
    {
        gameManager.disableImage();
        breaking = true;
        playing = false;
        Destroy(playerManaBar.transform.parent.gameObject);
        Destroy(playerPickupBar.transform.parent.gameObject);

        // explode
        transform.DOScale(new Vector3(50, 0, 15), 1.5f);
        // transform.DOLocalRotate(new Vector3(15, 270, 90), 15).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear)
        //     .SetRelative();
        transform.DOLocalRotate(new Vector3(15, 270, 0), 15).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear)
            .SetRelative();

        StartCoroutine(MoveScene());
    }

    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(10);
        gameManager.NextLevel();
    }
}