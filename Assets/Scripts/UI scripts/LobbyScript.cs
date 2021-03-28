﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
    public struct Server
    {
        public string name;
        public string pass;
        public string ID;
        public bool isPrivate;
        public bool isJoining;

    }
    public int playerID;
    //playerID = 
    /*
     if 
     */

    public Server toJoin;
    public Server toHost;

    //Hosting
    public Toggle t_IsPriv;
    public TMP_InputField IF_LobbyName;
    public TMP_InputField IF_LobbyPass;

    public TMP_Text t_LobbyN;

    //Joining:
    public Server tempServer;
    public TMP_InputField IF_JoinPass;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartServer()
    {

        toHost.name = IF_LobbyName.text;
        toHost.pass = IF_LobbyPass.text;
        toHost.isPrivate = t_IsPriv.isOn;
        toHost.isJoining = false;

        t_LobbyN.text = IF_LobbyName.text;

    }

    public void JoinServer()
    {
        toJoin.name = tempServer.name;
        toJoin.ID = tempServer.ID;
        toJoin.pass = IF_LobbyPass.text;
        toJoin.isPrivate = tempServer.isPrivate;
        toJoin.isJoining = true;


    }
}
