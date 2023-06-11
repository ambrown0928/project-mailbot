using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Loot
{
    public class LootReporter : IObservable<Item>
    {
        public LootReporter()
        {
           observers = new List<IObserver<Item>>();
        }

        private List<IObserver<Item>> observers;

        public IDisposable Subscribe(IObserver<Item> observer)
        {
           if (! observers.Contains(observer))
              observers.Add(observer);
           return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
           private List<IObserver<Item>>_observers;
           private IObserver<Item> _observer;

           public Unsubscriber(List<IObserver<Item>> observers, IObserver<Item> observer)
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
        public void AddItem(Item val)
        {
            foreach (IObserver<Item> observer in observers)
            {
                observer.OnNext(val);
            }
        }
        public void RemoveItem(Item val)
        {
            foreach (IObserver<Item> observer in observers)
            {
                observer.OnNext(val);
            }
        }
        public void EndTransmission()
        {
            foreach(IObserver<Item> observer in observers)
            {
                if(observers.Contains(observer))
                    observer.OnCompleted();
                
            }
            observers.Clear();
        }
    }
}
