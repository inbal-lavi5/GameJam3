using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private KeyCode move;
    [SerializeField] private float speed = 1;
    [SerializeField] private float currTargetPos = 5;
    [SerializeField] private float maxDistFromStart = 50;
    private float distMovedFromStart;
    private float amountMoved;
    private int currDirection;

    private void Awake()
    {
        currDirection = 1;
    }

    void Update()
    {
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
        // transform.Translate(speed * Time.deltaTime, 0, 0);

        
        if (amountMoved > currTargetPos)
        {
            distMovedFromStart += amountMoved * currDirection;
            currTargetPos = Math.Abs(distMovedFromStart) >= maxDistFromStart ?
                maxDistFromStart : Random.Range(10, 30);
            
            currDirection = -currDirection;
            amountMoved = 0;
        }

        float moved = speed * Time.deltaTime * currDirection;
        transform.Translate(moved, 0, 0);
        amountMoved += Math.Abs(moved);
    }
}