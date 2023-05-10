using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    /// 
    /// Class for reporting when dialogue has ended
    /// 
    public class DialogueReporter : IObservable<Dialogue>
    {
        public DialogueReporter()
        {
           observers = new List<IObserver<Dialogue>>();
        }

        private List<IObserver<Dialogue>> observers;

        public IDisposable Subscribe(IObserver<Dialogue> observer)
        {
            if (! observers.Contains(observer))
            {
              observers.Add(observer);
              Debug.Log(observer.ToString());
            }
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Dialogue>>_observers;
            private IObserver<Dialogue> _observer;

            public Unsubscriber(List<IObserver<Dialogue>> observers, IObserver<Dialogue> observer)
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
        public void ReportDialogue(Dialogue val)
        {

            foreach (IObserver<Dialogue> observer in observers)
            {
                observer.OnNext(val);
            }
        }
        public void EndTransmission()
        {
            foreach(IObserver<Dialogue> observer in observers)
            {
                if(observers.Contains(observer))
                    observer.OnCompleted();
                
            }
            observers.Clear();
        }
    }
    
}