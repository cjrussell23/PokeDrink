using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;

public class PlayerInfo : NetworkBehaviour
{
    private MyNetworkManager game;
    private MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    // Player Name
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))]
    public string playerName;

    [SerializeField]
    private Text playerNameText;
    [SerializeField] private Text playerNameTextMap;

    // Player Color
    [SyncVar(hook = nameof(HandlePlayerColorUpdate))]
    public Color playerColor;

    [SerializeField]
    private Image playerColorImage;
    [SerializeField] private Image playerColorImageMap;
    public Color[] playerColors =
    {
        Color.red,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        new Color(0, 0.8f, 1, 1) // light blue
    };

    // Player Ready State
    [SyncVar(hook = nameof(HandlePlayerReadyStateUpdate))]
    public bool playerReadyState;

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
        int connectionId = GetComponent<GamePlayer>().ConnectionId;
        if (connectionId < playerColors.Length)
        {
            CmdSetPlayerColor(playerColors[connectionId]);
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
        this.playerNameTextMap.text = playerName;
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
        this.playerColorImageMap.color = playerColor;
    }

    // End Player Color

    // Player Ready State
    public void ChangePlayerReadyState()
    {
        if (hasAuthority)
        {
            // Change State to false
            if (playerReadyState)
            {
                CmdChangePlayerReadyState(false);
                if (gameManager == null)
                {
                    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                }
                gameManager.areAllPlayersReady = false;
            }
            else // change State to true
            {
                CmdChangePlayerReadyState(true);
            }
        }
    }

    [Command]
    private void CmdChangePlayerReadyState(bool playerReadyState)
    {
        Debug.Log("CmdChangePlayerReadyState: Changing player ready State to: " + playerReadyState);
        this.HandlePlayerReadyStateUpdate(this.playerReadyState, playerReadyState);
    }

    public void HandlePlayerReadyStateUpdate(bool oldValue, bool newValue)
    {
        Debug.Log("Player ready State changed from: " + oldValue + " to: " + newValue);
        if (isServer)
        {
            this.playerReadyState = newValue;
        }
        if (hasAuthority)
        {
            this.playerReadyButtonImage.color = playerReadyState ? Color.green : Color.red;
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        gameManager.CheckIfAllPlayersAreReady();
    }

    // End Player Ready State
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
        }
    }
}
