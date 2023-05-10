using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Loot
{
    public class LootObserver : IObserver<ItemSaveData>
    {
        private IDisposable unsubscriber;
        public ItemSaveData currentItem;

        public LootObserver() { }

        public virtual void Subscribe(IObservable<ItemSaveData> provider)
        {
           if (provider != null)
              unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
           this.Unsubscribe();
        }

        public virtual void OnError(Exception e) { }

        public virtual void OnNext(ItemSaveData value)
        {
            currentItem = value;
        }

        public virtual void Unsubscribe()
        {
           unsubscriber.Dispose();
        }

    }
}
