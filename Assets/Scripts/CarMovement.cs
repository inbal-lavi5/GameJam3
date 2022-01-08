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
    private bool riding = true;
    [SerializeField] private bool a = false;
    [SerializeField] private int timeToReappear = 10;


    // Start is called before the first frame update
    void Start()
    {
        forward = new Vector3(dir, 0, 0);
        // startX = transform.localPosition.x;
        // transform.position = Vector3.zero;
        // StartCoroutine(StartOver());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.CompareTag("Collapse"))
        {
            transform.Translate(forward * speed * Time.deltaTime);
            if (a)
                print(transform.localPosition);
            Vector3 position = transform.localPosition;
            if (position.x <= endX && small || position.x >= endX && !small)
            {
                transform.localPosition = new Vector3(startX, position.y, position.z);
            }
        }
    }

    IEnumerator StartOver()
    {
        riding = false;

        yield return new WaitForSeconds(timeToReappear);

        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(startX - 20, position.y, position.z);
        riding = true;
    }
}