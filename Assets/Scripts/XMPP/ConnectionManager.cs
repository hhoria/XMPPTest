using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matrix;
using Matrix.Xmpp.Client;
using TheNextFlow.UnityPlugins;
using System;
using System.IO;
using System.Text;

public class ConnectionManager : MonoBehaviour {

    #region publics
    [Header("Public Scripts and GameObjects")]
    public LoginXMPP login;
    public BuddyListManager buddyListManager;

    public GameObject LoginPanel, BuddiesPanel;

    [Header("public vars")]
    public bool readyToOpen = false;


    public History his = new History();

    #endregion




    #region xmpp
    private XmppClient cli = new XmppClient();
    #endregion
    
    public void Connect()
    {
        cli.SetUsername(login.username);
        cli.SetXmppDomain(login.domain);
        cli.Password = login.password;
        cli.Status = login.status;

        //Log events

        cli.OnReceiveXml += new System.EventHandler<TextEventArgs>(XmppClientOnReceiveXML);
        cli.OnSendXml += new System.EventHandler<TextEventArgs>(XmppClientOnSendXML);
        cli.OnLogin += new System.EventHandler<Matrix.EventArgs>(XmppLogin);
        cli.OnAuthError += new System.EventHandler<Matrix.Xmpp.Sasl.SaslEventArgs>(XmppClientOnAuthErrorXML);
        cli.Open();

        cli.Show = Matrix.Xmpp.Show.Chat;

        Statics.mainClient = cli;

        Debug.Log("Chat history path:" + Application.persistentDataPath + "/history");

       

    }

    public void Disconnect()
    {
        cli.Close();
    }

    private void XmppLogin(object sender, Matrix.EventArgs e)
    {
        Debug.Log("Login:" + e.State);
        readyToOpen = true;
    }

    private void XmppClientOnSendXML(object sender, TextEventArgs e)
    {
        Debug.Log("SEND:" + e.Text);
    }


    private void XmppClientOnAuthErrorXML(object sender, Matrix.Xmpp.Sasl.SaslEventArgs e)
    {
        Debug.Log("Error:" + e.Error.Text);
    }

    private void XmppClientOnReceiveXML(object sender, TextEventArgs e)
    {
        Debug.Log("RECV:" + e.Text);
        if (CheckForErrors(e.Text))
        {
           
        }
    }

    #region Unity


    private void Update()
    {
        if (readyToOpen)
        {
            SwitchToBuddyList();
            readyToOpen = false;
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }


    #endregion

    private void SwitchToBuddyList()
    {
        BuddiesPanel.SetActive(true);
        LoginPanel.SetActive(false);
        buddyListManager.PupulateBuddies();
    }


    #region ErrorHandling

    public bool CheckForErrors(string response)
    {
        bool ok = true;
        if (response.Contains("not-authorized"))
        {
            Debug.LogError("Not authorized");
            MobileNativePopups.OpenAlertDialog("Login Error", "The login credentials are incorrect", "OK", () =>
            {
                Debug.LogWarning("Incorrect login prompt dismiss");
            });
            Disconnect();
            ok = false;
        }
        return ok;
    }

    #endregion

}
