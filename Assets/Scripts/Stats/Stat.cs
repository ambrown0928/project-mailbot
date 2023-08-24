using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    /// <summary>
    ///  The stat class is a container for 2 values where one
    ///  stores the maximum allowed amount, and the other
    ///  stores the current value of the stat.
    /// </summary>
    /// 
    [Serializable]
    public class Stat
    {
        [SerializeField] private float currentValue;
        [SerializeField] private int maxValue;

        public Stat(int maxValue)
        {
            this.currentValue = maxValue;
            this.maxValue = maxValue;
        }

        public float GetPercentValue()
        {
            return currentValue / maxValue;
        }
        public float GetValue()
        {
            return currentValue;
        }
        public void SetValue(float value)
        {
            currentValue = value;
        }
        public void AddValue(float value)
        {
            currentValue += value;
        }
        public void ResetValue()
        {
            currentValue = maxValue;
        }
        public bool IsZero()
        {
            return currentValue <= 0;
        }
    }
}
