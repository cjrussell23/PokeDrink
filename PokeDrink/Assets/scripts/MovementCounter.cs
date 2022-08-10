using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementCounter : MonoBehaviour
{
    [SerializeField] private Text movmentText;
    [SerializeField] private GameObject movementCounter;
    private int movement;
    private int oldMovement;
    public int Movement {
        get { return movement; }
        set { movement = value; }
    }
    private void FixedUpdate() {
        if (movement > 0 ){
            movementCounter.SetActive(true);
        }
        else{
            movementCounter.SetActive(false);
        }
        if(movement != oldMovement){
            oldMovement = movement;
            movmentText.text = ($"You can move {movement} spaces.");
        }
    }
}
