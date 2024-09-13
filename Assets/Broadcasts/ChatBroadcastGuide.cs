using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class ChatBroadcastGuide : MonoBehaviour
{
    public Transform chatHolder;
    public GameObject msgElement;
    public TMP_InputField playerUsername, playerMessage;

    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageRecieved);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnclientMessageRecieved);

    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageRecieved);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnclientMessageRecieved);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    private void SendMessage()
    {
        Message msg = new Message()
        {
            username = playerUsername.text,
            message = playerMessage.text
        };


        if (InstanceFinder.IsServer)
            InstanceFinder.ServerManager.Broadcast(msg);

        if (InstanceFinder.IsClient)
            InstanceFinder.ClientManager.Broadcast(msg);
    }

    private void OnMessageRecieved(Message msg, Channel channel)
    {
        GameObject finalMessage = Instantiate(msgElement, chatHolder);
        finalMessage.GetComponent<TextMeshProUGUI>().text = msg.username + ": " + msg.message;
    }

    private void OnclientMessageRecieved(NetworkConnection networkConnection, Message msg, Channel channel)
    {
        InstanceFinder.ServerManager.Broadcast(msg);
    }
    public struct Message : IBroadcast
    {
        public string username;
        public string message;
    }

}
