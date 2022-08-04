using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MessageController : MonoBehaviour
{
    public int maxMessages = 25;
    [SerializeField] private List<Message> messageList = new List<Message>();
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject textObject;
    [SerializeField] private InputField messageBox;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(messageBox.text != ""){
            if (Input.GetKeyDown(KeyCode.Return)){
                SendMessageToChat(messageBox.text);
                messageBox.text = "";
            }
        }
        else if (!messageBox.isFocused && Input.GetKeyDown(KeyCode.Return)){
            messageBox.ActivateInputField();
        }
    }
    public void SendMessageToChat(string text){
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.RemoveAt(0);
        }
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = text;
        messageList.Add(newMessage);
    }
    [System.Serializable]
    public class Message{
        public string text;
        public Text textObject;
    }
}
