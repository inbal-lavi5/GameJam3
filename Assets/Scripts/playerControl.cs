using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class playerControl : MonoBehaviour
{
    private GameManager gm;

    public float moveSpeed = 15;
    public KeyCode up;
    public KeyCode left;
    public KeyCode right;
    private Vector3 moveDir;
    private Rigidbody rb;

    [SerializeField] private float fall = 0.05f;
    [SerializeField] private float maxAngle = 40;
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] private bool breaking = true;

    private int pickUpCollected = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
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
                    otherGameObject.AddComponent<Rigidbody>();
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
            child.gameObject.AddComponent<Rigidbody>();
            if (child.childCount > 0)
                AddRigidChildren(child);
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