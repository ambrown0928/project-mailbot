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
        
        [SerializeField] public List<T> storage;
        
        public Storage(List<T> storage) // loading constructor
        {
            this.storage = storage;
        }

        public Storage()
        {
            storage = new List<T>();
        }

        public Storage(int count)
        {
            storage = new List<T>();
            for(int i = 0; i < count; i++)
            {
                storage.Add(default(T));
            }
        }
        
        public void Create(T item)
        {
            try
            {
                T searchResult = Search(item);
                Debug.Log(searchResult.ToString());
                throw new ItemAlreadyExistsException();
            }
            catch (System.NullReferenceException)
            { // search failed - array doesn't contain item
                storage.Add(item);
            }
        }    
        public void Insert(int index, T item)
        {
            storage.Insert(index, item);
        }   
        public T Read(int index)
        {
            return storage[index];
        }
        public void Update(T item)
        {
            T searchResult = Search(item);
            searchResult = item;
        }        
        public T Search(T item)
        {
           return storage.Find(x => x.Equals(item));
        }
        public void Delete(T item)
        {
            storage.Remove(item);
        }
        public void DeleteIndex(int index)
        {
            storage.RemoveAt(index);
        }
    }
}
