using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Battery
{
    public class BatteryController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text batteryText;
        [SerializeField] private Image batteryPercent;
        [SerializeField] private List<Sprite> batteryFrames;
        [Header("Color")]
        [SerializeField] private Color fullColor;
        [SerializeField] private Color emptyColor;
        [Header("Stats")]
        [SerializeField] private PlayerStatObject stats;
        [SerializeField] private float decreaseRate;

        private float currentPercent;

        public void SetCurrentPercent(float currentPercent) => this.currentPercent = currentPercent * 100;

        void Awake()
        {
            stats.battery.ResetValue();
        }

        // Called in UIControllerGlobalContainer so it runs when menu is closed
        public void BatteryUpdate()
        {
            DecreaseBattery();
            SetCurrentPercent(stats.battery.GetPercentValue());
            UpdatePercent();
            UpdateText();
        }
        
        private void UpdatePercent()
        { // updates the sprite of the battery based on current percent left
            batteryPercent.sprite = (currentPercent < 85) ? batteryFrames[(int)currentPercent / 10] : batteryFrames[8];
            batteryPercent.color = (currentPercent > 10) ? Color.Lerp(fullColor, emptyColor, 1 / Mathf.Abs((currentPercent / 10) - 1)) : Color.clear;
        }
        private void UpdateText()
        { // updates the text of the battery to show percent left and how much longer is left
            float lengthTillZero = stats.battery.GetValue() / decreaseRate;
            
            batteryText.text = string.Format("{0:p0}\n", currentPercent / 100);
            batteryText.text += TimeSpan.FromSeconds(lengthTillZero).ToString("hh':'mm':'ss");
        }

        private void DecreaseBattery()
        { // decreases battery percentage over time
            if(stats.battery.IsZero())
            {
                // TODO - add code to indicate battery is zero and slowly kill player
                return; // return to avoid depleting past zero
            }
            stats.battery.AddValue(-decreaseRate * Time.deltaTime);
        }
    }
    
}