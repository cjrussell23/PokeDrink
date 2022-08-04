using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour{
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private Text chatText = null;
    [SerializeField] private InputField inputField = null;
    private static event Action<string> OnMessage;
    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);
        OnMessage += HandleNewMessage;
    }
    [ClientCallback]
    private void OnDestroy() {
        if(!hasAuthority){return;}
        OnMessage -= HandleNewMessage;    
    }
    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }
    [Client]
    public void Send(string message){
        if (!Input.GetKeyDown(KeyCode.Return)){return;}
        if (string.IsNullOrWhiteSpace(message)){return;}
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }
    [Command]
    private void CmdSendMessage(string message){
        RpcHandleMessage($"[{connectionToClient.connectionId}] {message}");
    }
    [ClientRpc]
    private void RpcHandleMessage(string message){
        OnMessage?.Invoke($"\n{message}");
    }
}