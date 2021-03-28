﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class ServerScript : MonoBehaviour
{
	[Header("Host Config")]
	public player_controller_behavior host_player;
	private string s_hostName;

	private Vector3 prev_position;

	public int UpdateFramesPerSec = 30;
	private float updateTime;
	private float updateTimer = 0.0f;

	private const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	[Header("Clients Config")]
	public GameObject ClientPrefab;
	public Transform ClientList;
	public Transform OtherObjList;

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
			ip = host.AddressList[1];
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
	}


	// Start is called before the first frame update
	void Start()
    {
		OtherObjList = GameObject.FindGameObjectWithTag("ItemList").transform;
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

		updateTimer += Time.deltaTime;

		if (updateTimer >= updateTime)
		{
			updateTimer -= updateTime;

			Vector3 pos = host_player.handle.position;

			if ((pos - prev_position).magnitude > 0.0f)
			{
				//init msg with the player id
				string msg = "[updatepos];" + s_hostName + ";";

				msg += JsonUtility.ToJson(pos) + ";" + host_player.GetPlayerOrientation().ToString();

				for (int c = 0; c < host_player.trail.Length; c++)
				{
					msg += ";" + JsonUtility.ToJson(host_player.trail[c].position);
				}

				outBuffer = Encoding.ASCII.GetBytes(msg);
				
				//Debug.Log(msg);
				for (int c = 0; c < ClientList.childCount; c++)
				{
					string user = ClientList.GetChild(c).name;
					EndPoint remote_client = (EndPoint)client_endpoints[user];

					server.SendTo(outBuffer, remote_client);
				}
			}

			UpdateFerretState();
			Updateobjs();
		}

		try
		{
			int rec = server.ReceiveFrom(inBuffer, ref remoteClient);
			string msg = Encoding.ASCII.GetString(inBuffer, 0, rec);

			if (msg.ToLower().Contains("[connect]"))
			{
				if (ClientList.childCount >= MaxUsers)
				{
					outBuffer = Encoding.ASCII.GetBytes("[disconnect];Server Full!");

					server.SendTo(outBuffer, remoteClient);
					return;
				}

				GameObject newClient = GameObject.Instantiate(ClientPrefab);
				newClient.transform.parent = ClientList;

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

				// Send the new User's server ID back to the User.
				outBuffer = Encoding.ASCII.GetBytes("[setname];" + name);

				client_endpoints[name] = remoteClient;

				server.SendTo(outBuffer, remoteClient);

				// Send Server Settings to the Client
				outBuffer = Encoding.ASCII.GetBytes("[settings];" + MaxUsers.ToString());

				server.SendTo(outBuffer, remoteClient);

				// Send the new user the position of all currently connected users.
				for (int c = 0; c < ClientList.childCount; c++)
				{
					string outmsg = "[updatepos];";

					PuppetScript obj = ClientList.GetChild(c).gameObject.GetComponent<PuppetScript>();

					if (obj.gameObject.name.CompareTo(name) == 0)
						continue;

					outmsg += obj.gameObject.name + ";" + JsonUtility.ToJson(obj.Root.position) + ";" + obj.orientation.ToString();

					for (int d = 0; d < obj.trail.Length; d++)
					{
						outmsg += ";" + JsonUtility.ToJson(obj.trail[d].position);
					}

					outBuffer = Encoding.ASCII.GetBytes(outmsg);

					server.SendTo(outBuffer, remoteClient);
				}
			}
			else if (msg.ToLower().Contains("[setpos]"))
			{
				string[] data = msg.Split(';');

				PuppetScript client = ClientList.Find(data[1]).GetComponent<PuppetScript>();
				client.Root.position = JsonUtility.FromJson<Vector3>(data[2]);
				client.orientation = float.Parse(data[3]);

				for (int c = 0; c < data.Length - 4; c++)
				{
					client.trail[c].position = JsonUtility.FromJson<Vector3>(data[c + 4]);
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
			else if (msg.ToLower().Contains("[setobjpos]"))
			{
				string[] data = msg.Split(';');

				Transform objparent = OtherObjList.Find(data[1]);
				Transform obj = objparent.GetChild(0);
				if (!obj.GetComponent<Score>().moved || JsonUtility.FromJson<Boolean>(data[5]) == true)
				{
					obj.transform.position = JsonUtility.FromJson<Vector3>(data[2]);
					obj.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(data[3]));
					obj.GetComponent<Score>().networkedmoved = true;
				}

				if (ClientList.childCount > 1)
				{
					string outMsg = msg.Replace("[setobjpos]", "[updateobjpos]");

					outBuffer = Encoding.ASCII.GetBytes(outMsg);

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

						EndPoint out_client = (EndPoint)client_endpoints[n];

						server.SendTo(outBuffer, out_client);
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

				for (int c = 0; c < ClientList.childCount; c++)
				{
					string user = ClientList.GetChild(c).name;
					EndPoint remote_client = (EndPoint)client_endpoints[user];


					outBuffer = Encoding.ASCII.GetBytes("[cull];" + data[1]);

					server.SendTo(outBuffer, remote_client);
				}
			}
			else if (msg.ToLower().Contains("[setstate]")) //FOR ANIMATIONS
			{
				string[] data = msg.Split(';');
				PuppetScript client = ClientList.Find(data[1]).GetComponent<PuppetScript>();

				int c_state = int.Parse(data[2]); //Should be state int

				client.setState(c_state);
				client.AnimateFerret();

				if (ClientList.childCount > 1)
				{
					string outMsg = msg.Replace("[setstate]", "[updatestate]");

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
			//Console.WriteLine("Received: {0}, From Client: {1}", msg, remoteClient);
			Debug.Log("Received: " + msg + " From Client: " + remoteClient);
		} catch (Exception e) {

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
				if (cur_obj.moved)
				{
					cur_obj.moved = false;

					string msg = "[updateobjpos];" + cur_objparent.name + ";";

					msg += JsonUtility.ToJson(cur_obj.transform.position) + ";" + JsonUtility.ToJson(cur_obj.transform.eulerAngles);

					
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
	}

	public void UpdateFerretState()
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

	private void OnApplicationQuit()
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