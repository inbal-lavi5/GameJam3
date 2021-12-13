using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera main;
    [SerializeField] Camera cam1;
    [SerializeField] Camera cam2;

    [SerializeField] Transform rotator1;
    [SerializeField] Transform rotator2;
    [SerializeField] float speed = 5f;
    
    [SerializeField] float speedCrack = 5f;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    void Start()
    {
        main.enabled = true;
        cam1.enabled = false;
        cam2.enabled = false;
    }

    void Update()
    {
        CameraControl();

        // if (Time.time > nextActionTime)
        // {
        //     //rand period
        //     nextActionTime += period;
        //     // execute block of code here
        //     speed = -speed;
        //     print(speed);
        //     // transform.position = new Vector3(speed, 0.51f, 0);
        //     transform.Translate(speed, 0, 0);
        //
        // }
        if (Input.GetKey(KeyCode.Space))
        {
            rotator1.Rotate(speed * Time.deltaTime, 0, 0);
        }
        
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     rotator1.Rotate(speed * Time.deltaTime, 0, 0);
        // }
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     rotator2.Rotate(-speed * Time.deltaTime, 0, 0);
        // }
    }

    private void CameraControl()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            main.enabled = true;
            cam1.enabled = false;
            cam2.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            main.enabled = false;
            cam1.enabled = true;
            cam2.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            main.enabled = false;
            cam1.enabled = false;
            cam2.enabled = true;
        }
    }
}