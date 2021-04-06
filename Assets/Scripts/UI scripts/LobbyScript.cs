using System.Collections;
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

    //LOBBY CODE:
    public int i_TotRed;
    public int i_TotBlue;
    public int i_CurrPlacement;
    public bool b_Ready = false;
    public bool b_Change = false;

    public TMP_Text tmp_R1;
    public TMP_Text tmp_R2;
    public TMP_Text tmp_B1;
    public TMP_Text tmp_B2;

    public Toggle tog_R1;
    public Toggle tog_R2;
    public Toggle tog_B1;
    public Toggle tog_B2;

    public string s_Default = "Waiting for player..";

    //Probably a temp variable.
    public int i_CurrTeam;



    // Start is called before the first frame update
    void Start()
    {
        //Get the server data here.
        i_TotRed = 0;
        i_TotBlue = 0;

        if (i_TotRed >= 2)
        {
            i_CurrTeam = 1;
            i_TotBlue++;
            i_CurrPlacement = i_TotBlue;
            if(i_TotBlue <= 1)
            {
                tmp_B1.text = "test";
            }
            else
            {

                tmp_B2.text = "test";
            }
        }
        else
        {
            i_CurrTeam = 0;
            i_TotRed++;
            i_CurrPlacement = i_TotRed;
            if (i_TotRed <= 1)
            {
                tmp_R1.text = "test";
            }
            else
            {

                tmp_R2.text = "test";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (b_Change)
        {
            switch (i_CurrTeam)
            {
                case 0:
                    if (i_CurrPlacement > i_TotRed)
                    {
                        i_CurrPlacement = i_TotRed;
                        tmp_R1.text = "test";
                        tmp_R2.text = s_Default;
                    }
                    break;
                case 1:
                    if (i_CurrPlacement > i_TotBlue)
                    {
                        i_CurrPlacement = i_TotBlue;
                        tmp_B1.text = "test";
                        tmp_B2.text = s_Default;
                    }
                    break;
            }

            b_Change = false;
        }

    }

    public void Ready()
    {
        //Find out if I should make this code!!!
        switch(b_Ready)
        {
            case false:
                b_Ready = true;
                break;
            case true:
                b_Ready = false;
                break;
        }

        switch (i_CurrTeam)
        {
            case 0:
                switch (i_CurrPlacement)
                {
                    case 1:
                        tog_R1.isOn = b_Ready;
                        break;
                    case 2:
                        tog_R2.isOn = b_Ready;
                        break;
                }
                break;
            case 1:
                switch (i_CurrPlacement)
                {
                    case 1:
                        tog_B1.isOn = b_Ready;
                        break;
                    case 2:
                        tog_B2.isOn = b_Ready;
                        break;
                }
                break;
        }
    }

    public void UpdateThisText(string s_input)
    {
        switch(i_CurrTeam)
        {
            case 0:
                switch (i_CurrPlacement)
                {
                    case 1:
                        tmp_R1.text = s_input;
                        break;
                    case 2:
                        tmp_R2.text = s_input;
                        break;
                }
                break;
            case 1:
                switch (i_CurrPlacement)
                {
                    case 1:
                        tmp_B1.text = s_input;
                        break;
                    case 2:
                        tmp_B2.text = s_input;
                        break;
                }
                break;
        }
    }


    public void ChangeTeam()
    {
        if (b_Ready)
            Ready();

        UpdateThisText(s_Default);

        switch (i_CurrTeam)
        {
            case 0:
                i_CurrTeam = 1;
                i_TotRed--;
                i_TotBlue++;
                i_CurrPlacement = i_TotBlue;
                b_Change = true;
                break;
            case 1:
                i_CurrTeam = 0;
                i_TotRed++;
                i_TotBlue--;
                i_CurrPlacement = i_TotRed;
                b_Change = true;
                break;
        }
        UpdateThisText("test");
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
