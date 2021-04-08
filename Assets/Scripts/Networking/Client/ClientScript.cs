using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Network stuff
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class ClientScript : MonoBehaviour
{
	public player_controller_behavior Player;
	public Transform OtherList;
	public Transform OtherObjList;

	public LobbyScript lobbyscript;
	public LobbyBrowserScript LobbyBrowserManager;

	public enum SceneStates
	{
		LobbyScene,
		GameScene
	}
	public SceneStates sceneStates = SceneStates.LobbyScene;

	public GameObject remote;

	private Vector3 prev_position;

	public GameObject OtherTemplate;

	public int UpdateFramesPerSec = 30;
	public int ConnectionTimeout = 10;

	

	//[Header("UI elements")]
	//public GameObject PlayerCount;
	//public TMP_Text CurNumPlayers;
	//public TMP_Text MaxNumPlayers;
	public GameObject ErrorPrompt;
	public TMP_Text ErrorMsg;
	public GameObject ConnectingMsg;

	private float updateTime;
	private float updateTimer = 0.0f;

	private string[] other_ids;
	private string myId;
	private int spawn_pos = -1;

	private static byte[] outBuffer = new byte[2048];
	private static byte[] inBuffer = new byte[2048];
	private static IPEndPoint remoteEP;
	private static EndPoint serverEP;
	private static Socket client;

	public float connection_timer = 0.05f;

	private bool connected = false; 
	private bool connection_tried = false;
	private bool disconnected = false;

	private int num_players = 1;

	bool initcos = false;
	bool start = false;
	public int PlayID = 0;
	bool ready = false;
	public int[] PlayerPlaces = new int[4];
	bool sendobjupdate = false;


	public void RunClient()
	{
		//string config_path = "config.txt";
		//
		//StreamReader config_reader = new StreamReader(config_path, true);
		//
		//string line = config_reader.ReadLine();
		//string server_ip_string = "127.0.0.1";
		//
		//if (line.Contains("ip:"))
		//{
		//	server_ip_string = line.Split(':')[1];
		//}

		try
		{
			//IPAddress serverIp = IPAddress.Parse(server_ip_string);
			IPAddress serverIp = IPAddress.Parse(PlayerPrefs.GetString("IPConnect"));
			Debug.Log(PlayerPrefs.GetString("IPConnect"));

			//IPAddress serverIp = IPAddress.Parse("69.157.101.135");
			//IPAddress serverIp = IPAddress.Parse("192.168.2.48");
			//IPAddress serverIp = IPAddress.Parse("127.0.0.1");
			remoteEP = new IPEndPoint(serverIp, 11111);
			serverEP = (EndPoint)remoteEP;

			client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//client.ReceiveTimeout = 2;

			outBuffer = Encoding.ASCII.GetBytes("[connect];" + lobbyscript.Playername);
			client.SendTo(outBuffer, remoteEP);

			//client.ReceiveTimeout = 1000 * ConnectionTimeout;

			//int rec = client.ReceiveFrom(inBuffer, ref serverEP);

			client.Blocking = false;

			/*string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);
			if (msg.Contains("[setname]"))
			{
				ConnectingMsg.SetActive(false);

				string[] data = msg.Split(';');

				myId = data[1];

				CurNumPlayers.text = "1";
				PlayerCount.SetActive(true);
			}
			else if (msg.Contains("[disconnect]"))
			{
				string[] data = msg.Split(';');

				NoConnection(data[1]);
			}
			else
				NoConnection("Error Connecting to Server.");
			*/
		} catch (Exception e) {
			NoConnection("Server Not Found.");
		}

	}

    // Start is called before the first frame update
    void Start()
    {
		ErrorPrompt.SetActive(false);
		//PlayerCount.SetActive(false);

		//RunClient();

		updateTime = 1.0f / (float)UpdateFramesPerSec;
    }

    // Update is called once per frame
    void Update()
    {
		//if (Input.GetKeyDown(KeyCode.Escape))
		//{
		//	Application.Quit();
		//}
		switch(sceneStates)
        {
			case SceneStates.GameScene:
                {
					if (!disconnected)
					{
						if (!start)
						{
							OtherObjList = GameObject.FindGameObjectWithTag("ItemList").transform;

							Player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<player_controller_behavior>();

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

							remote = GameObject.FindGameObjectWithTag("Remote");
							start = true;
							CosmeticsInit();
						}
						updateTimer += Time.deltaTime;

						if (updateTimer >= updateTime && connected)
						{
							updateTimer -= updateTime;

							Vector3 pos = Player.handle.position;

							if ((pos - prev_position).magnitude > 0.0f)
							{
								//init msg with the player id
								string msg = "[setpos];" + myId + ";";

								msg += JsonUtility.ToJson(pos) + ";" + Player.GetPlayerOrientation().ToString() + ";" + Player.joystick_x.ToString() + ";" + Player.joystick_y.ToString();

								for (int c = 0; c < Player.trail.Length; c++)
								{
									msg += ";" + JsonUtility.ToJson(Player.trail[c].position);
								}

								//Debug.Log(msg);

								outBuffer = Encoding.ASCII.GetBytes(msg);
								client.SendTo(outBuffer, remoteEP);

								prev_position = pos;
							}
							if (sendobjupdate)
							{
								Updateobjs();
								sendobjupdate = false;
							}
							else
							{
								sendobjupdate = true;
							}
							UpdateFerretState();
							
						}

						try
						{
							int rec = client.ReceiveFrom(inBuffer, ref serverEP);
							string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);

							if (msg.Contains("[updatepos]"))
							{
								string[] data = msg.Split(';');

								Transform client_trans = OtherList.Find(data[1]);
								GameObject client_obj;

								if (client_trans == null)
								{
									client_obj = GameObject.Instantiate(OtherTemplate);
									CosmeticsInit();
									client_obj.name = data[1];
									client_obj.transform.parent = OtherList;
									num_players++;
								}
								else
								{
									client_obj = client_trans.gameObject;
								}

								PuppetScript client = client_obj.GetComponent<PuppetScript>();
								client.DeadReckoning(float.Parse(data[4]), float.Parse(data[5]));
								client.Root.position = JsonUtility.FromJson<Vector3>(data[2]);
								client.orientation = float.Parse(data[3]);

								for (int c = 0; c < data.Length - 4; c++)
								{
									client.trail[c].position = JsonUtility.FromJson<Vector3>(data[c + 6]);
								}

								client.UpdatePos();
							}
							else if (msg.ToLower().Contains("[updatespeaker]"))
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
							}
							else if (msg.ToLower().Contains("[updatecosmetic]"))
							{
								string[] data = msg.Split(';');
								PuppetScript client = OtherList.Find(data[1]).GetComponent<PuppetScript>();
								
								client.SetCosmetics(int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]),
									int.Parse(data[6]), int.Parse(data[7]), int.Parse(data[8]));
							}
							else if (msg.Contains("[updatestate]"))
							{
								string[] data = msg.Split(';');

								Transform client_trans = OtherList.Find(data[1]);
								GameObject client_obj = client_trans.gameObject;

								PuppetScript client = client_obj.GetComponent<PuppetScript>();

								client.setState(int.Parse(data[2]));
								client.AnimateFerret();
							}
							else if (msg.Contains("[updateobjpos]"))
							{
								string[] data = msg.Split(';');
								Transform objparent = OtherObjList.Find(data[1]);
								Transform obj = objparent.GetChild(0);

								switch (obj.GetComponent<Score>().state)
								{
									case 0:
										switch (int.Parse(data[4]))
										{
											case 1:
												obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
												obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
												obj.GetComponent<Score>().networkedmoved = true;
												break;
											case 2:
												obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
												obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
												obj.GetComponent<Score>().networkedmoved = true;
												obj.GetComponent<Score>().grabbable = false;
												break;
										}
										break;
									case 1:
										switch (int.Parse(data[4]))
										{
											case 0:

												break;
											case 1:

												break;
											case 2:
												obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
												obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
												obj.GetComponent<Score>().networkedmoved = true;
												obj.GetComponent<Score>().grabbable = false;
												break;
										}
										break;
									case 2:
										switch (int.Parse(data[4]))
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

								//if (!obj.GetComponent<Score>().moved || JsonUtility.FromJson<Boolean>(data[4]) == true)
								//{
								//	obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
								//	obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
								//	obj.GetComponent<Score>().networkedmoved = true;
								//}


								//obj.GetComponent<Score>().moved = false;
								//Transform client_trans = OtherList.Find(data[1]);
								//GameObject client_obj;
								//client.UpdatePos();
							}
							else if (msg.Contains("[disconnect]"))
							{
								string[] data = msg.Split(';');

								NoConnection(data[1]);
							}
							else if (msg.Contains("[cull]"))
							{
								string[] data = msg.Split(';');

								Transform poorSoul = OtherList.Find(data[1]);
								GameObject.Destroy(poorSoul.gameObject);

								num_players--;

								//CurNumPlayers.text = num_players.ToString();
							}
							else if (msg.Contains("[forcedshutdown]")) 
							{
								string[] data = msg.Split(';');
								NoConnection(data[1]);
							}

						}
						catch (Exception e)
						{

						}
					}

					break;
                }
			case SceneStates.LobbyScene:
				{

					UpdateLobby();
					break;
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
				if (cur_obj.state == 2)
				{

					string msg = "[setobjpos];" + cur_objparent.name + ";";

					msg += JsonUtility.ToJson(cur_obj.transform.position) + ";" + JsonUtility.ToJson(cur_obj.transform.eulerAngles) + ";" + myId + ";" +  cur_obj.state.ToString() + ";" + JsonUtility.ToJson(cur_obj.GetComponent<Rigidbody>().velocity);

					outBuffer = Encoding.ASCII.GetBytes(msg);
					client.SendTo(outBuffer, remoteEP);
					Debug.Log(msg);
				}
			}
			
		}
		if (remote.GetComponent<Remote>().b_active == true)
		{
			string msg = "[setspeaker];" + remote.GetComponent<Remote>().b_speakeron.ToString() + ";" + myId;
			
			outBuffer = Encoding.ASCII.GetBytes(msg);
			client.SendTo(outBuffer, remoteEP);
			Debug.Log(msg);
		}
	}

	public void UpdateFerretState()
    {
		int state = Player.GetFerretState();

		string msg = "[setstate];" + myId + ";";
		msg += state.ToString() + ";";

		outBuffer = Encoding.ASCII.GetBytes(msg);

		//Debug.Log(msg);
		client.SendTo(outBuffer, remoteEP);
	}
	public void UpdateLobby()
    {
		try
		{
			int rec = client.ReceiveFrom(inBuffer, ref serverEP);
			string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);

			Debug.Log(msg);
			if (msg.Contains("[setname]"))
			{
				ConnectingMsg.SetActive(false);

				string[] data = msg.Split(';');

				myId = data[1];

				LobbyScript.LobbyClient nC = new LobbyScript.LobbyClient();
				nC.name = lobbyscript.Playername;
				nC.position = int.Parse(data[2]);
				if (nC.position % 2 > 0)
					lobbyscript.i_CurrTeam = LobbyScript.Team.Blu;
				else
					lobbyscript.i_CurrTeam = LobbyScript.Team.Red;
				lobbyscript.LobbyPlayers.Add(myId, nC);
				lobbyscript.ID = myId;
				lobbyscript.i_CurrPlacement = nC.position;

				LobbyBrowserManager.ConnectedToLobby();
				//CurNumPlayers.text = "1";
				//PlayerCount.SetActive(true);

				connected = true;
			} else if (msg.Contains("[updatepos]")) {
				string[] data = msg.Split(';');

				Transform client_trans = OtherList.Find(data[1]);
				GameObject client_obj;

				if (client_trans == null)
				{
					client_obj = GameObject.Instantiate(OtherTemplate);
					client_obj.name = data[1];
					client_obj.transform.parent = OtherList;
					num_players++;

					LobbyScript.LobbyClient nC = new LobbyScript.LobbyClient();
					nC.name = data[2];
					nC.position = int.Parse(data[3]);
					nC.b_ready = bool.Parse(data[4]);
					lobbyscript.LobbyPlayers.Add(data[1], nC);
				}
				else
				{
					client_obj = client_trans.gameObject;
					LobbyScript.LobbyClient nC = new LobbyScript.LobbyClient();
					nC.name = data[2];
					nC.position = int.Parse(data[3]);
					nC.b_ready = bool.Parse(data[4]);

					lobbyscript.LobbyPlayers[data[1]] = nC;
				}

			}
			else if (msg.Contains("[settings]"))
			{
				string[] data = msg.Split(';');
				PlayID = int.Parse(data[1]);
				lobbyscript.PlayerNames[0] = data[2];
				lobbyscript.PlayerNames[1] = data[3];
				lobbyscript.PlayerNames[2] = data[4];
				lobbyscript.PlayerNames[3] = data[5];
				//MaxNumPlayers.text = data[1];
			}
			else if (msg.Contains("[cull]"))
			{
				string[] data = msg.Split(';');
				string id = data[1];

				Transform poorSoul = OtherList.Find(data[1]);
				GameObject.Destroy(poorSoul.gameObject);

				lobbyscript.RemovePlayer(id);
			} else if (msg.Contains("[startgame]")) {
				StartGame();
			}
		}
		catch (Exception e)
		{

		}
	}

	public void ReadyUpdate(int i)
	{
		if (ready == false && lobbyscript.b_Ready != false)
		{
			ready = true;
			string outmsg = "[updateready];" + myId + "1";

			outBuffer = Encoding.ASCII.GetBytes(outmsg);

			//Debug.Log(msg);
			client.SendTo(outBuffer, remoteEP);
		}
		else if (ready == true && lobbyscript.b_Ready != true)
		{
			ready = false;
			string outmsg = "[updateready];" + myId + "0";
			//outmsg += i.ToString();

				outBuffer = Encoding.ASCII.GetBytes(outmsg);

				//Debug.Log(msg);
				client.SendTo(outBuffer, remoteEP);
		}


	}
	public void CosmeticsInit()
    {
		string msg = "[setcosmetics];" + myId + ";";
		SettingAccessories acc = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<SettingAccessories>();
		msg += acc.bCurrentItem.ToString() + ";" + acc.hCurrentItem.ToString() + ";" + acc.mCurrentItem.ToString() + ";" +
		acc.i_BlueColour.ToString() + ";" + acc.i_RedColour.ToString() + ";" + acc.i_Skin.ToString() + ";" + acc.i_PlayerTeam.ToString();

		outBuffer = Encoding.ASCII.GetBytes(msg);

		//Debug.Log(msg);
		client.SendTo(outBuffer, remoteEP);

	}

	private void OnApplicationQuit()
	{
		CloseConnection();
	}

	private void CloseConnection()
	{
		if (!disconnected)
		{
			string msg = "[disconnect];" + myId;

			if (sceneStates == SceneStates.LobbyScene)
				msg += ";" + lobbyscript.i_CurrPlacement.ToString();

			outBuffer = Encoding.ASCII.GetBytes(msg);
			client.SendTo(outBuffer, remoteEP);

			client.Shutdown(SocketShutdown.Both);
			client.Close();
		}
	}

	private void NoConnection(string msg)
	{
		ConnectingMsg.SetActive(false);

		disconnected = true;

		ErrorPrompt.SetActive(true);
		ErrorMsg.text = msg;

		client.Shutdown(SocketShutdown.Both);
		client.Close();
	}

	public void LobbyMoved()
	{
		string msg = "[updatepos];";
		LobbyScript.LobbyClient pC = (LobbyScript.LobbyClient)lobbyscript.LobbyPlayers[lobbyscript.ID];

		msg += lobbyscript.ID + ";" + pC.name + ";" + pC.position.ToString() + ";" + pC.b_ready.ToString();

		outBuffer = Encoding.ASCII.GetBytes(msg);

		client.SendTo(outBuffer, remoteEP);
	}

	public void StartGame()
	{
		DontDestroyOnLoad(this.gameObject);
		spawn_pos = lobbyscript.i_CurrPlacement;
		PlayerPrefs.SetInt("PlayerTeam", (int)lobbyscript.i_CurrTeam);
		sceneStates = SceneStates.GameScene;
		Command c_command = new GotoClientSceneCommand();
		c_command.Execute(c_command, null);
	}
}
