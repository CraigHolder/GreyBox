using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

using System;
using System.Text;

public class LobbyBrowserScript : MonoBehaviour
{
	private static byte[] inBuffer = new byte[512];
	private static byte[] outBuffer;
	private Socket server;

	public GameObject lobbyList;

	[Header("Debug Settings")]
	public bool localServer = false;

	bool connected = false;

    // Start is called before the first frame update
    void Start()
    {
		server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		server.Blocking = false;
		if (localServer)
		{
			server.Connect(IPAddress.Parse("ec2-3-142-27-138.us-east-2.compute.amazonaws.com"), 11110);
		}
		else
		{
			server.Connect(IPAddress.Parse("127.0.0.1"), 11110);
		}

		if (server.Connected)
		{
			string msg = "[fetch];";

			outBuffer = Encoding.ASCII.GetBytes(msg);

			server.Send(outBuffer);
		}
		else
		{
			ErrorPrompt("ERR: Error Connecting to the server.");
		}

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
					string msg = Encoding.ASCII.GetString(inBuffer);
					Debug.Log(msg);

					string[] data = msg.Split(';');
					string code = data[0];

					if (code.Contains("[updatelobby]"))
					{
						string id = data[1];
						string lobbyName = data[2];
						string num_players = data[3];
						string max_players = data[4];

						GameObject ui_elem = lobbyList.transform.Find(id).gameObject;

						//if()

					}
				}
			} catch (Exception e) {

			}
		}
    }

	public void ErrorPrompt(string err)
	{
		Debug.Log(err);
	}
}
