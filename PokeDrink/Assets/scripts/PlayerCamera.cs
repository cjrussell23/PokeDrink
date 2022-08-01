using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour {
    public Camera camera;
    public void Start(){
        camera.enabled = false;
    }
    void Update(){
        SceneManager.activeSceneChanged += SceneChanged;
    }
    private void SceneChanged(Scene current, Scene next) {
        Debug.Log("SceneChanged");
        if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame"))
        {
            // Set active if in game scene
            if (isLocalPlayer)
            {
                camera.enabled = true;
            }
        }
    }
}