using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSize : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private bool shake;

    public void CopySize(ObjectSize otherSize)
    {
        size = otherSize.size;
        shake = otherSize.shake;
    }

    public float GetSize()
    {
        return size;
    }

    public bool toShake()
    {
        return shake;
    }
}