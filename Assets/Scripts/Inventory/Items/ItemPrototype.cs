using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 
/// Class for storing item data as an object in unity. Designed
/// for saving the data an item contains permanently as well as
/// any functions the item might have. When passed, it's passed
/// by reference with the intention of never being modified. 
/// Exceptions only occur when the ItemPrototype is a package.
/// 


namespace Inventory.Items
{
    [System.Serializable] [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Item", order = 1)]
    public class ItemPrototype : ScriptableObject
    {
        public Sprite icon; 
        [TextArea(3, 10)] public string description;
        public virtual void Use() { }
    }
}


