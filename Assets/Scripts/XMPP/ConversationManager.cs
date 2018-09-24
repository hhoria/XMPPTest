using Matrix.Xmpp.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class ConversationManager : MonoBehaviour {


    public Text username;
    private XmppClient cli;
    private Message currentMessage;

    public Transform messageParent;
    public GameObject messageTemplate;
    public InputField messageContent;

    private bool pendingMessage = false, sendingMessage = false;
    private List<GameObject> messages;
    public List<MessageArj> messageArchive;
    public string conversationID;
    public ChatArchive CA;

    public ConnectionManager CM;

    public void PopulateHeader(string _username) 
    {
        username.text = _username;
        if (messages == null)
        {
            messages = new List<GameObject>();
        }
        if (messageArchive == null)
        {
            messageArchive = new List<MessageArj>();
        }
        foreach(GameObject go in messages)
        {
            Destroy(go);

        }
        messages.Clear();
    }

    public void Activate()
    {
        cli = Statics.mainClient;

        cli.OnMessage += OnMessage;
        if (!File.Exists(Application.persistentDataPath + "/" + conversationID))
        {
            File.Create(Application.persistentDataPath + "/"+ conversationID);
            CA = new ChatArchive();
        }
        else
        {
            var file = File.ReadAllBytes(Application.persistentDataPath + "/" + conversationID);
            string json = Encoding.ASCII.GetString(file);
            CA = JsonUtility.FromJson<ChatArchive>(json);
        }
        foreach (MessageArj marj in CA.messages)
        {
            GameObject go = GameObject.Instantiate(messageTemplate, messageParent);
            go.SetActive(true);
            messages.Add(go);

            MessageTemplate mt = go.GetComponent<MessageTemplate>();
            mt.cm = this;

            mt.isSender = marj.SentByMe;

            mt.SendMessageXMPP(marj.body);

        }

    }

    public void OnMessage(object sender, MessageEventArgs e)
    {
        currentMessage = e.Message;
        pendingMessage = true;
    }

    #region Unity

    private void Update()
    {
        if (pendingMessage && currentMessage!=null) 
        {
            if (!string.IsNullOrEmpty(currentMessage.Body))
            {
                GameObject go = GameObject.Instantiate(messageTemplate, messageParent);
                go.SetActive(true);
                messages.Add(go);

                MessageArj marj = new MessageArj();
                marj.body = currentMessage.Body;
                marj.time = DateTime.Now.ToString();
                marj.SentByMe = false;
                messageArchive.Add(marj);

                CA.id = conversationID;
                CA.messages.Add(marj);


                MessageTemplate mt = go.GetComponent<MessageTemplate>();
                mt.cm = this;
                mt.isSender = false;
                mt.ReceiveMessageXMPP(currentMessage.Body);

                currentMessage = null;
                pendingMessage = false;
            }
        }
    }

    public void SendMessageXMPP()
    {
        if (!sendingMessage)
        {
            sendingMessage = true;
            string message = messageContent.text;
            Message msg = new Message
            {
                Type = Matrix.Xmpp.MessageType.Chat,
                To = conversationID,
                Body = message
            };
            cli.Send(msg);


            MessageArj marj = new MessageArj();
            marj.body = message;
            marj.time = DateTime.Now.ToString();
            marj.SentByMe = true;
            messageArchive.Add(marj);

            CA.id = conversationID;
            CA.messages.Add(marj);

            GameObject go = GameObject.Instantiate(messageTemplate, messageParent);
            go.SetActive(true);
            messages.Add(go);
            MessageTemplate mt = go.GetComponent<MessageTemplate>();
            mt.isSender = true;
            mt.cm = this;
            mt.SendMessageXMPP(message);
            sendingMessage = false;
        }
    }


    public void SaveConversation()
    {
        string json=JsonUtility.ToJson(CA);
        byte[] jsonBytes=System.Text.Encoding.UTF8.GetBytes(json);
        File.WriteAllBytes(Application.persistentDataPath + "/" + conversationID,jsonBytes);
    }

    #endregion



}
