using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
	public enum Team
	{
		Red, Blu
	}
	
	public struct LobbyClient
	{
		public int position;
		public string name;
		public bool b_ready;
	}

    public int playerID;
    //playerID = 
    /*
     if 
     */
    public string LobName;
    public string Playername;
    public string pass;
    public string ID;
    public bool isPrivate;
    public bool isJoining;
    //public Server toJoin;
    //public Server toHost;

    //Hosting
    public Toggle t_IsPriv;
    public TMP_InputField IF_LobbyName;
    //public TMP_InputField IF_PlayerName;
    public TMP_InputField IF_LobbyPass;

    public TMP_Text t_LobbyN;

    //Joining:
    //public Server tempServer;
    public TMP_InputField IF_JoinPass;

    //LOBBY CODE:
    public int i_TotRed;
    public int i_TotBlue;
    public int i_CurrPlacement;
    public bool b_Ready = false;
    public bool b_Change = false;

    public string[] PlayerNames = new string[4];
    public int[] RedPlaces = new int[2];
	public int[] BluePlaces = new int[2];

    public TMP_Text[] tmp_Texts = new TMP_Text[4];
    //public TMP_Text tmp_R1;
    //public TMP_Text tmp_R2;
    //public TMP_Text tmp_B1;
    //public TMP_Text tmp_B2;

    public Toggle tog_R1;
    public Toggle tog_R2;
    public Toggle tog_B1;
    public Toggle tog_B2;

    public GameObject servermanager;
    public GameObject clientmanager;

    public string s_Default = "Waiting for player..";

    //Probably a temp variable.
    public Team i_CurrTeam;

	public Hashtable LobbyPlayers = new Hashtable();

    private void OnDestroy()
    {
        servermanager.GetComponent<ServerScript>().sceneStates = ServerScript.SceneStates.GameScene;
        clientmanager.GetComponent<ClientScript>().sceneStates = ClientScript.SceneStates.GameScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(servermanager);
        DontDestroyOnLoad(clientmanager);
        //Get the server data here.
        i_TotRed = 0;
        i_TotBlue = 0;
        RedPlaces[0] = 0;
        RedPlaces[1] = 2;
        BluePlaces[0] = 1;
		BluePlaces[1] = 3;

        Playername = PlayerPrefs.GetString("PlayerName");

        //if (i_TotRed >= 2)
        //{
        //    i_CurrTeam = 1;
        //    i_TotBlue++;
        //    i_CurrPlacement = i_TotBlue;
        //    if(i_TotBlue <= 1)
        //    {
        //        tmp_B1.text = Playername;
        //    }
        //    else
        //    {
        //
        //        tmp_B2.text = Playername;
        //    }
        //}
        //else
        //{
        //    i_CurrTeam = 0;
        //    i_TotRed++;
        //    i_CurrPlacement = i_TotRed;
        //    if (i_TotRed <= 1)
        //    {
        //        tmp_R1.text = Playername;
        //    }
        //    else
        //    {
        //
        //        tmp_R2.text = Playername;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {

		//tmp_Texts[0].text = PlayerNames[0];
		//tmp_Texts[1].text = PlayerNames[1];
		//tmp_Texts[2].text = PlayerNames[2];
		//tmp_Texts[3].text = PlayerNames[3];

		for (int c = 0; c < PlayerNames.Length; c++)
		{
			PlayerNames[c] = "";
			//tmp_Texts[PlayerPlaces[c]].text = PlayerNames[c];
			
		}

		tog_R1.isOn = false;
		tog_R2.isOn = false;
		tog_B1.isOn = false;
		tog_B2.isOn = false;

		foreach (string k in LobbyPlayers.Keys)
		{
			LobbyClient c = (LobbyClient)LobbyPlayers[k];

			PlayerNames[c.position] = c.name;

			switch (c.position)
			{
				case 0:
					tog_R1.isOn = c.b_ready;
					break;
				case 2:
					tog_R2.isOn = c.b_ready;
					break;
				case 1:
					tog_B1.isOn = c.b_ready;
					break;
				case 3:
					tog_B2.isOn = c.b_ready;
					break;
			}
		}

        for (int c = 0; c < PlayerNames.Length; c++)
        {
			if (PlayerNames[c] != "")
			{
				tmp_Texts[c].text = PlayerNames[c];

			}
			else
			{
				tmp_Texts[c].text = "Waiting For Player...";
			}
            //tmp_Texts[PlayerPlaces[c]].text = PlayerNames[c];
        }




        if (b_Change)
        {
            switch (i_CurrTeam)
            {
                case Team.Red:
                    if (i_CurrPlacement > i_TotRed)
                    {
                        i_CurrPlacement = i_TotRed;
                       // tmp_R1.text = Playername;
                       // tmp_R2.text = s_Default;
                    }
                    break;
                case Team.Blu:
                    if (i_CurrPlacement > i_TotBlue)
                    {
                        i_CurrPlacement = i_TotBlue;
                       // tmp_B1.text = Playername;
                       // tmp_B2.text = s_Default;
                    }
                    break;
            }

            b_Change = false;
        }
        //Playername = IF_PlayerName.text;
        LobName = IF_LobbyName.text;
        t_LobbyN.text = LobName;
        pass = IF_LobbyPass.text;
    }

    public void Ready()
    {
        //Find out if I should make this code!!!
		b_Ready = !b_Ready;

		/*switch (i_CurrPlacement)
        {
            case 0:
                tog_R1.isOn = b_Ready;
                break;
            case 2:
                tog_R2.isOn = b_Ready;
                break;
            case 1:
                tog_B1.isOn = b_Ready;
                break;
            case 3:
                tog_B2.isOn = b_Ready;
                break;
        }*/

		LobbyClient pC = (LobbyClient)LobbyPlayers[ID];
		pC.b_ready = b_Ready;

		LobbyPlayers[ID] = pC;

		if (clientmanager.activeSelf)
			clientmanager.GetComponent<ClientScript>().LobbyMoved();
		else if (servermanager.activeSelf)
			servermanager.GetComponent<ServerScript>().LobbyMoved();
	}

	public void RemovePlayer(string key)
	{
		LobbyPlayers.Remove(key);
	}
    public void UpdateThisText(string s_input)
    {
        /*switch(i_CurrTeam)
        {
            case Team.Red:
                switch (i_CurrPlacement)
                {
                    case 1:
                       // tmp_R1.text = s_input;
                        break;
                    case 2:
                       // tmp_R2.text = s_input;
                        break;
                }
                break;
            case Team.Blu:
                switch (i_CurrPlacement)
                {
                    case 1:
                        //tmp_B1.text = s_input;
                        break;
                    case 2:
                       // tmp_B2.text = s_input;
                        break;
                }
                break;
        }*/
    }


    public void ChangeTeam()
    {
		/*
        int temp;
        temp = PlayerPlaces[0];
        PlayerPlaces[0] = PlayerPlaces[2];
        PlayerPlaces[2] = temp;

        temp = PlayerPlaces[1];
        PlayerPlaces[1] = PlayerPlaces[3];
        PlayerPlaces[3] = temp;

        clientmanager.GetComponent<ClientScript>().PlayerPlaces = PlayerPlaces;
        servermanager.GetComponent<ServerScript>().PlayerPlaces = PlayerPlaces;
		*/

		int[] team = RedPlaces;
		Team newTeam = Team.Red;

		switch (i_CurrTeam)
		{
			case Team.Red:
				team = BluePlaces;
				newTeam = Team.Blu;
				break;
			case Team.Blu:
				team = RedPlaces;
				newTeam = Team.Red;
				break;
		}

		for (int i = 0; i < 2; i++)
		{
			if (PlayerNames[team[i]] == "")
			{
				LobbyClient nC = new LobbyClient();
				nC.name = Playername;
				nC.position = team[i];

				LobbyPlayers[ID] = nC;
				//PlayerNames[i_CurrPlacement] = "";
				//PlayerNames[team[i]] = Playername;

				if (b_Ready)
				{
					b_Ready = false;

					switch (i_CurrPlacement)
					{
						case 0:
							tog_R1.isOn = b_Ready;
							break;
						case 2:
							tog_R2.isOn = b_Ready;
							break;
						case 1:
							tog_B1.isOn = b_Ready;
							break;
						case 3:
							tog_B2.isOn = b_Ready;
							break;
					}
				}


				i_CurrPlacement = team[i];

				i_CurrTeam = newTeam;
				break;
			}
		}

		if (clientmanager.activeSelf)
			clientmanager.GetComponent<ClientScript>().LobbyMoved();
		else if(servermanager.activeSelf)
			servermanager.GetComponent<ServerScript>().LobbyMoved();

		//if (b_Ready)
		//    Ready();
		//
		//UpdateThisText(s_Default);
		//
		//switch (i_CurrTeam)
		//{
		//    case 0:
		//        i_CurrTeam = 1;
		//        i_TotRed--;
		//        i_TotBlue++;
		//        i_CurrPlacement = i_TotBlue;
		//        b_Change = true;
		//        break;
		//    case 1:
		//        i_CurrTeam = 0;
		//        i_TotRed++;
		//        i_TotBlue--;
		//        i_CurrPlacement = i_TotRed;
		//        b_Change = true;
		//        break;
		//}
		//UpdateThisText(Playername);
	}

    //public void StartServer()
    //{
    //
    //    toHost.name = IF_LobbyName.text;
    //    toHost.pass = IF_LobbyPass.text;
    //    toHost.Playername = IF_PlayerName.text;
    //    toHost.isPrivate = t_IsPriv.isOn;
    //    toHost.isJoining = false;
    //
    //    t_LobbyN.text = IF_LobbyName.text;
    //
    //}
    //
    //public void JoinServer()
    //{
    //    toJoin.name = tempServer.name;
    //    toJoin.ID = tempServer.ID;
    //    toJoin.pass = IF_LobbyPass.text;
    //    toJoin.Playername = IF_PlayerName.text;
    //    toJoin.isPrivate = tempServer.isPrivate;
    //    toJoin.isJoining = true;
    //
    //
    //}
}
