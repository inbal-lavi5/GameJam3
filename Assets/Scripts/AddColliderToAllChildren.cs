using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddColliderToAllChildren : MonoBehaviour
{
    void Awake()
    {
        AddChildren(transform);
    }

    void AddChildren(Transform parent)
    {
        bool isCollapse = parent.CompareTag("Collapse");
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            
            // add collider
            MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            if (isCollapse)
            {
                child.tag = "Collapse";
            }

            // add custom tag
            CustomTag customTag = parent.gameObject.GetComponent<CustomTag>();
            if (customTag != null)
            {
                CustomTag addComponent = child.gameObject.AddComponent<CustomTag>();
                addComponent.CopyTags(customTag.GetTags());
            }

            // do the same for kids
            if (child.childCount > 0)
            {
                AddChildren(child);
            }
        }
    }
}