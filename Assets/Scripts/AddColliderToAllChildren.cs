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
        bool isTag = !parent.CompareTag("Untagged");
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);


            // add collider
            MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            if (isTag)
            {
                child.tag = parent.tag;
            }

            // add size
            ObjectSize parentObjectSize = parent.gameObject.GetComponent<ObjectSize>();
            if (parentObjectSize != null)
            {
                ObjectSize childObjectSize = child.gameObject.GetComponent<ObjectSize>();
                if (childObjectSize == null)
                {
                    childObjectSize = child.gameObject.AddComponent<ObjectSize>();
                }

                childObjectSize.CopySize(parentObjectSize);
            }

            // add only father flag
            OnlyFatherFlag parentOnlyFatherFlag = parent.gameObject.GetComponent<OnlyFatherFlag>();
            if (parentOnlyFatherFlag != null)
            {
                child.gameObject.AddComponent<OnlyFatherFlag>();
            }

            // do the same for kids
            if (child.childCount > 0)
            {
                AddChildren(child);
            }
        }
    }
}