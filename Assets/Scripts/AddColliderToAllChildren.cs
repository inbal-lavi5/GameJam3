using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddColliderToAllChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AddChildren(transform);
    }

    void AddChildren(Transform parent)
    {
        // print(parent.name);
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            child.gameObject.AddComponent<MeshCollider>();
            child.GetComponent<MeshCollider>().convex = true;
            if (parent.tag == "Collapse")
            {
                child.tag = "Collapse";
            }
            if (child.childCount > 0)
            {
                AddChildren(child);
            }
            else
            {
                // parent.tag = "Object";
            }
        }
    }

    void AssignTag(Transform o)
    {
        if (o.tag == "Collapse")
        {
            
        }
    }
}