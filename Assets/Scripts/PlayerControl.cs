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

    public float moveSpeed = 15;
    public KeyCode up;
    public KeyCode left;
    public KeyCode right;
    private Vector3 moveDir;
    private Rigidbody rb;

    [SerializeField] private ManaBar playerManaBar;
    [SerializeField] private float fall = 0.05f;
    [SerializeField] private float maxAngle = 40;
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] private bool breaking = true;

    private int pickUpCollected = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
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
        float input = Input.GetAxis("Horizontal");
        Vector3 m_EulerAngleVelocity = new Vector3(0, input * rotationSpeed, 0);
        moveDir = new Vector3(input, 0, 1).normalized;
        
        // float angle = rb.transform.eulerAngles.y;
        // angle = (angle > 180) ? angle - 360 : angle;
        // if (angle > maxAngle && input > 0 || angle < -maxAngle && input < 0)
        // {
        //     moveDir = new Vector3(0, 0, 1).normalized;
        // }
        // else
        // {
        //     Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        //     rb.MoveRotation(rb.rotation * deltaRotation);
        // }
        
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        Transform otherTransform = other.transform;

        if (breaking)
        {
            // print("COLLLLL tag: " + other.transform.tag + " name: " + other.transform.name);
            if (!otherTransform.CompareTag("Plane"))
            {
                GameObject otherGameObject = other.gameObject;
                if (otherGameObject.GetComponent<Rigidbody>() == null)
                {
                    otherGameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f),
                        Random.Range(0f, 0.5f));
                    if (otherTransform.CompareTag("Collapse"))
                    {
                        AddRigidChildren(otherTransform.parent.parent);
                    }
                }
            }
        }
        else
        {
            if (otherTransform.CompareTag("PickUp"))
            {
                pickUpCollected++;
                Destroy(other.gameObject);
                print("pickUpCollected: " + pickUpCollected);
                if (pickUpCollected >= gameManager.pickUpsToCollectTillExplosion)
                {
                    NextLevel();
                }
            }
        }
    }

    void AddRigidChildren(Transform parent)
    {
        // print(parent.name);
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.GetComponent<Rigidbody>() == null)
            {
                child.gameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f),
                    Random.Range(0f, 0.5f));
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
        // explode
        transform.DORotate(Vector3.zero, 0.5f);
        transform.DOScaleX(50, 1.5f);
        transform.DOScaleZ(15, 1.5f);

        StartCoroutine(MoveScene());
    }

    IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(10);
        gameManager.NextLevel();
    }
}