using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Respawn
{
    public class PlayerRespawnController : MonoBehaviour
    {
        [SerializeField] private RespawnData respawnData;
        [SerializeField] private float respawnTime;

        public RespawnData RespawnData { get => respawnData; set => respawnData = value; }
        public float RespawnTime { get => respawnTime; set => respawnTime = value; }

        public void OnRespawnEnter(RespawnData respawnData)
        {
            this.respawnData = respawnData;
        }
    }
    
}