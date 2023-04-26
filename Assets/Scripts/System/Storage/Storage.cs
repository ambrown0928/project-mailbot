using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Storage
{
    ///
    /// Creates a storage object that creates any size matrix and allows for insert/remove.
    /// 
    [System.Serializable]
    public class Storage<T>
    {
        
        private T [,] storage;
        
        public Storage(T [,] storage)
        {
            this.storage = storage;
        }

        public Storage()
        {

        }

        public void InsertAt(T item, Vector2 slot)
        {
            try
            {
                if ( SlotIsNotEmpty(slot) ) throw new System.InventorySlotOccupiedException();

                storage[(int)slot.x, (int)slot.y] = item;
            }
            catch (System.InventorySlotOccupiedException)
            {
                Debug.Log("Slot has an item in it already!");
                throw;
            }
        }
        public T RemoveAt(Vector2 slot)
        {
            T item = storage[(int)slot.x, (int)slot.y];
            if ( NoItemInLocation(item) ) return default(T);
            storage[(int)slot.x, (int)slot.y] = default(T);
            return item;
        }

        private static bool NoItemInLocation(T item)
        {
            return item == null;
        }

        private bool SlotIsNotEmpty(Vector2 slot)
        {
            return storage[(int)slot.x, (int)slot.y] != null;
        }
    }
}
