using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu]
    public class PlayerStatObject : ScriptableObject
    {
        public Stat health;
        public Stat battery;
    }
}