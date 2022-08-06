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
    [SerializeField] private Text chatText = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private GameObject chatPanel = null;
    [SerializeField] private GameObject messagePrefab = null;
    [SerializeField] private List<Message> messageList = new List<Message>();
    public Color playerColor = Color.white;
    private static event Action<string> OnMessage;
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
        Message newMessage = new Message();
        newMessage.text = message;
        GameObject newText = Instantiate(messagePrefab, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = message;
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
        RpcHandleMessage($"[{gameObject.GetComponent<PlayerInfo>().playerName}] {message}");
    }
    [ClientRpc]
    private void RpcHandleMessage(string message){
        OnMessage?.Invoke(message);
    }
    [System.Serializable]
    public class Message{
        public string text;
        public Text textObject;
    }
}
