using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items
{
    public class Package : Item
    {
        public List<Item> intendedItems; // items package is meant to have
        public List<Item> currentItems;
    }
}
