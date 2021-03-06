﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class ServerScript : MonoBehaviour
{
	public enum SceneStates
	{
		LobbyScene,
		GameScene
    }

	public LobbyScript lobbyscript;
	public LobbyBrowserScript lobbyBrowserManager;

	[Header("Host Config")]
	public player_controller_behavior host_player;
	private string s_hostName;
	private int spawn_pos = -1;

	private Vector3 prev_position;

	public int UpdateFramesPerSec = 30;
	private float updateTime;
	private float updateTimer = 0.0f;

	private const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	[Header("Clients Config")]
	public GameObject ClientPrefab;
	public Transform ClientList;
	public Transform OtherObjList;
	public GameObject remote;

	[Header("Server Config")]
	public int IdLength = 10;
	public int MaxUsers = 10;
	public bool RunLocal = false;

	private static byte[] inBuffer;
	private static byte[] outBuffer;

	private static IPHostEntry host;
	private static IPAddress ip;
	private static IPEndPoint localEP;
	private static Socket server;

	private static IPEndPoint client;
	private static EndPoint remoteClient;

	private static Hashtable client_endpoints = new Hashtable();

	bool initcos = false;
	bool start = false;
	bool lobbystart = false;
	public int curr_ID = 2;
	public int play_ID = 1;
	public int numReady = 0;
	bool ready = false;
	public GameObject redNest;
	public GameObject blueNest;
	public int[] PlayerPlaces = new int[4];
	bool sendobjupdate = false;

	//Dylan's Lobby Stuff
	public bool b_FoundObjs = false;
	public SceneStates sceneStates = SceneStates.LobbyScene;
	public float f_lobbyStartDelay = 0.1f;
	private float f_lobbyStartTimer = 0.0f;

	public void RunServer()
	{
		inBuffer = new byte[2048];

		host = Dns.GetHostEntry(Dns.GetHostName());
		if (RunLocal)
		{
			ip = IPAddress.Parse("127.0.0.1");
		}
		else
		{
			//ip = host.AddressList[1];
			foreach (IPAddress i in host.AddressList)
			{
				Debug.Log(i.ToString());
				if (i.AddressFamily == AddressFamily.InterNetwork)
				{
					ip = i;
				}
			}
		}

		Debug.Log("Server: " + host.HostName + " | IP: " + ip);

		localEP = new IPEndPoint(ip, 11111);

		server = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
		//server.ReceiveTimeout = 5;
		server.Blocking = false;

		client = new IPEndPoint(IPAddress.Any, 0);
		remoteClient = (EndPoint)client;

		server.Bind(localEP);

		Debug.Log("Waiting for Data.");

		lobbystart = false;
	}


	// Start is called before the first frame update
	void Start()
    {
		s_hostName = "";	
		for (int i = 0; i < IdLength; i++)
		{
			s_hostName += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
		}

		updateTime = 1.0f / (float)UpdateFramesPerSec;

		RunServer();
    }

    // Update is called once per frame
    void Update()
    {
		//if (Input.GetKeyDown(KeyCode.Escape))
		//{
		//	Application.Quit();
		//}

		switch (sceneStates)
		{
			case SceneStates.GameScene:
                {
					if(!start)
                    {
						OtherObjList = GameObject.FindGameObjectWithTag("ItemList").transform;
						host_player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<player_controller_behavior>();
						remote = GameObject.FindGameObjectWithTag("Remote");
						redNest = GameObject.FindGameObjectWithTag("RedNest");
						blueNest = GameObject.FindGameObjectWithTag("BlueNest");

						GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
						playerObj.SetActive(false);

						switch (spawn_pos)
						{
							case 0:// Red 1
								playerObj.transform.position = GameObject.FindGameObjectWithTag("RedNest").transform.Find("SpawnR1").position;
								playerObj.transform.rotation = GameObject.FindGameObjectWithTag("RedNest").transform.Find("SpawnR1").rotation;
								break;
							case 1: // Blue 1
								playerObj.transform.position = GameObject.FindGameObjectWithTag("BlueNest").transform.Find("SpawnB1").position;
								playerObj.transform.rotation = GameObject.FindGameObjectWithTag("BlueNest").transform.Find("SpawnB1").rotation;
								break;
							case 2: // Red 2
								playerObj.transform.position = GameObject.FindGameObjectWithTag("RedNest").transform.Find("SpawnR2").position;
								playerObj.transform.rotation = GameObject.FindGameObjectWithTag("RedNest").transform.Find("SpawnR2").rotation;
								break;
							case 3: // Blue 2
								playerObj.transform.position = GameObject.FindGameObjectWithTag("BlueNest").transform.Find("SpawnB2").position;
								playerObj.transform.rotation = GameObject.FindGameObjectWithTag("BlueNest").transform.Find("SpawnB2").rotation;
								break;
							default:
								break;
						}
						playerObj.SetActive(true);

						start = true;
						InitFerretCosmetics();
						initcos = true;
						//for (int x = 0; x < ClientList.childCount; x++)
						//{
						//	//if (ClientList.GetChild(x).GetComponentInChildren<PuppetScript>().PlayId == 1)
						//		for (int c = 0; c < PlayerPlaces.Length; c++)
						//		{
						//		
						//			//tmp_Texts[PlayerPlaces[c]].text = PlayerNames[c];
						//			//ClientList.GetChild([PlayerPlaces[c]).transform.position;
						//		}
						//	
						//}

					}



					updateTimer += Time.deltaTime;

					if (updateTimer >= updateTime)
					{
						updateTimer -= updateTime;

						Vector3 pos = host_player.handle.position;

						if ((pos - prev_position).magnitude > 0.0f)
						{
							//init msg with the player id
							string msg = "[updatepos];" + s_hostName + ";";
							msg += JsonUtility.ToJson(pos) + ";" + host_player.GetPlayerOrientation().ToString() + ";" + host_player.joystick_x.ToString() + ";" + host_player.joystick_y.ToString();


							outBuffer = Encoding.ASCII.GetBytes(msg);

							//Debug.Log(msg);
							for (int c = 0; c < ClientList.childCount; c++)
							{
								string user = ClientList.GetChild(c).name;
								EndPoint remote_client = (EndPoint)client_endpoints[user];
								for (int x = 0; x < host_player.trail.Length; x++)
								{
									msg += ";" + JsonUtility.ToJson(host_player.trail[x].position);
								}

								outBuffer = Encoding.ASCII.GetBytes(msg);

								server.SendTo(outBuffer, remote_client);
								//Debug.Log(msg);
							}
						}
						if(sendobjupdate)
                        {
							Updateobjs();
							sendobjupdate = false;
						}
                        else
                        {
							sendobjupdate = true;
						}
						UpdateFerretState();
						//Updateobjs();
						
					}

					for (int y = 0; y < 20; y++)
					{
						try
						{
							int rec = server.ReceiveFrom(inBuffer, ref remoteClient);
							string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);

							Debug.Log(msg);
							//if (msg.ToLower().Contains("[connect]"))
							//{
							//	if (ClientList.childCount >= MaxUsers)
							//	{
							//		outBuffer = Encoding.ASCII.GetBytes("[disconnect];Server Full!");
							//
							//		server.SendTo(outBuffer, remoteClient);
							//		return;
							//	}
							//
							//	GameObject newClient = GameObject.Instantiate(ClientPrefab);
							//	newClient.transform.parent = ClientList;
							//
							//	Transform exists = null;
							//
							//	string name;
							//
							//	do
							//	{
							//		name = "";
							//
							//		for (int i = 0; i < IdLength; i++)
							//		{
							//			name += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
							//		}
							//
							//		exists = ClientList.Find(name);
							//	} while (exists != null);
							//
							//	newClient.name = name;
							//
							//	Debug.Log("New Client: " + name + " connected!");
							//
							//	// Send the new User's server ID back to the User.
							//	outBuffer = Encoding.ASCII.GetBytes("[setname];" + name);
							//
							//	client_endpoints[name] = remoteClient;
							//
							//	server.SendTo(outBuffer, remoteClient);
							//
							//	// Send Server Settings to the Client
							//	outBuffer = Encoding.ASCII.GetBytes("[settings];" + MaxUsers.ToString());
							//
							//	server.SendTo(outBuffer, remoteClient);
							//
							//	// Send the new user the position of all currently connected users.
							//	for (int c = 0; c < ClientList.childCount; c++)
							//	{
							//		string outmsg = "[updatepos];";
							//
							//		PuppetScript obj = ClientList.GetChild(c).gameObject.GetComponent<PuppetScript>();
							//
							//		if (obj.gameObject.name.CompareTo(name) == 0)
							//			continue;
							//
							//		outmsg += obj.gameObject.name + ";" + JsonUtility.ToJson(obj.Root.position) + ";" + obj.orientation.ToString();
							//
							//		for (int d = 0; d < obj.trail.Length; d++)
							//		{
							//			outmsg += ";" + JsonUtility.ToJson(obj.trail[d].position);
							//		}
							//
							//		outBuffer = Encoding.ASCII.GetBytes(outmsg);
							//
							//		server.SendTo(outBuffer, remoteClient);
							//	}
							//}
							if (msg.ToLower().Contains("[setpos]"))
							{
								string[] data = msg.Split(';');

								PuppetScript client = ClientList.Find(data[1]).GetComponent<PuppetScript>();
								client.DeadReckoning(float.Parse(data[4]), float.Parse(data[4]));
								client.Root.position = JsonUtility.FromJson<Vector3>(data[2]);
								client.orientation = float.Parse(data[3]);

								for (int c = 0; c < data.Length - 6; c++)
								{
									client.trail[c].position = JsonUtility.FromJson<Vector3>(data[c + 6]);
								}

								client.UpdatePos();

								if (ClientList.childCount > 1)
								{
									string outMsg = msg.Replace("[setpos]", "[updatepos]");

									outBuffer = Encoding.ASCII.GetBytes(outMsg);

									for (int c = 0; c < ClientList.childCount; c++)
									{
										Transform cur_client = ClientList.GetChild(c);
										string n = cur_client.gameObject.name;

										if (n.CompareTo(data[1]) == 0)
											continue;

										EndPoint out_client = (EndPoint)client_endpoints[n];

										server.SendTo(outBuffer, out_client);
									}
								}
							}
							if (msg.ToLower().Contains("[setcosmetics]"))
							{
								string[] data = msg.Split(';');
								PuppetScript client = ClientList.Find(data[1]).GetComponent<PuppetScript>();

								client.SetCosmetics(int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]),
									int.Parse(data[6]), int.Parse(data[7]), int.Parse(data[8]));

								//client.UpdatePos();

								if (ClientList.childCount > 1)
								{
									string outMsg = msg.Replace("[setcosmetics]", "[updatecosmetic]");

									outBuffer = Encoding.ASCII.GetBytes(outMsg);

									for (int c = 0; c < ClientList.childCount; c++)
									{
										Transform cur_client = ClientList.GetChild(c);
										string n = cur_client.gameObject.name;

										if (n.CompareTo(data[1]) == 0)
											continue;

										EndPoint out_client = (EndPoint)client_endpoints[n];

										server.SendTo(outBuffer, out_client);
									}
								}
							}
							else if (msg.ToLower().Contains("[setobjpos]"))
							{
								string[] data = msg.Split(';');

								Transform objparent = OtherObjList.Find(data[1]);
								Transform obj = objparent.GetChild(0);

								switch (obj.GetComponent<Score>().state)
								{
									case 0:
										switch (int.Parse(data[5]))
										{
											case 1:
												obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
												obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
												obj.GetComponent<Rigidbody>().velocity = JsonUtility.FromJson<Vector3>(data[6]);
												obj.GetComponent<Score>().networkedmoved = true;
												break;
											case 2:
												obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
												obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
												obj.GetComponent<Rigidbody>().velocity = JsonUtility.FromJson<Vector3>(data[6]);
												obj.GetComponent<Score>().networkedmoved = true;
												obj.GetComponent<Score>().grabbable = false;
												break;
										}
										break;
									case 1:
										switch (int.Parse(data[5]))
										{
											case 0:

												break;
											case 1:

												break;
											case 2:
												obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
												obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
												obj.GetComponent<Rigidbody>().velocity = JsonUtility.FromJson<Vector3>(data[6]);
												obj.GetComponent<Score>().networkedmoved = true;
												obj.GetComponent<Score>().grabbable = false;
												break;
										}
										break;
									case 2:
										switch (int.Parse(data[5]))
										{
											case 0:

												break;
											case 1:

												break;
											case 2:

												break;
										}
										break;
								}


								//if(!obj.GetComponent<Score>().moved || JsonUtility.FromJson<int>(data[5]) == true)
								//{
								//	obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
								//	obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
								//	obj.GetComponent<Score>().networkedmoved = true;
								//}

								if (ClientList.childCount > 0)
								{
									for (int c = 0; c < ClientList.childCount; c++)
									{
										//for (int u = 0; u < OtherObjList.childCount; u ++)
										//{
										//	Transform cur_obj = OtherObjList.GetChild(u);
										//
										//}
										Transform cur_client = ClientList.GetChild(c);
										string n = cur_client.gameObject.name;

										if (n.CompareTo(data[4]) == 0)
											continue;

										string outMsg = msg.Replace("[setobjpos]", "[updateobjpos]");
										outBuffer = Encoding.ASCII.GetBytes(outMsg);
										EndPoint out_client = (EndPoint)client_endpoints[n];

										server.SendTo(outBuffer, out_client);
									}
								}
							}
							if (msg.Contains("[setstate]"))
							{
								string[] data = msg.Split(';');

								Transform client_trans = ClientList.Find(data[1]);

								if (client_trans != null)
								{
									GameObject client_obj = client_trans.gameObject;

									PuppetScript client = client_obj.GetComponent<PuppetScript>();

									client.setState(int.Parse(data[2]));
									client.AnimateFerret();

									string outmsg = msg.Replace("[setstate]", "[updatestate]");
									outBuffer = Encoding.ASCII.GetBytes(outmsg);

									foreach (Transform child in ClientList)
									{
										if(child.name != data[1])
											server.SendTo(outBuffer, (EndPoint)client_endpoints[child.name]);
									}
								}
							}
							else if (msg.ToLower().Contains("[setspeaker]"))
							{
								string[] data = msg.Split(';');

								remote.GetComponent<Remote>().b_speakeron = bool.Parse(data[1]);
								if (remote.GetComponent<Remote>().b_speakeron == true)
								{
									remote.GetComponent<Remote>().mr_light.material = remote.GetComponent<Remote>().M_on;
									remote.GetComponent<Remote>().fly_shareddata.e_speakerstate = Speakers.SpeakerState.On;
								}
								else
								{
									remote.GetComponent<Remote>().mr_light.material = remote.GetComponent<Remote>().M_off;
									remote.GetComponent<Remote>().fly_shareddata.e_speakerstate = Speakers.SpeakerState.Off;
								}

								string outmsg = msg.Replace("[setspeaker]", "[updatespeaker]");
								outBuffer = Encoding.ASCII.GetBytes(outmsg);

								foreach (Transform child in ClientList)
								{
									server.SendTo(outBuffer, (EndPoint)client_endpoints[child.gameObject.name]);
								}
							}
							else if (msg.ToLower().Contains("[disconnect]"))
							{
								string[] data = msg.Split(';');

								Transform client = ClientList.Find(data[1]);

								Debug.Log("Client: " + data[1] + " disconnected.");

								GameObject.Destroy(client.gameObject);
								client_endpoints.Remove(data[1]);

								for (int c = 0; c < ClientList.childCount; c++)
								{
									string user = ClientList.GetChild(c).name;
									EndPoint remote_client = (EndPoint)client_endpoints[user];


									outBuffer = Encoding.ASCII.GetBytes("[cull];" + data[1]);

									server.SendTo(outBuffer, remote_client);
								}
							}
							Console.WriteLine("Received: {0}, From Client: {1}", msg, remoteClient);
						}
						catch (Exception e)
						{
							break;
						}
					}

					break;
				}
			case SceneStates.LobbyScene:
                {
					UpdateLobby();
				}
				break;
		}
	}
	/// <summary>
	// LOBBY SCRIPT
	/// </summary>
	public void UpdateLobby()
    {
		if(!lobbystart)
        {
			lobbyscript.ID = s_hostName;
			lobbyscript.PlayerNames[0] = lobbyscript.Playername;

			LobbyScript.LobbyClient nC = new LobbyScript.LobbyClient();
			nC.name = lobbyscript.Playername;
			nC.position = 0;

			lobbyscript.LobbyPlayers[s_hostName] = nC;
			lobbystart = true;
		}

		//CONNECTING TO LOBBY
		try
		{




			int rec = server.ReceiveFrom(inBuffer, ref remoteClient);
			string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);
			Debug.Log(msg);

			if (msg.ToLower().Contains("[connect]"))
			{
				string[] data = msg.Split(';');
				if (ClientList.childCount >= MaxUsers)
				{
					outBuffer = Encoding.ASCII.GetBytes("[disconnect];Server Full!");

					server.SendTo(outBuffer, remoteClient);
					return;
				}

				GameObject newClient = GameObject.Instantiate(ClientPrefab);
				newClient.transform.parent = ClientList;
				newClient.GetComponent<PuppetScript>().PlayId = curr_ID;
				Transform exists = null;

				string name;

				do
				{
					name = "";

					for (int i = 0; i < IdLength; i++)
					{
						name += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
					}

					exists = ClientList.Find(name);
				} while (exists != null);

				newClient.name = name;

				Debug.Log("New Client: " + name + " connected!");
				lobbyBrowserManager.ConnectedToLobby();

				int clientPos = 0;

				for (int c = 0; c < lobbyscript.PlayerNames.Length; c++)
				{
					if (lobbyscript.PlayerNames[c] == "")
					{
						//lobbyscript.PlayerNames[c] = data[1];
						clientPos = c;
						break;
					}
				}

				// Send the new User's server ID back to the User.
				outBuffer = Encoding.ASCII.GetBytes("[setname];" + name + ";" + clientPos.ToString());

				client_endpoints[name] = remoteClient;

				server.SendTo(outBuffer, remoteClient);

				LobbyScript.LobbyClient nC = new LobbyScript.LobbyClient();
				nC.name = data[1];
				nC.position = clientPos;

				lobbyscript.LobbyPlayers.Add(name, nC);

				//lobbyscript.PlayerNames[curr_ID - 1] = data[1];


				// Send Server Settings to the Client
				//outBuffer = Encoding.ASCII.GetBytes("[settings];" + curr_ID.ToString() + ";" + lobbyscript.PlayerNames[0] + ";" + lobbyscript.PlayerNames[1] + ";" + 
				//	lobbyscript.PlayerNames[2] + ";" + lobbyscript.PlayerNames[3]);

				//curr_ID++;
				//server.SendTo(outBuffer, remoteClient);

				// Send the new user the position of all currently connected users.

				string outmsg = "[updatepos];";
				outmsg += s_hostName + ";" + lobbyscript.Playername + ";" + lobbyscript.i_CurrPlacement.ToString() + ";" + lobbyscript.b_Ready.ToString();
				outBuffer = Encoding.ASCII.GetBytes(outmsg);

				server.SendTo(outBuffer, remoteClient);

				for (int c = 0; c < ClientList.childCount; c++)
				{
					outmsg = "[updatepos];";

					PuppetScript obj = ClientList.GetChild(c).gameObject.GetComponent<PuppetScript>();

					if (obj.gameObject.name.CompareTo(name) == 0)
						continue;

					outmsg += obj.gameObject.name + ";" + ((LobbyScript.LobbyClient)lobbyscript.LobbyPlayers[obj.gameObject.name]).name + ";" + ((LobbyScript.LobbyClient)lobbyscript.LobbyPlayers[obj.gameObject.name]).position.ToString() + ";" + ((LobbyScript.LobbyClient)lobbyscript.LobbyPlayers[obj.gameObject.name]).b_ready.ToString();

					outBuffer = Encoding.ASCII.GetBytes(outmsg);

					server.SendTo(outBuffer, remoteClient);

					outmsg = "[updatepos];" + name + ";" + data[1] + ";" + clientPos + ";" + false.ToString();
					outBuffer = Encoding.ASCII.GetBytes(outmsg);

					server.SendTo(outBuffer, (EndPoint)client_endpoints[obj.gameObject.name]);
				}
			} else if(msg.ToLower().Contains("[updatepos]"))
			{
				string[] data = msg.Split(';');

				Transform client_trans = ClientList.Find(data[1]);
				GameObject client_obj;

				client_obj = client_trans.gameObject;
				LobbyScript.LobbyClient nC = new LobbyScript.LobbyClient();
				nC.name = data[2];
				nC.position = int.Parse(data[3]);
				nC.b_ready = bool.Parse(data[4]);

				lobbyscript.LobbyPlayers[data[1]] = nC;

				foreach (Transform child in ClientList)
				{
					string cid = child.gameObject.name;
					if (string.Compare(cid, data[1]) != 0)
					{
						string outmsg = "[updatepos];";

						// obj = ClientList.GetChild(c).gameObject.GetComponent<PuppetScript>();

						outmsg += data[1] + ";" + nC.name + ";" + nC.position.ToString() + ";" + nC.b_ready.ToString();

						outBuffer = Encoding.ASCII.GetBytes(outmsg);

						server.SendTo(outBuffer, (EndPoint)client_endpoints[child.gameObject.name]);
					}
				}
			}
			else if (msg.ToLower().Contains("[disconnect]"))
			{
				string[] data = msg.Split(';');

				Transform client = ClientList.Find(data[1]);

				Debug.Log("Client: " + data[1] + " disconnected.");

				GameObject.Destroy(client.gameObject);
				client_endpoints.Remove(data[1]);

				lobbyBrowserManager.DisconnectedFromLobby();

				for (int c = 0; c < ClientList.childCount; c++)
				{
					string user = ClientList.GetChild(c).name;
					EndPoint remote_client = (EndPoint)client_endpoints[user];


					outBuffer = Encoding.ASCII.GetBytes("[cull];" + data[1]);

					server.SendTo(outBuffer, remote_client);
				}
			}
			else if (msg.Contains("[setready]"))
			{
				string[] data = msg.Split(';');
				numReady = int.Parse(data[1]);

				//MaxNumPlayers.text = data[1];
			}
		}
		catch (Exception e)
		{

		}

		bool b_everyoneReady = true;
		foreach (string k in lobbyscript.LobbyPlayers.Keys)
		{
			LobbyScript.LobbyClient c = (LobbyScript.LobbyClient)lobbyscript.LobbyPlayers[k];
			if (!c.b_ready)
				b_everyoneReady = false;
		}

		if (b_everyoneReady)
		{
			if (f_lobbyStartTimer < f_lobbyStartDelay)
			{
				f_lobbyStartTimer += Time.deltaTime;
			}
			else
			{

				string outmsg = "[startgame];";

				outBuffer = Encoding.ASCII.GetBytes(outmsg);

				foreach (Transform child in ClientList)
				{
					string id = child.gameObject.name;

					server.SendTo(outBuffer, (EndPoint)client_endpoints[id]);
				}

				StartGame();
			}
		}
		else
		{
			f_lobbyStartTimer = 0.0f;
		}
	}

	public void ReadyUpdate(int i)
    {
		if(ready == false && lobbyscript.b_Ready != false)
        {
			ready = true;
			string outmsg = "[updateready];";
			//outmsg += ;

			for (int c = 0; c < ClientList.childCount; c++)
			{
				string user = ClientList.GetChild(c).name;
				EndPoint remote_client = (EndPoint)client_endpoints[user];


				outBuffer = Encoding.ASCII.GetBytes(outmsg);

				server.SendTo(outBuffer, remote_client);
			}
		}
		else if(ready == true && lobbyscript.b_Ready != true)
        {
			ready = false;
			string outmsg = "[updateready];";
			outmsg += i.ToString();


			for (int c = 0; c < ClientList.childCount; c++)
			{
				string user = ClientList.GetChild(c).name;
				EndPoint remote_client = (EndPoint)client_endpoints[user];


				outBuffer = Encoding.ASCII.GetBytes(outmsg);

				server.SendTo(outBuffer, remote_client);
			}
		}

		
	}

	public void Updateobjs()
	{
		for (int u = 0; u < OtherObjList.childCount; u++)
		{
			Transform cur_objparent = OtherObjList.GetChild(u);
			Score cur_obj = null;
			if (cur_objparent.childCount > 0)
            {
				cur_obj = cur_objparent.GetChild(0).GetComponent<Score>();
			}
			else
            {
				cur_obj = cur_objparent.GetComponent<Score>();
			}

			if (cur_obj != null)
            {
				if (cur_obj.state != 0)
				{
					string msg = "[updateobjpos];" + cur_objparent.name + ";";

					msg += JsonUtility.ToJson(cur_obj.transform.position) + ";" + JsonUtility.ToJson(cur_obj.transform.eulerAngles) + ";" + cur_obj.state.ToString();

					
					for (int c = 0; c < ClientList.childCount; c++)
					{
						string user = ClientList.GetChild(c).name;
						EndPoint remote_client = (EndPoint)client_endpoints[user];


						outBuffer = Encoding.ASCII.GetBytes(msg);

						server.SendTo(outBuffer, remote_client);
						Debug.Log(msg);
					}
				}
			}
			
			
		}
		if (remote.GetComponent<Remote>().b_active == true)
        {
			string msg = "[updatespeaker];" + remote.GetComponent<Remote>().b_speakeron.ToString();

			for (int c = 0; c < ClientList.childCount; c++)
			{
				string user = ClientList.GetChild(c).name;
				EndPoint remote_client = (EndPoint)client_endpoints[user];


				outBuffer = Encoding.ASCII.GetBytes(msg);

				server.SendTo(outBuffer, remote_client);
				Debug.Log(msg);
			}
		}
	}

	public void UpdateFerretState()
    {
        try
        {
			int state = host_player.GetFerretState();

			string msg = "[updatestate];" + s_hostName + ";";
			msg += state.ToString() + ";";

			outBuffer = Encoding.ASCII.GetBytes(msg);

			//Debug.Log(msg);
			for (int c = 0; c < ClientList.childCount; c++)
			{
				string user = ClientList.GetChild(c).name;
				EndPoint remote_client = (EndPoint)client_endpoints[user];

				server.SendTo(outBuffer, remote_client);
			}
		}
		catch (Exception e)
        {

        }

	}

	public void ServerShutdown()
    {
		string msg = "[forcedshutdown];Server forcibly shut down by host.";

		outBuffer = Encoding.ASCII.GetBytes(msg);
		for (int c = 0; c < ClientList.childCount; c++)
		{
			string user = ClientList.GetChild(c).name;
			EndPoint remote_client = (EndPoint)client_endpoints[user];

			server.SendTo(outBuffer, remote_client);
		}
	}

	public void InitFerretCosmetics()
	{
		string msg = "[updatecosmetic];" + s_hostName + ";";

		//msg += PlayerPrefs.GetInt("Body").ToString() + ";" + PlayerPrefs.GetInt("Hat").ToString() + ";" + PlayerPrefs.GetInt("Mask").ToString() + ";" + 
		//	PlayerPrefs.GetInt("BlueColour").ToString() +";" + PlayerPrefs.GetInt("RedColour").ToString() +";" + PlayerPrefs.GetInt("Skin").ToString() + ";" + PlayerPrefs.GetInt("PlayerTeam");
		SettingAccessories acc = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<SettingAccessories>();
		//bCurrentItem
		//hCurrentItem
		//	mCurrentItem
		//i_Skini_PlayerTeam
		//	i_BlueColour
		//	i_RedColour
		msg += acc.bCurrentItem.ToString() + ";" + acc.hCurrentItem.ToString() + ";" + acc.mCurrentItem.ToString() + ";" +
		acc.i_BlueColour.ToString() +";" + acc.i_RedColour.ToString() +";" + acc.i_Skin.ToString() + ";" + acc.i_PlayerTeam.ToString();

		for (int c = 0; c < ClientList.childCount; c++)
		{
			string user = ClientList.GetChild(c).name;
			EndPoint remote_client = (EndPoint)client_endpoints[user];


			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.SendTo(outBuffer, remote_client);
			Debug.Log(msg);
		}

	}

	private void OnApplicationQuit()
	{
		ShutDownServer();
	}

	private void OnDestroy()
	{
		ShutDownServer();
	}

	public void ShutDownServer()
	{
		if (server != null)
		{
			Console.WriteLine("Server Shutting down...");

			for (int c = 0; c < ClientList.childCount; c++)
			{
				string user = ClientList.GetChild(c).name;
				EndPoint remote_client = (EndPoint)client_endpoints[user];


				outBuffer = Encoding.ASCII.GetBytes("[disconnect];Server Shutdown.");

				server.SendTo(outBuffer, remote_client);
			}

			server.Shutdown(SocketShutdown.Both);
			server.Close();
		}
	}

	public void LobbyMoved()
	{
		string msg = "[updatepos];";
		LobbyScript.LobbyClient pC = (LobbyScript.LobbyClient)lobbyscript.LobbyPlayers[lobbyscript.ID];

		msg += lobbyscript.ID + ";" + pC.name + ";" + pC.position.ToString() + ";" + pC.b_ready.ToString();

		outBuffer = Encoding.ASCII.GetBytes(msg);

		for (int c = 0; c < ClientList.childCount; c++)
		{
			string user = ClientList.GetChild(c).name;
			EndPoint remote_client = (EndPoint)client_endpoints[user];

			server.SendTo(outBuffer, remoteClient);
		}
	}

	public void StartGame()
	{
		lobbyBrowserManager.CloseLobby();
		spawn_pos = lobbyscript.i_CurrPlacement;
		PlayerPrefs.SetInt("PlayerTeam", (int)lobbyscript.i_CurrTeam);
		sceneStates = SceneStates.GameScene;

		DontDestroyOnLoad(this.gameObject);
		Command c_command = new GotoClientSceneCommand();
		c_command.Execute(c_command, null);
	}
}
