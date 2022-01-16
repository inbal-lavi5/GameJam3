using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSize : MonoBehaviour
{
    [SerializeField] private float size;
    
    public void CopyTag(float otherSize)
    {
        size = otherSize;
    }

    public float GetSize()
    {
        return size;
    }
}