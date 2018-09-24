using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuddyTemplate : MonoBehaviour {

    public Image profilePicImage, AvailableIco;
    public Text nicknameText, timeText, lastMessageText;
    public string buddyID = string.Empty;

    public GameObject buddyList, conversationPanel;
    public ConversationManager conversationManager;

   


    public void PopulateBuddy(Sprite profilePic, string nickname, string time, string lastMessage,string buddyId)
    {
        buddyID = buddyId;
     
        if (profilePic != null)
        {
            profilePicImage.sprite = profilePic;
        }
        if (!string.IsNullOrEmpty(nickname))
        {
            nicknameText.text = nickname;
        }
        if (!string.IsNullOrEmpty(time))
        {
            timeText.text = time;
        }

        if (!string.IsNullOrEmpty(lastMessage))
        {
            lastMessageText.text = lastMessage;
        }

      
    }


    public void SetPresence(bool presence)
    {
        if (presence)
        {
            AvailableIco.color = Color.green;
        }
        else
        {
            AvailableIco.color = Color.gray;
        }
    }

    public void OpenChat()
    {
        conversationPanel.SetActive(true);
        buddyList.SetActive(false);
        conversationManager.conversationID = buddyID;
        conversationManager.PopulateHeader(nicknameText.text);
        conversationManager.Activate();
    }
}
