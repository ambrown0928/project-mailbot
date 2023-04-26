using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Respawn
{
    public class RespawnPointController : MonoBehaviour
    {
        [SerializeField]
        private RespawnData data; 
        [SerializeField]
        private RectTransform canvas;
        [SerializeField]
        private GameObject window;
        [SerializeField]
        private GameObject respawnWindow;
        
        public RespawnData Data { get => data; set => data = value; }
    
        public void OpenRespawnWindow()
        {
            if(respawnWindow != null) return;
            respawnWindow = Instantiate(window);
            respawnWindow.GetComponentInChildren<Text>().text = data.location;
            RectTransform windowRectTransform = respawnWindow.GetComponent<RectTransform>();
    
            windowRectTransform.SetParent(canvas);
            windowRectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }
        public void CloseRespawnWindow()
        {
            Destroy(respawnWindow);
        }
    }
}
