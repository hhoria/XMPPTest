using Matrix.Xmpp.Client;
using Matrix.Xmpp.Roster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuddyListManager : MonoBehaviour {

    public GameObject buddyTemplate;
    public List<GameObject> buddiesGameObjects;
    public List<RosterItem> buddies;
    public Transform buddyListParent;

    private GameObject buddyToMarkOnline;
    private bool markOnline=false;

    public bool readyToPopulate=false; 

    public void PupulateBuddies()
    {
        if (buddies == null)
        {
            buddies = new List<RosterItem>(); 
        }
        buddies.Clear();
        XmppClient cliCache = Statics.mainClient;
        cliCache.OnRosterStart += OnRosterStart;
        cliCache.OnRosterEnd += OnRosterEnd;
        cliCache.OnRosterItem += OnRosterItem;
        cliCache.OnPresence += OnPresence;
    }

    private void OnPresence(object sender, PresenceEventArgs e)
    {
        Presence pre = e.Presence;
        ///from
        ///
        int c = 0;
        string from = pre.From;
        foreach (RosterItem ri in buddies)
        {
            if (ri.Jid.ToString() == from)
            {
                bool presence = false;
                if(pre.Type!= Matrix.Xmpp.PresenceType.Unavailable)
                {
                    presence = true;
                }
                buddyToMarkOnline = buddiesGameObjects[c];
                // buddiesGameObjects[c].GetComponent<BuddyTemplate>().SetPresence(presence);
                markOnline = true;
                return;
            }
            c++;
        }
    }

    private void OnRosterItem(object sender, Matrix.Xmpp.Roster.RosterEventArgs e)
    {
        Debug.LogWarning("RosterItem:" + e.RosterItem.Jid.User);
        buddies.Add(e.RosterItem);
    }

    private void OnRosterEnd(object sender, Matrix.EventArgs e) 
    {

        Debug.LogWarning("RosterEnd; ready to populate:" + e.State);
        readyToPopulate = true;
       
    }

    private void OnRosterStart(object sender, Matrix.EventArgs e)
    {
        Debug.LogWarning("RosterStart:" + e.State);
    }

    #region Unity
    private void Update()
    {

        
        if(buddyToMarkOnline!=null && markOnline)
        {
            buddyToMarkOnline.GetComponent<BuddyTemplate>().SetPresence(true);
            markOnline = false;
            buddyToMarkOnline = null;
        }


        if (readyToPopulate)
        {
            if (buddies.Count > 0)
            {
                foreach (GameObject go in buddiesGameObjects)
                {
                    Destroy(go);
                }
                buddiesGameObjects.Clear();

            }
            foreach (RosterItem ri in buddies)
            {

                GameObject Go = GameObject.Instantiate(buddyTemplate, buddyListParent);
                BuddyTemplate bt = Go.GetComponent<BuddyTemplate>();
                bt.PopulateBuddy(null, ri.Name, ri.BaseUri, ri.Value,ri.Jid);
                Go.SetActive(true);
                buddiesGameObjects.Add(Go);
            }
            readyToPopulate = false;
        }
    }
    #endregion
}
