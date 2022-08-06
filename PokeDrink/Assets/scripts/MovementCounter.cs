using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementCounter : MonoBehaviour
{
    [SerializeField] private Text movmentText;
    private int movement;
    private int oldMovement;
    public int Movement {
        get { return movement; }
        set { movement = value; }
    }
    private void Update() {
        if(movement != oldMovement){
            oldMovement = movement;
            movmentText.text = movement.ToString();
        }
    }
}
