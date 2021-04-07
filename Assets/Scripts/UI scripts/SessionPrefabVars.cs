using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SessionPrefabVars : MonoBehaviour, ISelectHandler
{
	[Header("Handles")]
	public TMP_Text LobbyName;
	public TMP_Text ActivePlayers;
	public Toggle PrivateMarker;

	public LobbyBrowserScript browser;

	public void OnSelect(BaseEventData eventData)
	{
		browser.SelectedLobby = this.gameObject;
	}
}
