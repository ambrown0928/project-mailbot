using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
   /// 
   /// Class for observing when the dialogue has ended
   /// 
   public class DialogueObserver : IObserver<Dialogue>
   {
       private IDisposable unsubscriber;
       private Dialogue currentDialogue;
       public Dialogue CurrentDialogue { get => currentDialogue; set => currentDialogue = value; }

       public DialogueObserver() { }

       public virtual void Subscribe(IObservable<Dialogue> provider)
       {
          if (provider != null)
             unsubscriber = provider.Subscribe(this);
       }

       public virtual void OnCompleted()
       {
          this.Unsubscribe();
       }

       public virtual void OnError(Exception e) { }

       public virtual void OnNext(Dialogue value)
       {
          CurrentDialogue = value;
       }

       public virtual void Unsubscribe()
       {
          unsubscriber.Dispose();
       }
   }
}
