using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Respawn Data Object
/// Creates a scriptable object that stores the respawn point's 
/// location.

namespace Respawn
{
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Respawn Data Object", order = 1)]
    public class RespawnData : ScriptableObject
    {
        public string location;
        public Vector3 points;
        // TODO Quest Board, 
    }
}
