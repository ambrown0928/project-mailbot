using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Editor;

namespace Editor
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