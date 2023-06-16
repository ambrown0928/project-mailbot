using System.Collections;
using System.Collections.Generic;
using Dialog;
using NPC.ActionMenu;
using Tasks;
using UnityEngine;

namespace NPC
{
    public class NPCController : MonoBehaviour
    {
        public string nPCName;

        private DialogTrigger dialogTrigger;
        
        bool withinPlayer = false;

        void Awake()
        {
            dialogTrigger = GetComponent<DialogTrigger>();

        }
        void OnTriggerEnter(Collider other) 
        {
            if( other.tag == "Player" ) withinPlayer = true;   
        }
        void OnTriggerExit(Collider other) 
        {
            if( other.tag == "Player" ) withinPlayer = false;
        }

        void OnActivate()
        { // TODO - better implement task giver / dialogue system
            if ( !withinPlayer ) return; // stop if not within player or action menu is opened

            dialogTrigger.TriggerDialog();
        }
    }

}
