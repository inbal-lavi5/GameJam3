using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float moveSpeed = 15;
    public KeyCode up;
    public KeyCode left;
    public KeyCode right;
    private Vector3 moveDir;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(0, 0f, 1f).normalized;

    }

    void Update()
    {
        if (Input.GetKey(left))
        {
            moveDir = new Vector3(-1f, 0f, 1f).normalized;
        }
        
        else if (Input.GetKey(right))
        {
            moveDir = new Vector3(1, 0f, 1f).normalized;
        }
        
        else if (Input.GetKey(up))
        {
            moveDir = new Vector3(0, 0f, 1f).normalized;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }
}
