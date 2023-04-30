using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.UI;
using Tasks;

namespace Respawn
{
    public class RespawnPointController : MonoBehaviour
    {
        [SerializeField] private RespawnData data; 
        [SerializeField] private RectTransform canvas;
        [SerializeField] private WindowController respawnWindow;

        [SerializeField] private Task taskToGive;
        
        public RespawnData Data { get => data; set => data = value; }
    
        public void OpenRespawnWindow()
        {
            respawnWindow.Open();
            respawnWindow.GetComponentInChildren<Text>().text = data.location;
            respawnWindow.GetComponentInChildren<TaskGiverWindow>().TaskToGive = taskToGive;
            respawnWindow.GetComponentInChildren<TaskGiverWindow>().SetTaskWindow();
        }
        public void CloseRespawnWindow()
        {
            respawnWindow.Close();
        }
        public bool IsOpen()
        {
            return respawnWindow.gameObject.activeInHierarchy;
        }
    }
}
