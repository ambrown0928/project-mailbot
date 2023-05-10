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
        
        [SerializeField] public List<List<T>> storage;
        [SerializeField] public int width;
        [SerializeField] public int length;
        
        public Storage(List<List<T>> storage, int width, int length) // loading constructor
        {
            this.storage = storage;
            this.width = width;
            this.length = length;
        }

        public Storage()
        {

        }

        public Storage(int width, int length)
        {
            storage = new List<List<T>>();
            this.width = width;
            this.length = length;
            for(int i = 0; i < width; i++)
            {
                storage.Add(new List<T>());
                for(int j = 0; j < length; j++)
                {
                    storage[i].Add(default(T));
                }
            }
        }

        public void InsertAt(T item, Vector2Int slot)
        {
            if ( SlotIsNotEmpty(slot) ) throw new System.InventorySlotOccupiedException();
            storage[slot.x][slot.y] = item;
        }
        public T RemoveAt(Vector2Int slot)
        {
            T item = storage[slot.x][slot.y];
            if ( NoItemInLocation(item) ) return default(T);
            storage[(int)slot.x][(int)slot.y] = default(T);
            return item;
        }
        public T GetAt(Vector2 slot)
        {
            T item = storage[(int)slot.x][(int)slot.y];
            if ( NoItemInLocation(item) ) return default(T);
            return item;
        }
        public T GetItemSearch(T item)
        {
            foreach(List<T> list in storage)
            {
                foreach(T searchItem in list)
                {
                    if( searchItem == null ) continue;
                    if( searchItem.Equals(item) )
                    {
                        return searchItem;
                    }
                }
            }
            throw new ItemNotFoundException();
        }
        public List<T> GetAllItemsSearch(T item)
        {
            List<T> itemList = new List<T>();
            foreach(List<T> list in storage)
            {
                foreach(T searchItem in list)
                {
                    if( searchItem == null ) continue;
                    if( searchItem.Equals(item) )
                    {
                        itemList.Add(searchItem);
                    }
                }
            }
            if(itemList.Count == 0) throw new ItemNotFoundException();
            return itemList;
        }
        private static bool NoItemInLocation(T item)
        {
            return item == null;
        }

        private bool SlotIsNotEmpty(Vector2Int slot)
        {
            return storage[(int)slot.x][(int)slot.y] != null;
        }

        public Vector2Int GetNextEmpty()
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < length; j++)
                {
                    Debug.Log(storage[i][j]);
                    if(Object.Equals(storage[i][j], default(T)) || Object.Equals(storage[i][j], null))
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }
            throw new InventoryIsFullException();
        }
        public void UpdateItem(T item, Vector2Int slot)
        {
            if(!SlotIsNotEmpty(slot)) throw new ItemNotFoundException();
            storage[slot.x][slot.y] = item;
        }
    }
}
