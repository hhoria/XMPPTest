# XMPPTest
XMPPTest


Before starting anything, make sure that OpenFire is up and running on the local machine. Add a few users to OpenFire and make sure they are buddies in-between. (user1 is buddy with user 2 and vice-versa). You can also install locally another XMPP client such as pidgin or jitsi.
Login on the client with one of the accounts setup on OpenFire.
Inside the unity editor, launch Scene1.unity, located in /Scenes/Scene1.unity. This is the main scene that will launch the app, with the following workflow:
	Login (Username, Domain, Password and an optional, status)
		Username: the username for the account setup on OpenFire.
		Domain: if OpenFire is setup on the same machine as the unity editor, you can use localhost. It works for any public XMPP server (we've tried it on our local network).
		Password: The password for the current account.
		Status (Optional): The status of the curent user, this is not mandatory.
	
After entering the credentials, the app will connect to the XMPP server and authenticate. Should the user not be authorized, (wrong password, in-existing user-name etc), a native mobile popup will appear (only works on mobile, on the Unity Editor, you will just receive an error in the console "Not authorized"). Having proper credentials inputed, the app will take you to the next screen, which is the buddy list.
Your friends list is populated from the XMPP server. You can see which user is online, by the green dot next to the user's icon. Tapping any user from the friend list will open the chat window. All the conversations are saved as a text file, per user (10 conversations with 10 different friends will have 10 text files saved in the application's Persistent data folder). 

As a project structure, In the Scene1.scene you will find a GameObject XMPP with several children Login, BuddiesList and Conversation.
XMPP has the main Manager which in turn uses the scripts placed on Login, BuddiesList and Conversation. In LoginXMPP.cs script, there is also the Matrix SDK license activation. The license is stored in a text file and loaded at runtime (the 30 days trial is valid until 24th Oct 2018).

