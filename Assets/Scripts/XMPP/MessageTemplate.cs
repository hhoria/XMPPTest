using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageTemplate : MonoBehaviour {

    public bool isSender = true;
    public Text messageText;
    public ConversationManager cm;

    public void SendMessageXMPP(string message)
    {
        if (isSender) {
            messageText.alignment = TextAnchor.MiddleRight;
        }else
        {
            messageText.alignment = TextAnchor.MiddleLeft;
        }
        messageText.text = message;
    }

    public void ReceiveMessageXMPP(string message)
    {
        if (isSender)
        {
            messageText.alignment = TextAnchor.MiddleRight;
        }
        else
        {
            messageText.alignment = TextAnchor.MiddleLeft;
        }
        messageText.text = message;

    }
}
