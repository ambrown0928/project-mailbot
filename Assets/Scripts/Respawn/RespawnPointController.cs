using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour
{
    [SerializeField]
    private RespawnData data; 

    public RespawnData Data { get => data; set => data = value; }
}
