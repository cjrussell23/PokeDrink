using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour {
    private Camera camera;
    public void Start(){
        camera = GetComponentInChildren<Camera>();
        camera.enabled = false;
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
                if (camera == null)
                {
                    camera = GetComponentInChildren<Camera>();
                }
                camera.enabled = true;
            }
        }
    }
}