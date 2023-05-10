using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Loot
{
    public class LootReporter : IObservable<ItemSaveData>
    {
        public LootReporter()
        {
           observers = new List<IObserver<ItemSaveData>>();
        }

        private List<IObserver<ItemSaveData>> observers;

        public IDisposable Subscribe(IObserver<ItemSaveData> observer)
        {
           if (! observers.Contains(observer))
              observers.Add(observer);
           return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
           private List<IObserver<ItemSaveData>>_observers;
           private IObserver<ItemSaveData> _observer;

           public Unsubscriber(List<IObserver<ItemSaveData>> observers, IObserver<ItemSaveData> observer)
           {
              this._observers = observers;
              this._observer = observer;
           }

           public void Dispose()
           {
              if (_observer != null && _observers.Contains(_observer))
                 _observers.Remove(_observer);
           }
        }
        public void AddItem(ItemSaveData val)
        {
            foreach (IObserver<ItemSaveData> observer in observers)
            {
                observer.OnNext(val);
            }
        }
        public void RemoveItem(ItemSaveData val)
        {
            foreach (IObserver<ItemSaveData> observer in observers)
            {
                observer.OnNext(val);
            }
        }
        public void EndTransmission()
        {
            foreach(IObserver<ItemSaveData> observer in observers)
            {
                if(observers.Contains(observer))
                    observer.OnCompleted();
                
            }
            observers.Clear();
        }
    }
}
