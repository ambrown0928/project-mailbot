using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 
/// Class for storing item 
/// 


namespace Inventory.Items
{
    [System.Serializable] [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public Sprite icon; 
        public new string name;
        public IType type;
        [TextArea(3, 10)] public string description;

        public virtual void Use() { type.Use(); }
    }
}


