using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

using System;
using System.Text;

using TMPro;

public class LobbyBrowserScript : MonoBehaviour
{
	private static byte[] inBuffer = new byte[512];
	private static byte[] outBuffer;
	private Socket server;
	public ClientScript clientmanager;

	[Header("UI Elements")]
	public GameObject BrowserWindow;
	public GameObject lobbyList;
	public GameObject LobbyEntryTemplate;
	public GameObject ConnectingPrompt;
	public GameObject PasswordWindow;
	public TMP_InputField PasswordInput;

	[Header("Host Elements")]
	public GameObject HostPopup;
	public TMP_InputField HostLobbyName;
	public TMP_InputField HostLobbyPassword;
	public Toggle HostLobbyPrivate;

	[Header("Lobby Elements")]
	public string LobbyId = "";
	public GameObject LobbyMenu;
	public GameObject SelectedLobby = null;
	public LobbyScript lobbyScript;

	[Header("Direct Connect")]
	public GameObject DirectConnectWindow;
	public InputField DirectConnectIP;

	[Header("Debug Settings")]
	public bool localServer = false;

	bool connected = false;
	bool hosting = false;

	//Dylan additions
	bool b_Destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
		Connect();

		if (connected)
		{
			RefreshBrowser();
		}
		else
		{
			ErrorPrompt("ERR: Error Connecting to the server.");
		}
	}

	private void Connect()
	{
		server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		if (!localServer)
		{
			IPHostEntry serverEntry = Dns.GetHostEntry("ec2-3-142-27-138.us-east-2.compute.amazonaws.com");

			IPAddress ip = null;
			foreach (IPAddress i in serverEntry.AddressList)
			{
				if (i.AddressFamily == AddressFamily.InterNetwork)
					ip = i;
			}

			if (ip != null)
				server.Connect(ip, 11110);
		}
		else
		{
			server.Connect(IPAddress.Parse("127.0.0.1"), 11110);
		}

		server.Blocking = false;

		connected = server.Connected;
	}

	// Update is called once per frame
	void Update()
    {
		if (connected)
		{
			try
			{
				while (server.Available > 0)
				{
					int rec = server.Receive(inBuffer);
					string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);
					Debug.Log(msg);

					string[] data = msg.Split(';');
					string code = data[0];

					if (code.Contains("[updatelobby]"))
					{
						string id = data[1];
						string lobbyName = data[2];
						string num_players = data[3];
						string max_players = data[4];
						string status = data[5];

						Transform ui_elem_exists = lobbyList.transform.Find(id);

						SessionPrefabVars handles;

						if (ui_elem_exists != null)
						{
							handles = ui_elem_exists.gameObject.GetComponent<SessionPrefabVars>();
						}
						else
						{
							GameObject newUiElem = GameObject.Instantiate(LobbyEntryTemplate);
							newUiElem.name = id;
							newUiElem.transform.SetParent(lobbyList.transform, false);
							//newUiElem.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lobbyList.GetComponent<RectTransform>().sizeDelta.x);
							newUiElem.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(lobbyList.GetComponent<RectTransform>().sizeDelta.x), 30.0f);
							newUiElem.GetComponent<RectTransform>().localPosition = new Vector3(0, 58 - 29 * (lobbyList.transform.childCount - 1), 0);
							
							handles = newUiElem.GetComponent<SessionPrefabVars>();
						}

						handles.browser = this;
						handles.LobbyName.text = lobbyName;
						handles.ActivePlayers.text = num_players + "/" + max_players;
						if (status.Contains("Open"))
						{
							handles.PrivateMarker.isOn = false;
						}
						else if (status.Contains("Private"))
						{
							handles.PrivateMarker.isOn = true;
						}
						else if (status.Contains("Closed"))
						{
							handles.gameObject.GetComponent<Selectable>().interactable = false;
							//GameObject.Destroy(ui_elem_exists.gameObject);
						}
					} else if (code.Contains("[lobbycreated]")) {
						LobbyId = data[1];
						LobbyMenu.SetActive(true);
						HostPopup.SetActive(false);
						hosting = true;
					} else if (code.Contains("[connect]")) {
						string hostip = data[1];
						PlayerPrefs.SetString("IPConnect", hostip);
						clientmanager.RunClient();
						clientmanager.gameObject.SetActive(true);
						//ConnectedToLobby();

						ConnectingPrompt.SetActive(false);
						BrowserWindow.SetActive(false);
						LobbyMenu.SetActive(true);
					} else if (code.Contains("[err]")) {
						string errMsg = data[1];
						ErrorPrompt(errMsg);
					}
				}
			} catch (Exception e) {

			}
		}

		if (server == null)
			Connect();
    }

	public void ErrorPrompt(string err)
	{
		Debug.Log(err);
	}

	public void RefreshBrowser()
	{
		foreach (Transform child in lobbyList.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		if (connected)
		{
			string msg = "[fetch];";

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);
		}
	}

	public void HostLobby()
	{
		try
		{
			string outmsg = "[host];" + HostLobbyName.text + ";4";

			if (HostLobbyPrivate.isOn)
				outmsg += ";" + HostLobbyPassword.text;

			outBuffer = Encoding.ASCII.GetBytes(outmsg);

			server.Send(outBuffer);
		}
		catch (Exception e)
		{
			ErrorPrompt(e.Message);
		}

	}

	public void ConnectToLobby()
	{
		if (SelectedLobby != null)
		{
			if (SelectedLobby.GetComponent<SessionPrefabVars>().PrivateMarker.isOn)
			{
				PasswordWindow.SetActive(true);
			}
			else
			{
				ConnectRequest();
			}
		}
	}

	public void PasswordConnect()
	{
		string pass = PasswordInput.text;

		if (pass.Length > 0)
		{
			ConnectRequest(pass);

			PasswordWindow.SetActive(false);
		}
	}

	private void ConnectRequest(string password = "")
	{
		if (SelectedLobby != null)
		{
			string msg = "[connect];" + SelectedLobby.name;

			if (password.Length > 0)
				msg += ";" + password;

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);

			ConnectingPrompt.SetActive(true);
		}
	}

	public void CloseLobby()
	{
		if (LobbyId != "" && hosting)
		{
			string msg = "[closelobby];" + LobbyId;

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);

			hosting = false;
			LobbyId = "";
		}
	}

	public void ConnectedToLobby()
	{
		if (SelectedLobby != null)
		{
			string msg = "[connected];" + SelectedLobby.name;

			LobbyId = SelectedLobby.name;

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);
		}
	}

	public void DisconnectFromLobby()
	{
		clientmanager.CloseConnection();
		clientmanager.gameObject.SetActive(false);

		LobbyId = "";
		
	}

	public void DisconnectedFromLobby()
	{
		if (LobbyId != "")
		{
			string msg = "[disconnected];" + LobbyId;

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);
		}
	}

	public void DirectConnect()
	{
		string ip = DirectConnectIP.text;

		PlayerPrefs.SetString("IPConnect", ip);
		clientmanager.RunClient();
		clientmanager.gameObject.SetActive(true);
		//ConnectedToLobby();

		DirectConnectWindow.SetActive(false);
		ConnectingPrompt.SetActive(false);
		BrowserWindow.SetActive(false);
		LobbyMenu.SetActive(true);
	}

	public void ServerShutdown()
	{
		if (LobbyId != "")
		{
			string msg = "[servershutdown];" + LobbyId;

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);

			LobbyId = "";
		}
	}

	public void ExitLobby()
	{
		if (LobbyId.Length > 0)
		{
			foreach (string k in lobbyScript.LobbyPlayers.Keys)
			{
				lobbyScript.LobbyPlayers.Remove(k);
			}

			if (hosting)
			{
				CloseLobby();
			}
			else
			{
				if (b_Destroyed)
					ServerShutdown();
				else
					DisconnectFromLobby();
			}
		}
	}

    public void OnApplicationQuit()
    {
		b_Destroyed = true;
		ExitLobby();
    }
}
