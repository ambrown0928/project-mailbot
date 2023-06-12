using System;
using System.Collections;
using System.Collections.Generic;
using Loot;
using UnityEngine;

/// 
/// Class for a scriptable object representation 
/// 

namespace Inventory.Items
{
    [System.Serializable] [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Package ", order = 1)]
    public class PackagePrototype : ItemPrototype
    {
        internal LootObserver lootObserver;
        public LootWindowController lootWindowController;
        public Item intendedItem; // item package is meant to have
        public Item currentItem; // item package currently contains
        
        void OnEnable() 
        {
            lootObserver = new LootObserver();
        }
        public override void Use()
        {
            lootObserver.Subscribe(lootWindowController.OpenLootWindow(currentItem, name));
        }
        public void RemoveItem()
        {
            currentItem = null;
            lootObserver.currentItem = null;
        }
        public void AddItem(Item item)
        {
            if(currentItem != null && currentItem.Name != "" && currentItem.Quantity != 0) throw new PackageIsFullException();

            currentItem = item;
        }
    }
}
