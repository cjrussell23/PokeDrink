using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour {
    private Camera playerCam;
    public void Start(){
        playerCam = GetComponentInChildren<Camera>();
        playerCam.enabled = false;
    }
    void Update(){
        SceneManager.activeSceneChanged += SceneChanged;
    }
    private void SceneChanged(Scene current, Scene next) {
        if (next.name.Equals("Scene_SteamworksGame"))
        {
            // Set active if in game scene
            if (isLocalPlayer)
            {
                if (playerCam == null)
                {
                    playerCam = GetComponentInChildren<Camera>();
                }
                playerCam.enabled = true;
            }
        }
    }
}