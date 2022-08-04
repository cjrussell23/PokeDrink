using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Vector3 teleportLocation;
    public Vector3 TeleportLocation
    {
        get { return teleportLocation;}
    }
}
