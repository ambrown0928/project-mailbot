using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEditor
{
    public class RenameAttribute : PropertyAttribute
    {
        public string newName {get; private set;}
        
        public RenameAttribute (string name)
        {
            newName = name;
        }
    }
}