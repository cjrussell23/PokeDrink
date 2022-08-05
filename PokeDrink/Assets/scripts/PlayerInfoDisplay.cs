using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerInfoDisplay : NetworkBehaviour
{
    // Player Name
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))] public string playerName;
    [SerializeField] private Text playerNameText;
    // Player Color
    [SyncVar(hook = nameof(HandlePlayerColorUpdate))] public Color playerColor;
    [SerializeField] private Image playerColorImage;
    private Color[] playerColors = { Color.red, Color.cyan, Color.blue, Color.green, Color.yellow, Color.magenta };
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        // Assign players color based on their ID, once more players than colors, assign a random color
        if (connectionToClient.connectionId < playerColors.Length){
            CmdSetPlayerColor(playerColors[connectionToClient.connectionId]);
        }
        else {
            CmdSetPlayerColor(playerColors[Random.Range(0, playerColors.Length)]);
        }
        
    }
    // Player Name
    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        Debug.Log("CmdSetPlayerName: Setting player name to: " + playerName);
        this.HandlePlayerNameUpdate(this.playerName, playerName);
    }
    public void HandlePlayerNameUpdate(string oldValue, string newValue)
    {
        Debug.Log("Player name has been updated for: " + oldValue + " to new value: " + newValue);
        if (isServer)
        {   
            this.playerName = newValue;
        }
        this.playerNameText.text = playerName;  
    }
    // Player Color
    [Command]
    private void CmdSetPlayerColor(Color playerColor)
    {
        Debug.Log("CmdSetPlayerColor: Setting player color to: " + playerColor);
        this.HandlePlayerColorUpdate(this.playerColor, playerColor);
    }
    public void HandlePlayerColorUpdate(Color oldValue, Color newValue)
    {
        Debug.Log("Player color has been updated for: " + oldValue + " to new value: " + newValue);
        if (isServer)
        {
            this.playerColor = newValue;
        }
        this.playerColorImage.color = playerColor;
    }
}
