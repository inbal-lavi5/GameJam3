using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class playerControl : MonoBehaviour
{
    private GameManager gm;

    public float moveSpeed = 15;
    public KeyCode up;
    public KeyCode left;
    public KeyCode right;
    private Vector3 moveDir;
    private Rigidbody rb;
    private GameManager gameManager;
    
    [SerializeField] private manaBar playerManaBar;
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
        gm = GameManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(left))
        {
            moveDir = new Vector3(-1, 0, 1).normalized;
        }

        else if (Input.GetKeyDown(right))
        {
            moveDir = new Vector3(1, 0, 1).normalized;
        }

        else //if (Input.GetKey(up))
        {
            moveDir = new Vector3(0, 0, 1).normalized;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLevel();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            breaking = !(playerManaBar.getMana() > gameManager.MANA_MIN);
        }

        else
        {
            breaking = true;
        }
    }

    private void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");
        Vector3 m_EulerAngleVelocity = new Vector3(0, input * rotationSpeed, 0);

        float angle = rb.transform.eulerAngles.y;
        angle = (angle > 180) ? angle - 360 : angle;
        if (angle > maxAngle && input > 0 || angle < -maxAngle && input < 0)
        {
            moveDir = new Vector3(0, 0, 1).normalized;
        }
        else
        {
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (breaking)
        {
            // print("COLLLLL tag: " + other.transform.tag + " name: " + other.transform.name);
            if (other.transform.tag != "Plane")
            {
                GameObject otherGameObject = other.gameObject;
                if (otherGameObject.GetComponent<Rigidbody>() == null)
                {
                    otherGameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));

                    if (otherGameObject.transform.parent != null)
                    {
                        Transform rootObject = otherGameObject.transform.parent.transform.parent;  // todo is it correct ??

                        if (rootObject != null && rootObject.tag != "road" && rootObject.tag != "sidewalk")
                        {
                            AddRigidChildren(rootObject);
                        }
                    }
                    // StartCoroutine(ExecuteAfterTime(fall, otherGameObject));
                }
            }
        }

        // else
        {
            if (other.transform.tag == "PickUp")
            {
                pickUpCollected++;
                Destroy(other.gameObject);
                print("pickUpCollected: " + pickUpCollected);
                if (pickUpCollected >= gm.pickUpsToCollectTillExplosion)
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
                child.gameObject.AddComponent<Rigidbody>().AddForce(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
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
        gm.NextLevel();
    }
}