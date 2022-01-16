using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 4f;
    [SerializeField] private float cameraChangeY = 10f;
    [SerializeField] private float cameraChangeZ = 10f;
    [SerializeField] private float angleChange = 10f;


    public void changeCameraPosition()
    {
        Vector3 finalPsition = new Vector3(transform.position.x, transform.position.y + cameraChangeY, transform.position.z + cameraChangeZ);
        transform.position = Vector3.Lerp(transform.position, finalPsition, Time.deltaTime * cameraSpeed);
        
        
        float newXAngle = transform.rotation.eulerAngles.x + angleChange;
        Vector3 curAngle = new Vector3(Mathf.LerpAngle(transform.rotation.eulerAngles.x, newXAngle, Time.deltaTime * cameraSpeed), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.eulerAngles = curAngle;
    }
}
