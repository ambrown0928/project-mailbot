using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Loot
{
    public class LootObserver : IObserver<Item>
    {
        private IDisposable unsubscriber;
        public Item currentItem;

        public LootObserver() { }

        public virtual void Subscribe(IObservable<Item> provider)
        {
           if (provider != null)
              unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
           this.Unsubscribe();
        }

        public virtual void OnError(Exception e) { }

        public virtual void OnNext(Item value)
        {
            currentItem = value;
        }

        public virtual void Unsubscribe()
        {
           unsubscriber.Dispose();
        }

    }
}
