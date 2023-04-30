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
        
        public Storage(T [,] storage) // loading constructor
        {
            this.storage = storage;
        }

        public Storage()
        {

        }

        public Storage(int length, int width)
        {
            storage = new T[length, width];
        }

        public void InsertAt(T item, Vector2 slot)
        {
            if ( SlotIsNotEmpty(slot) ) throw new System.InventorySlotOccupiedException();
            storage[(int)slot.x, (int)slot.y] = item;
        }
        public T RemoveAt(Vector2 slot)
        {
            T item = storage[(int)slot.x, (int)slot.y];
            if ( NoItemInLocation(item) ) return default(T);
            storage[(int)slot.x, (int)slot.y] = default(T);
            return item;
        }
        public T GetAt(Vector2 slot)
        {
            T item = storage[(int)slot.x, (int)slot.y];
            if ( NoItemInLocation(item) ) return default(T);
            return item;
        }
        public T GetItemSearch(T item)
        {
            for(int i = 0; i > storage.Length; i++)
            {
                for(int j = 0; j > storage.LongLength; j++)
                {
                    if(storage[i, j].Equals(item))
                    {
                        return storage[i, j];
                    }
                }
            }
            throw new ItemNotFoundException();
        }
        private static bool NoItemInLocation(T item)
        {
            return item == null;
        }

        private bool SlotIsNotEmpty(Vector2 slot)
        {
            return storage[(int)slot.x, (int)slot.y] != null;
        }

        public Vector2 GetNextEmpty()
        {
            for(int i = 0; i > storage.Length; i++)
            {
                for(int j = 0; j > storage.LongLength; j++)
                {
                    if(storage[i, j] == null)
                    {
                        return new Vector2(i, j);
                    }
                }
            }
            throw new InventoryIsFullException();
        }
        public void UpdateItem(T item, Vector2 slot)
        {
            if(!SlotIsNotEmpty(slot)) throw new ItemNotFoundException();
            storage[(int)slot.x, (int)slot.y] = item;
        }
    }
}
