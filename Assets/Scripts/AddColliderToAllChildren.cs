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
            // meshCollider.cookingOptions = ~MeshColliderCookingOptions.None;
            if (isCollapse)
            {
                child.tag = "Collapse";
            }

            // add custom tag
            CustomTag parentCustomTag = parent.gameObject.GetComponent<CustomTag>();
            if (parentCustomTag != null)
            {
                CustomTag childCustomTag = child.gameObject.GetComponent<CustomTag>();
                if (childCustomTag == null)
                {
                    childCustomTag = child.gameObject.AddComponent<CustomTag>();
                }
                childCustomTag.CopyTags(parentCustomTag.GetTags());
            }

            // do the same for kids
            if (child.childCount > 0)
            {
                AddChildren(child);
            }
        }
    }
}