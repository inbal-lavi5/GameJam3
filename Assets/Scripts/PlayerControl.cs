using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerControl : MonoBehaviour
{
    private GameManager gameManager;
    private bool playing = true;

    public float moveSpeed = 15;
    private Vector3 moveDir;
    private Rigidbody rb;
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] private float maxAngle = 40;
    [SerializeField] private float fall = 0.05f;

    [SerializeField] private bool breaking = true;
    [SerializeField] private ManaBar playerManaBar;
    [SerializeField] private PickupBar playerPickupBar;

    private int pickUpCollected = 0;
    Dictionary<string, int> countingList = new Dictionary<string, int>();

    void Add(string s)
    {
        if (countingList.ContainsKey(s))
            countingList[s]++;
        else
            countingList.Add(s, 1);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (!playing) return;
        if (Input.GetKey(KeyCode.Space))
        {
            playerManaBar.dec = true;
            breaking = !(playerManaBar.getMana() > gameManager.MANA_MIN);
        }
        else
        {
            playerManaBar.dec = false;
            breaking = true;
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
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        Transform otherTransform = other.transform;

        if (breaking)
        {
            if (!otherTransform.CompareTag("Plane"))
            {
                GameObject otherGameObject = other.gameObject;
                // if (otherTransform.CompareTag("Collapsed"))
                // {
                //     // gameManager.Reset();
                //     print("LOSEEEEE");
                // }
                if (otherTransform.CompareTag("Collapse"))
                {
                    // add rigid body to all children
                    AddRigidChildren(otherTransform.parent.parent);
                    CheckCollision(other);
                }
                else
                {
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
                if (pickUpCollected >= gameManager.pickUpsToCollectTillExplosion)
                {
                    NextLevel();
                }
            }
        }
    }

    private void CheckCollision(UnityEngine.Collision other)
    {
        var multiTag = other.gameObject.GetComponent<CustomTag>();
        if (multiTag != null && multiTag.HasTag("StreetLight"))
        {
            Add("yo");
            Debug.Log("Character collided with " + countingList);
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

    IEnumerator ExecuteAfterTime(float time, GameObject other)
    {
        yield return new WaitForSeconds(time);
        other.gameObject.GetComponent<MeshCollider>().isTrigger = true;
        yield return new WaitForSeconds(3);
        Destroy(other.gameObject);
    }

    void NextLevel()
    {
        breaking = true;
        playing = false;
        Destroy(playerManaBar.transform.parent.gameObject);
        Destroy(playerPickupBar.transform.parent.gameObject);

        // explode
        transform.DOScale(new Vector3(50, 0, 15), 1.5f);
        transform.DOLocalRotate(new Vector3(15, 270, 90), 15).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear)
            .SetRelative();

        StartCoroutine(MoveScene());
    }

    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(10);
        gameManager.NextLevel();
    }
}