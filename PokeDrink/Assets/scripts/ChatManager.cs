using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Steamworks;
using UnityEngine.SceneManagement;

public class ChatManager : NetworkBehaviour{
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private GameObject chatPanel = null;
    [SerializeField] private GameObject messagePrefab = null;
    [SerializeField] private List<Message> messageList = new List<Message>();
    [SerializeField] private GameObject playerMapImage;
    private static event Action<string> OnMessage;
    private Color[] playerColors;
    public void Start(){
        chatUI.SetActive(false);
        playerMapImage.SetActive(false);
        playerColors = GetComponent<PlayerInfo>().PlayerColors;
    }
    public override void OnStartAuthority()
    {
        OnMessage += HandleNewMessage;
    }
    private void Update() {
        SceneManager.activeSceneChanged += SceneChanged;
        if (inputField.isFocused)
        {
            gameObject.GetComponent<GridMovement>().CanMove = false;
        }
        else if (!inputField.isFocused && gameObject.GetComponent<GridMovement>().CanMove == false)
        {
            gameObject.GetComponent<GridMovement>().CanMove = true;
        }
    }
    private void SceneChanged(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().name.Equals("Scene_SteamworksGame"))
        {
            playerMapImage.SetActive(true);
            if (hasAuthority)
            {
                chatUI.SetActive(true);
                
            }
            else
            {
                chatUI.SetActive(false);
            }
        }
    }
    [ClientCallback]
    private void OnDestroy() {
        if(!hasAuthority){return;}
        OnMessage -= HandleNewMessage;    
    }
    private void HandleNewMessage(string message)
    {
        Debug.Log("Received message: " + message);
        // First character is the color identifier
        // Split the message into the color and the actual message
        string messageColor = message.Substring(0, 1);
        string messageText = message.Substring(1);
        Color color;
        switch (messageColor)
        {
            case "0":
                color = playerColors[0];
                break;
            case "1":
                color = playerColors[1];
                break;
            case "2":
                color = playerColors[2];
                break;
            case "3":
                color = playerColors[3];
                break;
            case "4":
                color = playerColors[4];
                break;
            case "5":
                color = playerColors[5];
                break;
            default:
                color = Color.white;
                break;
        }
        Message newMessage = new Message();
        newMessage.text = messageText;
        newMessage.color = color;
        GameObject newText = Instantiate(messagePrefab, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = newMessage.color;
        messageList.Add(newMessage);
    }
    [Client]
    public void Send(string message){
        if (!Input.GetKeyDown(KeyCode.Return)){return;}
        if (string.IsNullOrWhiteSpace(inputField.text)){return;}
        Debug.Log("Sending message: " + inputField.text);
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }
    [Command]
    public void CmdSendMessage(string message){    
        PlayerInfo playerInfo = gameObject.GetComponent<PlayerInfo>();
        Color color = playerInfo.playerColor;
        Debug.Log("Set player color to: " + color);
        int colorId;
        if (color.Equals(playerColors[0])){
            colorId = 0;
        }
        else if (color.Equals(playerColors[1])){
            colorId = 1;
        }
        else if (color.Equals(playerColors[2])){
            colorId = 2;
        }
        else if (color.Equals(playerColors[3])){
            colorId = 3;
        }
        else if (color.Equals(playerColors[4])){
            colorId = 4;
        }
        else if (color.Equals(playerColors[5])){
            colorId = 5;
        }
        else{
            colorId = 6;
        }
        RpcHandleMessage($"{colorId}[{playerInfo.playerName}] {message}");
    }
    [ClientRpc]
    private void RpcHandleMessage(string message){
        OnMessage?.Invoke(message);
    }
    [System.Serializable]
    public class Message{
        public string text;
        public Text textObject;
        public Color color;
    }
    public void localPlayerMessage(string message){
        if (hasAuthority) {
            HandleNewMessage("6" + message);
        }
    }
}
