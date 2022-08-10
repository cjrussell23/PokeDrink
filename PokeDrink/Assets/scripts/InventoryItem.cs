using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    private int itemIndex;
    public bool isLocked;
    public void Start(){
        isLocked = false;
        inventory = gameObject.GetComponentInParent<Inventory>();
    }
    public void OnToggle(bool toggle){
        isLocked = toggle;
        Debug.Log("Toggled to " + toggle);
        SetLock(toggle);
    }
    public void SetIndex(int index){
        itemIndex = index;
    }
    public int GetIndex(){
        return itemIndex;
    }
    private void SetLock(bool lockState){
        inventory.LockPokemon(itemIndex, lockState);
    }
}
