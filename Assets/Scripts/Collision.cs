using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // gameObject.AddComponent<Rigidbody>();
    }

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        print("COLLLLL"+other.transform.tag);
        if (other.transform.tag == "Player")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
            }
            print("PLAYAAAAAAA");
        }
    }
}