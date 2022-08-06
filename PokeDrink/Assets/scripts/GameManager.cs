using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GameManager : NetworkBehaviour
{
    public enum GameState {
        Movement,
        Catch
    }
    [SyncVar] public bool areAllPlayersReady = false;
    [SyncVar(hook = nameof(HandleGameStateUpdate))] public GameState currentGameState;
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
    void Awake()
    {
        currentGameState = GameState.Movement;
    }
    public void CheckIfAllPlayersAreReady()
    {
        Debug.Log("Executing CheckIfAllPlayersAreReady");
        areAllPlayersReady = false;
        foreach (GamePlayer player in Game.GamePlayers)
        {
            if (player.gameObject.GetComponent<PlayerInfo>().playerReadyStatus == true)
            {
                areAllPlayersReady = true;
            }
            else
            {
                Debug.Log("CheckIfAllPlayersAreReady: Not all players are ready. Waiting for: " + player.playerName);
                areAllPlayersReady = false;
                break;
            }
        }
        if (areAllPlayersReady)
        {
            Debug.Log("All players are ready");
            NextGameState();
        }
    }
    private void NextGameState(){
        switch (currentGameState){
            case GameState.Movement:
                Debug.Log("Executing NextGameState: Movement");
                CmdChangeGameState(GameState.Catch);
                break;
            case GameState.Catch:
                Debug.Log("Executing NextGameState: Catch");
                CmdChangeGameState(GameState.Movement);
                break;
        }
    }
    public void HandleGameStateUpdate(GameState oldValue, GameState newValue){
        Debug.Log("GameState changed from " + oldValue + " to " + newValue);
        if (isServer){
            this.currentGameState = newValue;
        }
        GameObject.Find("LocalGamePlayer").GetComponent<PlayerInfo>().UpdateGameStateUI(newValue);
    }
    [Command(requiresAuthority = false)]
    public void CmdChangeGameState(GameState newGameState){
        Debug.Log("Executing CmdChangeGameState");
        this.currentGameState = newGameState;
    }
}
