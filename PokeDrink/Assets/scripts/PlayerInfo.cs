using System.Collections;
using System.Collections.Generic;
using System;
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

    [SerializeField]
    private Text playerNameTextMap;

    // Player Color
    [SyncVar(hook = nameof(HandlePlayerColorUpdate))]
    public Color playerColor;

    [SerializeField]
    private Image playerColorImage;

    [SerializeField]
    private Image playerColorImageMap;
    private Color[] playerColors =
    {
        Color.red,
        Color.green,
        Color.cyan,
        Color.yellow,
        Color.grey,
        Color.white
    };
    public Color[] PlayerColors
    {
        get { return playerColors; }
    }

    // Player Ready State
    [SyncVar(hook = nameof(HandlePlayerReadyStateUpdate))]
    public bool playerReadyState;

    [SerializeField]
    private Button playerReadyButton;
    private Image playerReadyButtonImage;
    private Text playerReadyButtonText;

    // Game State
    [SerializeField]
    private Text gameStateText;
    public Image gameStateImage;
    public GameManager gameManager;
    private CatchPhase catchPhase;
    private Dice dice;

    // Events
    public int chanceOfEvent;
    private ChatManager chatManager;
    private GamePlayer gamePlayer;
    private static event Action<string> OnBattleRequest;

    public override void OnStartAuthority()
    {
        OnBattleRequest += HandleBattleRequest;
        gamePlayer = GetComponent<GamePlayer>();
        dice = GetComponent<Dice>();
        chatManager = GetComponent<ChatManager>();
        chanceOfEvent = 25;
        catchPhase = GetComponent<CatchPhase>();
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        playerReadyButtonImage = playerReadyButton.GetComponent<Image>();
        playerReadyButtonText = playerReadyButton.GetComponentInChildren<Text>();
        // Assign players color based on their ID, once more players than colors, assign a random color
        int connectionId = GetComponent<GamePlayer>().ConnectionId;
        if (connectionId < playerColors.Length)
        {
            Debug.Log(
                "Assigining color "
                    + playerColors[connectionId]
                    + " to player "
                    + connectionId
                    + " "
                    + SteamFriends.GetPersonaName()
            );
            CmdSetPlayerColor(playerColors[connectionId]);
        }
        else
        {
            CmdSetPlayerColor(playerColors[UnityEngine.Random.Range(0, playerColors.Length)]);
        }
    }
    public void HandleBattleRequest(string playerName){
        if (playerName == gamePlayer.playerName){
            dice.ResetRolls(1);
            dice.SetPlayerBattle(true);
            Debug.Log("In battle");
        }
    }
    [Command]
    public void CmdSendBattleRequest(string playerName){
        OnBattleRequest(playerName);
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
        this.playerNameTextMap.text = playerName.Substring(0, 1).ToUpper();
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
                catchPhase.DisableCatchUI();
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
            this.playerReadyButtonText.text = playerReadyState
                ? "Ready, waiting on other players"
                : "Not Ready, press SPACE to ready up";
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
            dice.ClearDiceText();
            if (gameState == GameManager.GameState.Movement)
            {
                gameStateText.text = "Roll the Dice to move!";
                gameStateImage.gameObject.SetActive(true);
            }
            if (gameState == GameManager.GameState.Catch)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (catchPhase.inGrass || catchPhase.inBattle){
                    gamePlayer.CmdSetPlayerAvailableForPlayerBattle(false);
                }
                else{
                    gamePlayer.CmdSetPlayerAvailableForPlayerBattle(true);
                }
                if (catchPhase.inGrass)
                {
                    gameStateText.text =
                        "Roll the Dice to catch the Pokemon!\n(SPACE) to run away.";
                    gameStateImage.gameObject.SetActive(true);
                }
                else if (catchPhase.inBattle)
                {
                    gameStateText.text = "Roll the Dice to attack!";
                    gameStateImage.gameObject.SetActive(true);
                }
                // Random event chance of %25
                else if (random <= chanceOfEvent)
                {
                    // Random event
                    int randomEvent = UnityEngine.Random.Range(0, 5);
                    // int randomEvent = 4;
                    switch (randomEvent)
                    {
                        case 0:
                            chatManager.CmdSendMessage(
                                "Stubbed thier toe. They must kiss it better, or drink to numb the pain."
                            );
                            break;
                        case 1:
                            chatManager.CmdSendMessage(
                                "is given an egg by a mysterious man... They must hold it close until it hatches."
                            );
                            break;
                        case 2:
                            chatManager.CmdSendMessage(
                                "!! YOUR EGG HATCHED !! If you don't have an egg, drink as you ponder why..."
                            );
                            break;
                        case 3:
                            chatManager.CmdSendMessage(
                                "A wild Drowzee used confusion!?!? You must can only talk in one syllable words till you enounter another Drowzee to remove your trance."
                            );
                            break;
                        case 4:
                            CheckForPlayerBattle();
                            break;
                    }
                }
                else
                {
                    StartCoroutine(AutoReadyPlayer());
                }
            }
        }
    }

    private void CheckForPlayerBattle()
    {
        foreach (GamePlayer player in Game.GamePlayers)
        {
            if (player != this.gameObject.GetComponent<GamePlayer>())
            {
                if (player.availableForPlayerBattle)
                {
                    chatManager.CmdSendMessage(
                        "is challenging " + player.playerName + " to a battle!"
                    );
                    // Give each player a roll
                    dice.ResetRolls(1);
                    dice.SetPlayerBattle(true);
                    CmdSendBattleRequest(player.playerName);
                    return;
                }
            }
        }
        chatManager.CmdSendMessage(
            "Wanted to battle; but no one was available. They must drink in loneliness because they have no friends."
        );
    }
    private IEnumerator AutoReadyPlayer()
    {
        yield return new WaitForSeconds(1);
        ChangePlayerReadyState();
    }

    public void ToggleGameStateImage(bool toggle)
    {
        gameStateImage.gameObject.SetActive(toggle);
    }
}
