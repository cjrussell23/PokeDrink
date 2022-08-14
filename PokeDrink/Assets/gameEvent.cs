using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEvent : MonoBehaviour
{
    [SerializeField] private string eventMessage;
    public string GetEventMessage(){
        return eventMessage;
    }
}
