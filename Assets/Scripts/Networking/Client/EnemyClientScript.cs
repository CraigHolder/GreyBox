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

public class EnemyClientScript : MonoBehaviour
{
	public player_controller_behavior Player;
	public Transform OtherList;

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

	public void RunClient()
	{
		string config_path = "config.txt";

		StreamReader config_reader = new StreamReader(config_path, true);

		string line = config_reader.ReadLine();
		string server_ip_string = "127.0.0.1";

		if (line.Contains("ip:"))
		{
			server_ip_string = line.Split(':')[1];
		}

		try
		{
			IPAddress serverIp = IPAddress.Parse(server_ip_string);
			//IPAddress serverIp = IPAddress.Parse("69.157.101.135");
			//IPAddress serverIp = IPAddress.Parse("192.168.2.48");
			//IPAddress serverIp = IPAddress.Parse("127.0.0.1");
			remoteEP = new IPEndPoint(serverIp, 11111);
			serverEP = (EndPoint)remoteEP;

			client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//client.ReceiveTimeout = 2;

			outBuffer = Encoding.ASCII.GetBytes("[connect];");
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

		RunClient();

		updateTime = 1.0f / (float)UpdateFramesPerSec;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}


		if (!disconnected)
		{

			updateTimer += Time.deltaTime;

			if (updateTimer >= updateTime && connected)
			{
				updateTimer -= updateTime;

				Vector3 pos = Player.handle.position;

				if ((pos - prev_position).magnitude > 0.0f)
				{
					//init msg with the player id
					string msg = "[setpos];" + myId + ";";

					msg += JsonUtility.ToJson(pos) + ";" + Player.GetPlayerOrientation().ToString();

					for (int c = 0; c < Player.trail.Length; c++)
					{
						msg += ";" + JsonUtility.ToJson(Player.trail[c].position);
					}

					//Debug.Log(msg);

					outBuffer = Encoding.ASCII.GetBytes(msg);
					client.SendTo(outBuffer, remoteEP);

					prev_position = pos;
				}
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

						client_obj.name = data[1];
						client_obj.transform.parent = OtherList;
						num_players++;
					}
					else
					{
						client_obj = client_trans.gameObject;
					}

					PuppetScript client = client_obj.GetComponent<PuppetScript>();

					client.Root.position = JsonUtility.FromJson<Vector3>(data[2]);
					client.orientation = float.Parse(data[3]);

					for (int c = 0; c < data.Length - 4; c++)
					{
						client.trail[c].position = JsonUtility.FromJson<Vector3>(data[c + 4]);
					}

					client.UpdatePos();
				}
				else if (msg.Contains("[setname]"))
				{
					ConnectingMsg.SetActive(false);

					string[] data = msg.Split(';');

					myId = data[1];

					//CurNumPlayers.text = "1";
					//PlayerCount.SetActive(true);

					connected = true;
				}
				else if (msg.Contains("[settings]"))
				{
					string[] data = msg.Split(';');

					//MaxNumPlayers.text = data[1];
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

			}
			catch (Exception e)
			{

			}
		}
	}

	private void OnApplicationQuit()
	{
		if (!disconnected)
		{
			outBuffer = Encoding.ASCII.GetBytes("[disconnect];" + myId);

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
}
