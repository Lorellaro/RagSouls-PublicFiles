using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
	[SerializeField] bool isOfflineMode = false;

    // Start is called before the first frame update
    void Start()
    {
		if (isOfflineMode)
		{
			PhotonNetwork.OfflineMode = true;
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}

	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		SceneManager.LoadScene("Lobby");
	}
}
