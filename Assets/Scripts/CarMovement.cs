using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 17;
    private Vector3 forward;
    private int dir = -1;
    [SerializeField] bool small = true;
    [SerializeField] float startX;
    [SerializeField] float endX;

    // Start is called before the first frame update
    void Start()
    {
        forward = new Vector3(dir, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.CompareTag("Collapsed"))
        {
            transform.Translate(forward * speed * Time.deltaTime);
            Vector3 position = transform.localPosition;
            if (position.x <= endX && small || position.x >= endX && !small)
            {
                transform.localPosition = new Vector3(startX, position.y, position.z);
            }
        }
    }
}