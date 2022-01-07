using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTag : MonoBehaviour
{
     [SerializeField]
     private List<string> tags = new List<string>();
     
     public bool HasTag(string tag)
     {
         return tags.Contains(tag);
     }

     public void CopyTags(List<string> otherTags)
     {
         tags.AddRange(otherTags);
     }

     public List<string> GetTags()
     {
         return tags;
     }
     
     public void Rename(int index, string tagName)
     {
         tags[index] = tagName;
     }
     
     public string GetAtIndex(int index)
     {
         return tags[index];
     }
     
     public int Count
     {
         get { return tags.Count; }
     }
}
