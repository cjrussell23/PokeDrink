using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerInfoDisplay : NetworkBehaviour
{
    [SyncVar(hook = nameof(NameUpdated))] private string playerName;
    [SyncVar(hook = nameof(ColorUpdated))] private Color color;
    [SerializeField] public Text displayNameText;
    [SerializeField] public Image nameColor;
    [SerializeField] public Canvas canvas;
    private Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow};
    private void NameUpdated(string oldName, string newName){
        displayNameText.text = playerName;
    }
    private void ColorUpdated(Color oldColor, Color newColor){
        nameColor.color = newColor;
    }
    void Start(){
        if(isLocalPlayer){
            
        }
    }
    void Update() {
        SceneManager.activeSceneChanged += SceneChanged;
    }
    public void SceneChanged(Scene current, Scene next) {
        if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame")) {
            if(isLocalPlayer)
            {
                UpdatePlayerDisplay();
                // canvas = gameObject.GetComponentInChildren<Canvas>();
                // Camera camera = GetComponentInChildren<Camera>();
                // canvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                // canvas.GetComponent<Canvas>().worldCamera = camera;
                // UpdatePlayerDisplay();
            }
        }
    }
    private void UpdatePlayerDisplay(){
        playerName = SteamFriends.GetFriendPersonaName(new CSteamID(SteamUser.GetSteamID().m_SteamID));
        color = colors[Random.Range(0, colors.Length)];
    }
}
