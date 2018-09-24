using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHistory : MonoBehaviour {

  

}


[System.Serializable]
public class History
{
    public ChatArchive[] archive;
}

[System.Serializable]
public class ChatArchive
{
    public string id;
    public List<MessageArj> messages = new List<MessageArj>();
  
     
}

[System.Serializable]
public class MessageArj
{
    public string time;
    public string body;
    public bool SentByMe;
}


