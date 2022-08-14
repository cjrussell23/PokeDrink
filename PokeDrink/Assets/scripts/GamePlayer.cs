using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Steamworks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayer : NetworkBehaviour
{
    [Header("GamePlayer Info")]
    [SyncVar(hook = nameof(HandlePlayerNameUpdate))] public string playerName;
    [SyncVar] public int ConnectionId;
    [SyncVar] public int playerNumber;
    [Header("Game Info")]
    [SyncVar] public bool IsGameLeader = false;
    [SyncVar(hook = nameof(HandlePlayerReadyStatusChange))] public bool isPlayerReady;
    [SyncVar] public ulong playerSteamId;
    [SyncVar(hook = nameof(HandlePlayerBadgeCountUpdate))] public int playerBadgeCount;
    [SyncVar] public bool availableForPlayerBattle;
    public SyncList<int> playerPokemon = new SyncList<int>();
    private PlayerInventoriesManager playerInventoriesManager;
    private PokemonController pokemonController;

    [Command]
    public void CmdSetPlayerAvailableForPlayerBattle(bool available)
    {
        availableForPlayerBattle = available;
    }
    public void HandlePlayerBadgeCountUpdate(int oldValue, int newValue)
    {
        playerBadgeCount = newValue;
        if (playerInventoriesManager != null)
        {
            playerInventoriesManager.UpdateBadges();
        }
    }
    [Command]
    public void CmdSetPlayerBadgeCount(int badgeCount)
    {
        playerBadgeCount = badgeCount;
    }
    [Command]
    public void CmdAddPokemon(int pokemonId)
    {
        playerPokemon.Add(pokemonId);
    }
    [Command]
    public void CmdSetPokemonAt(int index, int pokemonId)
    {
        playerPokemon[index] = pokemonId;
    }
    [Command]
    public void CmdRemovePokemonAt(int index)
    {
        playerPokemon.RemoveAt(index);
    }
    public int GetPlayerBadgeCount(){
        return playerBadgeCount;
    }
    public void HandlePlayerPokemonUpdate(SyncList<int>.Operation op, int index, int oldPokemonID, int newPokemonID)
    {
        Debug.Log("Player pokemon update: " + op + " at index: " + index);
        if (op == SyncList<int>.Operation.OP_ADD)
        {
            Debug.Log("Player pokemon added: " + newPokemonID);
        }
        else if (op == SyncList<int>.Operation.OP_CLEAR)
        {
            Debug.Log("Player pokemon cleared");
        }
        else if (op == SyncList<int>.Operation.OP_REMOVEAT)
        {
            Debug.Log("Player pokemon removed: " + oldPokemonID);
        }
        else if (op == SyncList<int>.Operation.OP_SET)
        {
            Debug.Log("Player pokemon set: " + newPokemonID);
        }
        if (playerInventoriesManager == null)
        {
            playerInventoriesManager = FindObjectOfType<PlayerInventoriesManager>();
        }
        playerInventoriesManager.UpdateInventories();
    }

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
    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyManager.instance.FindLocalGamePlayer();
        LobbyManager.instance.UpdateLobbyName();
    }
    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        Debug.Log("CmdSetPlayerName: Setting player name to: " + playerName);
        this.HandlePlayerNameUpdate(this.playerName, playerName);
    }
    public override void OnStartClient()
    {
        Game.GamePlayers.Add(this);
        playerPokemon.Callback += HandlePlayerPokemonUpdate;
        pokemonController = GameObject.Find("Pokemon").GetComponent<PokemonController>();
        LobbyManager.instance.UpdateLobbyName();
        LobbyManager.instance.UpdateUI();        
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandlePlayerNameUpdate(string oldValue, string newValue)
    {
        Debug.Log("Player name has been updated for: " + oldValue + " to new value: " + newValue);
        if (isServer)
            this.playerName = newValue;
        if (isClient)
        {
            LobbyManager.instance.UpdateUI();
        }

    }
    public void ChangeReadyStatus()
    {
        Debug.Log("Executing ChangeReadyStatus for player: " + this.playerName);
        if (hasAuthority)
            CmdChangePlayerReadyStatus();
    }
    [Command]
    void CmdChangePlayerReadyStatus()
    {
        Debug.Log("Executing CmdChangePlayerReadyStatus on the server for player: " + this.playerName);
        this.HandlePlayerReadyStatusChange(this.isPlayerReady, !this.isPlayerReady);
    }
    void HandlePlayerReadyStatusChange(bool oldValue, bool newValue)
    {
        if (isServer)
            this.isPlayerReady = newValue;
        if (isClient)
            LobbyManager.instance.UpdateUI();
    }
    public void CanLobbyStartGame()
    {
        if (hasAuthority)
            CmdCanLobbyStartGame();
    }
    [Command]
    void CmdCanLobbyStartGame()
    {
        Game.StartGame();
    }
    public void QuitLobby()
    {
        if (hasAuthority)
        {
            if (IsGameLeader)
            {
                Game.StopHost();
            }
            else
            {
                Game.StopClient();
            }
        }
    }
    private void OnDestroy()
    {
        if (hasAuthority)
        {
            LobbyManager.instance.DestroyPlayerListItems();
            SteamMatchmaking.LeaveLobby((CSteamID)LobbyManager.instance.currentLobbyId);
        }
        Debug.Log("LobbyPlayer destroyed. Returning to main menu.");
    }
    public override void OnStopClient()
    {
        Debug.Log(playerName + " is quiting the game.");
        Game.GamePlayers.Remove(this);
        Debug.Log("Removed player from the GamePlayer list: " + this.playerName);
        LobbyManager.instance.UpdateUI();
    }
}
