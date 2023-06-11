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
        public T Read(T item)
        {
            return default(T);
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
    }
}
