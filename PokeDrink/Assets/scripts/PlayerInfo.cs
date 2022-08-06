using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;

public class PlayerInfo : NetworkBehaviour
{
    // Player Name
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))]
    public string playerName;

    [SerializeField]
    private Text playerNameText;

    // Player Color
    [SyncVar(hook = nameof(HandlePlayerColorUpdate))]
    public Color playerColor;

    [SerializeField]
    private Image playerColorImage;
    private Color[] playerColors =
    {
        Color.red,
        Color.cyan,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta
    };

    // Player Ready Status
    [SyncVar(hook = nameof(HandlePlayerReadyStatusUpdate))]
    public bool playerReadyStatus;

    [SerializeField]
    private Button playerReadyButton;
    private Image playerReadyButtonImage;

    // Game State
    [SerializeField]
    private Text gameStateText;
    public GameManager gameManager;

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        playerReadyButtonImage = playerReadyButton.GetComponent<Image>();
        // Assign players color based on their ID, once more players than colors, assign a random color
        if (connectionToClient.connectionId < playerColors.Length)
        {
            CmdSetPlayerColor(playerColors[connectionToClient.connectionId]);
        }
        else
        {
            CmdSetPlayerColor(playerColors[Random.Range(0, playerColors.Length)]);
        }
        UpdateGameStateUI(GameManager.GameState.Movement);
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

    // End Player Name

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

    // End Player Color

    // Player Ready Status
    public void ChangePlayerReadyStatus()
    {
        if (hasAuthority)
        {
            // Change status to false
            if (playerReadyStatus)
            {
                CmdChangePlayerReadyStatus(false);
                if (gameManager == null)
                {
                    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                }
                gameManager.areAllPlayersReady = false;
            }
            else // change status to true
            {
                CmdChangePlayerReadyStatus(true);
            }
        }
    }

    [Command]
    private void CmdChangePlayerReadyStatus(bool playerReadyStatus)
    {
        Debug.Log(
            "CmdChangePlayerReadyStatus: Changing player ready status to: " + playerReadyStatus
        );
        this.HandlePlayerReadyStatusUpdate(this.playerReadyStatus, playerReadyStatus);
    }

    public void HandlePlayerReadyStatusUpdate(bool oldValue, bool newValue)
    {
        Debug.Log("Player ready status changed from: " + oldValue + " to: " + newValue);
        if (isServer)
        {
            this.playerReadyStatus = newValue;
        }
        if (hasAuthority)
        {
            this.playerReadyButtonImage.color = playerReadyStatus ? Color.green : Color.red;
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        gameManager.CheckIfAllPlayersAreReady();
    }

    // End Player Ready Status
    public void UpdateGameStateUI(GameManager.GameState gameState)
    {
        if (hasAuthority)
        {
            if (gameState == GameManager.GameState.Movement)
            {
                gameStateText.text = "Movement";
            }
            else if (gameState == GameManager.GameState.Catch)
            {
                gameStateText.text = "Catch";
            }
            // CmdChangePlayerReadyStatus(false);
        }
    }
}
